#include "GcDumpState.h"

#include "Windows.h"

#include <iomanip>
#include <iostream>

GcDumpState::GcDumpState()
    :
    _types(1024)
{
    _isStarted = false;
    _hasEnded = false;
    _collectionIndex = 0;
    _hEventStop = ::CreateEvent(nullptr, FALSE, FALSE, nullptr);
}

GcDumpState::~GcDumpState()
{
    if (_hEventStop != nullptr)
    {
        if (!::CloseHandle(_hEventStop))
        {
            std::cout << "Failed to close Stop event handle (0x"
                      << std::hex << ::GetLastError()
                      << ")" << std::endl;
        }
        _hEventStop = nullptr;
    }
}

void GcDumpState::Clear()
{
    // BUG: the types are not sent after the first gcdump  :^(
    //_types.clear();

    // keep the type ID but clear the instances
    for (auto& type : _types)
    {
        auto typeInfo = type.second;
        uint64_t instancesCount = typeInfo._instances.size();
        for (size_t i = 0; i < instancesCount; i++)
        {
            typeInfo._instances.clear();
        }
    }

    ::ResetEvent(_hEventStop);

    _isStarted = false;
    _hasEnded = false;
    _collectionIndex = 0;
}

void GcDumpState::DumpHeap()
{
    // iterate on _types and dump the instances count + size
    std::cout << std::endl
              << "Live heap dump" << std::endl;
    std::cout << "---------------------------------------------------------" << std::endl;
    std::cout << "    Count        Size  Type" << std::endl;
    for (auto& type : _types)
    {
        auto typeInfo = type.second;
        auto name = typeInfo._name;

        uint64_t instancesCount = typeInfo._instances.size();
        uint64_t instancesSize = 0;
        for (size_t i = 0; i < instancesCount; i++)
        {
            instancesSize += typeInfo._instances[i]._size;
        }
        std::cout << std::setfill(' ') << std::setw(9) << instancesCount << std::setw(12) << instancesSize << "  " << typeInfo._name << std::endl;
    }
}

void GcDumpState::OnGcStart(uint32_t index, uint32_t generation, GCReason reason, GCType type)
{
    if ((generation == 2) && (reason == GCReason::Induced) && (type == GCType::NonConcurrentGC))
    {
        _isStarted = true;
        _collectionIndex = index;
    }
}

void GcDumpState::OnGcEnd(uint32_t index, uint32_t generation)
{
    if (!_isStarted)
    {
        return;
    }

    if (index == _collectionIndex)
    {
        _isStarted = false;
        _hasEnded = true;

        ::SetEvent(_hEventStop);

        DumpHeap();
    }
}

void GcDumpState::OnTypeMapping(uint64_t id, uint32_t nameId, std::string name)
{
    if (!_isStarted)
    {
        return;
    }

    TypeInfo info;
    info.SetId(id);
    info.SetName(name);
    _types[id] = info;
}

bool GcDumpState::AddLiveObject(uint64_t address, uint64_t typeId, uint64_t size)
{
    if (!_isStarted)
    {
        return false;
    }

    auto entry = _types.find(typeId);
    if (entry == _types.end())
    {
        // this should never happen
        return false;
    }

    entry->second.AddInstance(address, size);
    return true;
}
