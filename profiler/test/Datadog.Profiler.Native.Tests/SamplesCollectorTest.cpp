// Unless explicitly stated otherwise all files in this repository are licensed under the Apache 2 License.
// This product includes software developed at Datadog (https://www.datadoghq.com/). Copyright 2022 Datadog, Inc.

#include "SamplesCollector.h"

#include "gmock/gmock.h"
#include "gtest/gtest.h"

#include "Configuration.h"
#include "IExporter.h"
#include "ISamplesProvider.h"
#include "ProfilerMockedInterface.h"
#include "Sample.h"
#include "ThreadsCpuManagerHelper.h"

#include <chrono>
#include <list>
#include <tuple>

using ::testing::_;
using ::testing::AtLeast;
using ::testing::ByMove;
using ::testing::Invoke;
using ::testing::InvokeWithoutArgs;
using ::testing::Return;
using ::testing::Throw;

using namespace std::chrono_literals;

std::unique_ptr<IExporter> CreateTransparentExporter(std::list<std::shared_ptr<Sample>>& pendingSamples, std::list<std::shared_ptr<Sample>>& exportedSamples)
{
    auto [exporter, mockExporter] = CreateExporter();

    EXPECT_CALL(mockExporter, Add(_))
        .WillRepeatedly(Invoke([&pendingSamples](std::shared_ptr<Sample> const& sample) {
            pendingSamples.push_back(sample);
        }));

    EXPECT_CALL(mockExporter, Export())
        .WillRepeatedly(Invoke([&pendingSamples, &exportedSamples] {
            exportedSamples.splice(exportedSamples.end(), std::move(pendingSamples));
            return true;
        }));

    return std::move(exporter);
}

template <typename T>
class FakeSamplesProvider : public T
{
public:
    FakeSamplesProvider(std::string_view runtimeId, int nbSamples) :
        _calls{0},
        _runtimeId{runtimeId},
        _nbSamples{nbSamples}
    {
    }

    std::list<std::shared_ptr<Sample>> GetSamples() override
    {
        _calls++;
        return CreateSamples(_runtimeId, _nbSamples);
    }

    int GetNbCalls()
    {
        return _calls;
    }

    std::shared_ptr<Sample> CreateSample(std::string_view rid)
    {
        static std::string ModuleName = "My module";
        static std::string FunctionName = "My frame";

        auto s = std::make_shared<Sample>(rid);

        s->AddFrame({ModuleName, FunctionName, "", 0});

        return s;
    }

    std::list<std::shared_ptr<Sample>> CreateSamples(std::string_view runtimeId, int nbSamples)
    {
        std::list<std::shared_ptr<Sample>> samples;
        for (int i = 0; i < nbSamples; i++)
        {
            samples.push_back(CreateSample(runtimeId));
        }
        return samples;
    }

    const char* GetName() override
    {
        return "FakeSamplesProvider";
    }

private:
    std::string_view _runtimeId;
    int _nbSamples;
    int _calls;
};


TEST(SamplesCollectorTest, MustCollectSamplesFromTwoProviders)
{
    std::string runtimeId = "MyRid";
    FakeSamplesProvider<ISamplesProvider> samplesProvider(runtimeId, 1);

    std::string runtimeId2 = "MyRid2";
    FakeSamplesProvider<ISamplesProvider> samplesProvider2(runtimeId2, 2);

    auto threadsCpuManagerHelper = ThreadsCpuManagerHelper();

    auto [configuration, mockConfiguration] = CreateConfiguration();
    EXPECT_CALL(mockConfiguration, GetUploadInterval()).Times(1).WillOnce(Return(1000s));

    std::list<std::shared_ptr<Sample>> pendingSamples;
    std::list<std::shared_ptr<Sample>> exportedSamples;

    auto exporter = CreateTransparentExporter(pendingSamples, exportedSamples);
    auto metricsSender = MockMetricsSender();

    auto collector = SamplesCollector(configuration.get(), &threadsCpuManagerHelper, exporter.get(), &metricsSender);
    collector.Register(&samplesProvider);
    collector.Register(&samplesProvider2);

    collector.Start();
    // wait for more than process interval so that ProcessSamples() is called multiple times
    std::this_thread::sleep_for(90ms);

    collector.Stop();

    auto exportsCount = samplesProvider.GetNbCalls();
    ASSERT_GE(exportsCount, 2);
    auto exportsCount2 = samplesProvider2.GetNbCalls();
    ASSERT_GE(exportsCount2, 2);

    uint32_t samplesCount1 = 0;
    uint32_t samplesCount2 = 0;

    for (auto const& sample : exportedSamples)
    {
        if (sample->GetRuntimeId() == runtimeId)
        {
            samplesCount1++;
        }
        else if (sample->GetRuntimeId() == runtimeId2)
        {
            samplesCount2++;
        }
        else
        {
            // unexpected
            ASSERT_TRUE(false);
        }
    }

    ASSERT_EQ(samplesCount1, exportsCount);
    ASSERT_EQ(samplesCount2, exportsCount2 * 2);

    exportedSamples.clear();

    collector.Export();

    ASSERT_EQ(exportedSamples.size(), 0);
    ASSERT_EQ(pendingSamples.size(), 0);
}

