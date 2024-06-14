﻿// <copyright company="Datadog">
// Unless explicitly stated otherwise all files in this repository are licensed under the Apache 2 License.
// This product includes software developed at Datadog (https://www.datadoghq.com/). Copyright 2017 Datadog, Inc.
// </copyright>
// <auto-generated/>

#nullable enable


using System;
using System.Runtime.InteropServices;

namespace NativeObjects;

internal unsafe class ICrashReport : Datadog.Trace.Tools.dd_dotnet.ICrashReport
{
    public static ICrashReport Wrap(IntPtr obj) => new ICrashReport(obj);

    private readonly IntPtr _implementation;

    public ICrashReport(IntPtr implementation)
    {
        _implementation = implementation;
    }

    private nint* VTable => (nint*)*(nint*)_implementation;

    public void Dispose()
    {
        GC.SuppressFinalize(this);

        if (_implementation != IntPtr.Zero)
        {
            Release();
        }
    }

    ~ICrashReport()
    {
        Dispose();
    }

    public int QueryInterface(in System.Guid a0, out nint a1)
    {
        var func = (delegate* unmanaged[Stdcall]<IntPtr, in System.Guid, out nint, int>)*(VTable + 0);
        var result = func(_implementation, in a0, out a1);
        return result;
    }
    public int AddRef()
    {
        var func = (delegate* unmanaged[Stdcall]<IntPtr, int>)*(VTable + 1);
        var result = func(_implementation);
        return result;
    }
    public int Release()
    {
        var func = (delegate* unmanaged[Stdcall]<IntPtr, int>)*(VTable + 2);
        var result = func(_implementation);
        return result;
    }
    public int Initialize()
    {
        var func = (delegate* unmanaged[Stdcall]<IntPtr, out int, int>)*(VTable + 3);
        var result = func(_implementation, out var returnvalue);
        if (result != 0)
        {
            throw new System.ComponentModel.Win32Exception(result);
        }
        return returnvalue;
    }
    public void GetLastError(out nint a0, out int a1)
    {
        var func = (delegate* unmanaged[Stdcall]<IntPtr, out nint, out int, int>)*(VTable + 4);
        var result = func(_implementation, out a0, out a1);
        if (result != 0)
        {
            throw new System.ComponentModel.Win32Exception(result);
        }
    }
    public int AddTag(nint a0, nint a1)
    {
        var func = (delegate* unmanaged[Stdcall]<IntPtr, nint, nint, out int, int>)*(VTable + 5);
        var result = func(_implementation, a0, a1, out var returnvalue);
        if (result != 0)
        {
            throw new System.ComponentModel.Win32Exception(result);
        }
        return returnvalue;
    }
    public int SetSignalInfo(int a0, nint a1)
    {
        var func = (delegate* unmanaged[Stdcall]<IntPtr, int, nint, out int, int>)*(VTable + 6);
        var result = func(_implementation, a0, a1, out var returnvalue);
        if (result != 0)
        {
            throw new System.ComponentModel.Win32Exception(result);
        }
        return returnvalue;
    }
    public int ResolveStacks(int a0, nint a1, nint a2, out bool a3)
    {
        var func = (delegate* unmanaged[Stdcall]<IntPtr, int, nint, nint, out bool, out int, int>)*(VTable + 7);
        var result = func(_implementation, a0, a1, a2, out a3, out var returnvalue);
        if (result != 0)
        {
            throw new System.ComponentModel.Win32Exception(result);
        }
        return returnvalue;
    }
    public int SetMetadata(nint a0, nint a1, nint a2, Datadog.Trace.Tools.dd_dotnet.ICrashReport.Tag* a3, int a4)
    {
        var func = (delegate* unmanaged[Stdcall]<IntPtr, nint, nint, nint, Datadog.Trace.Tools.dd_dotnet.ICrashReport.Tag*, int, out int, int>)*(VTable + 8);
        var result = func(_implementation, a0, a1, a2, a3, a4, out var returnvalue);
        if (result != 0)
        {
            throw new System.ComponentModel.Win32Exception(result);
        }
        return returnvalue;
    }
    public int Send()
    {
        var func = (delegate* unmanaged[Stdcall]<IntPtr, out int, int>)*(VTable + 9);
        var result = func(_implementation, out var returnvalue);
        if (result != 0)
        {
            throw new System.ComponentModel.Win32Exception(result);
        }
        return returnvalue;
    }
    public int WriteToFile(nint a0)
    {
        var func = (delegate* unmanaged[Stdcall]<IntPtr, nint, out int, int>)*(VTable + 10);
        var result = func(_implementation, a0, out var returnvalue);
        if (result != 0)
        {
            throw new System.ComponentModel.Win32Exception(result);
        }
        return returnvalue;
    }
    public int CrashProcess()
    {
        var func = (delegate* unmanaged[Stdcall]<IntPtr, out int, int>)*(VTable + 11);
        var result = func(_implementation, out var returnvalue);
        if (result != 0)
        {
            throw new System.ComponentModel.Win32Exception(result);
        }
        return returnvalue;
    }


}