TEST(SamplesCollectorTest, MustCollectSamplesFromProviderAndBatchedProvider)
{
    std::string runtimeId = "MyRid";
    FakeSamplesProvider<ISamplesProvider> samplesProvider(runtimeId, 1);

    std::string runtimeId2 = "MyRid2";
    FakeSamplesProvider<IBatchedSamplesProvider> batchedSamplesProvider(runtimeId2, 2);

    auto threadsCpuManagerHelper = ThreadsCpuManagerHelper();

    auto [configuration, mockConfiguration] = CreateConfiguration();
    EXPECT_CALL(mockConfiguration, GetUploadInterval()).Times(1).WillOnce(Return(1000s));

    std::list<std::shared_ptr<Sample>> pendingSamples;
    std::list<std::shared_ptr<Sample>> exportedSamples;

    auto exporter = CreateTransparentExporter(pendingSamples, exportedSamples);
    auto metricsSender = MockMetricsSender();

    auto collector = SamplesCollector(configuration.get(), &threadsCpuManagerHelper, exporter.get(), &metricsSender);
    collector.Register(&samplesProvider);
    collector.RegisterBatchedProvider(&batchedSamplesProvider);

    collector.Start();

    // wait for more than process interval so that ProcessSamples() is called at least once for the SamplesProvider
    std::this_thread::sleep_for(90ms);

    collector.Stop();

    auto exportsCount = samplesProvider.GetNbCalls();
    ASSERT_EQ(exportsCount, 2);  // 1 at work time + 1 at stop time
    auto exportsCount2 = batchedSamplesProvider.GetNbCalls();
    ASSERT_EQ(exportsCount2, 1); // 1 at export time

    uint32_t samplesCount1 = 0;
    uint32_t samplesCount2 = 0;

    for (auto& sample : exportedSamples)
    {
        if (sample->GetRuntimeId() == runtimeId)
        {
            samplesCount1++;
        }
        else if (sample->GetRuntimeId() == runtimeId2)
        {
            samplesCount2++;
        }
        else
        {
            // unexpected
            ASSERT_TRUE(false);
        }
    }

    ASSERT_EQ(samplesCount1, exportsCount);
    ASSERT_EQ(samplesCount2, exportsCount2 * 2);

    exportedSamples.clear();

    collector.Export();

    ASSERT_EQ(exportedSamples.size(), 2);  // the BatchedSamplesProvider is called once in export
    ASSERT_EQ(pendingSamples.size(), 0);
}

TEST(SamplesCollectorTest, MustStopCollectingSamples)
{
    const std::string runtimeId = "MyRid";
    FakeSamplesProvider<ISamplesProvider> samplesProvider(runtimeId, 1);

    auto threadsCpuManagerHelper = ThreadsCpuManagerHelper();

    auto [configuration, mockConfiguration] = CreateConfiguration();
    EXPECT_CALL(mockConfiguration, GetUploadInterval()).Times(1).WillOnce(Return(1000s));

    auto [exporter, mockExporter] = CreateExporter();
    auto metricsSender = MockMetricsSender();

    auto collector = SamplesCollector(configuration.get(), &threadsCpuManagerHelper, exporter.get(), &metricsSender);
    collector.Register(&samplesProvider);

    collector.Start();
    // wait for more than upload interval so that ProcessSamples() is called multiple times
    std::this_thread::sleep_for(90ms);

    collector.Stop();

    auto exportsCount = samplesProvider.GetNbCalls();

    ASSERT_GE(exportsCount, 2);

    std::this_thread::sleep_for(200ms);

    auto newExportsCount = samplesProvider.GetNbCalls();

    ASSERT_EQ(newExportsCount, exportsCount);
}

TEST(SamplesCollectorTest, MustNotFailWhenSendingProfileThrows)
{
    auto [configuration, mockConfiguration] = CreateConfiguration();
    EXPECT_CALL(mockConfiguration, GetUploadInterval()).Times(1).WillOnce(Return(1s));

    auto [exporter, mockExporter] = CreateExporter();
    EXPECT_CALL(mockExporter, Add(_)).Times(AtLeast(1));
    EXPECT_CALL(mockExporter, Export()).Times(AtLeast(1)).WillRepeatedly(Throw(std::exception()));

    const std::string runtimeId = "MyRid";
    FakeSamplesProvider<ISamplesProvider> samplesProvider(runtimeId, 1);

    auto metricsSender = MockMetricsSender();
    auto threadsCpuManagerHelper = ThreadsCpuManagerHelper();

    auto collector = SamplesCollector(&mockConfiguration, &threadsCpuManagerHelper, &mockExporter, &metricsSender);

    collector.Register(&samplesProvider);

    collector.Start();
    std::this_thread::sleep_for(1s);
    collector.Export();
    collector.Stop();

    ASSERT_TRUE(metricsSender.WasCounterCalled());
}

TEST(SamplesCollectorTest, MustExportAfterStop)
{
    auto [configuration, mockConfiguration] = CreateConfiguration();
    EXPECT_CALL(mockConfiguration, GetUploadInterval()).Times(1).WillOnce(Return(2s));

    auto [exporter, mockExporter] = CreateExporter();

    // the provider and exporter are supposed to be called once AFTER Stop()
    EXPECT_CALL(mockExporter, Add(_)).Times(2);
    EXPECT_CALL(mockExporter, Export()).Times(1).WillRepeatedly(Return(true));

    auto metricsSender = MockMetricsSender();
    auto threadsCpuManagerHelper = ThreadsCpuManagerHelper();

    const std::string runtimeId = "MyRid";
    FakeSamplesProvider<ISamplesProvider> samplesProvider(runtimeId, 1);

    auto collector = SamplesCollector(&mockConfiguration, &threadsCpuManagerHelper, &mockExporter, &metricsSender);

    collector.Register(&samplesProvider);

    collector.Start();

    std::this_thread::sleep_for(100ms);

    collector.Stop();

    // Stop() is supposed call Export one last time
    ASSERT_TRUE(metricsSender.WasCounterCalled());
}

TEST(SamplesCollectorTest, MustNotFailWhenAddingSampleThrows)
{
    const std::string runtimeId = "MyRid";
    FakeSamplesProvider<ISamplesProvider> samplesProvider(runtimeId, 1);

    auto [configuration, mockConfiguration] = CreateConfiguration();
    EXPECT_CALL(mockConfiguration, GetUploadInterval()).Times(1).WillOnce(Return(1s));

    auto [exporter, mockExporter] = CreateExporter();
    EXPECT_CALL(mockExporter, Add(_)).Times(AtLeast(3)).WillOnce(Return()).WillRepeatedly(Throw(std::exception()));
    EXPECT_CALL(mockExporter, Export()).Times(1); // Called once when stopping

    auto metricsSender = MockMetricsSender();
    auto threadsCpuManagerHelper = ThreadsCpuManagerHelper();

    auto collector = SamplesCollector(&mockConfiguration, &threadsCpuManagerHelper, &mockExporter, &metricsSender);

    collector.Register(&samplesProvider);

    collector.Start();
    std::this_thread::sleep_for(200ms);
    collector.Stop();

    ASSERT_GT(samplesProvider.GetNbCalls(), 1);
}

TEST(SamplesCollectorTest, MustdNotAddSampleInExporterIfEmptyCallstack)
{
    auto [configuration, mockConfiguration] = CreateConfiguration();
    EXPECT_CALL(mockConfiguration, GetUploadInterval()).Times(1).WillOnce(Return(10s));

    std::string runtimeId = "MyRid";

    auto [samplesProvider, mockSamplesProvider] = CreateSamplesProvider();

    EXPECT_CALL(mockSamplesProvider, GetSamples())
        .Times(AtLeast(1))
        .WillRepeatedly(InvokeWithoutArgs([runtimeId] {
            std::list<std::shared_ptr<Sample>> samples;

            // add sample with empty callstack
            samples.push_back(std::make_shared<Sample>(runtimeId.c_str()));
            return samples;
        }));

    EXPECT_CALL(mockSamplesProvider, GetName())
        .Times(AtLeast(1))
        .WillRepeatedly(InvokeWithoutArgs([runtimeId] {
            return "MockedProvider";
        }));

    auto [exporter, mockExporter] = CreateExporter();
    EXPECT_CALL(mockExporter, Add(_)).Times(0);

    auto metricsSender = MockMetricsSender();
    auto threadsCpuManagerHelper = ThreadsCpuManagerHelper();

    auto collector = SamplesCollector(&mockConfiguration, &threadsCpuManagerHelper, &mockExporter, &metricsSender);

    collector.Register(samplesProvider.get());

    collector.Start();
    std::this_thread::sleep_for(100ms);
    collector.Stop();
}
