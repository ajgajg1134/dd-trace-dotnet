﻿// <copyright file="ITestModule.cs" company="Datadog">
// Unless explicitly stated otherwise all files in this repository are licensed under the Apache 2 License.
// This product includes software developed at Datadog (https://www.datadoghq.com/). Copyright 2017 Datadog, Inc.
// </copyright>

#nullable enable

using Datadog.Trace.DuckTyping;
using Datadog.Trace.SourceGenerators;

namespace Datadog.Trace.Ci;

/// <summary>
/// CI Visibility test module
/// </summary>
[DuckType("Datadog.Trace.Ci.TestModule", "Datadog.Trace")]
[DuckAsClass]
public interface ITestModule
{
    /// <summary>
    /// Gets the module name
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Gets the test module start date
    /// </summary>
    DateTimeOffset StartTime { get; }

    /// <summary>
    /// Gets the test framework
    /// </summary>
    string? Framework { get; }

    /// <summary>
    /// Sets a string tag into the test
    /// </summary>
    /// <param name="key">Key of the tag</param>
    /// <param name="value">Value of the tag</param>
    void SetTag(string key, string? value);

    /// <summary>
    /// Sets a number tag into the test
    /// </summary>
    /// <param name="key">Key of the tag</param>
    /// <param name="value">Value of the tag</param>
    void SetTag(string key, double? value);

    /// <summary>
    /// Set Error Info
    /// </summary>
    /// <param name="type">Error type</param>
    /// <param name="message">Error message</param>
    /// <param name="callStack">Error callstack</param>
    void SetErrorInfo(string type, string message, string? callStack);

    /// <summary>
    /// Set Error Info from Exception
    /// </summary>
    /// <param name="exception">Exception instance</param>
    void SetErrorInfo(Exception exception);

    /// <summary>
    /// Close test module
    /// </summary>
    /// <remarks>Use CloseAsync() version whenever possible.</remarks>
    void Close();

    /// <summary>
    /// Close test module
    /// </summary>
    /// <remarks>Use CloseAsync() version whenever possible.</remarks>
    /// <param name="duration">Duration of the test module</param>
    void Close(TimeSpan? duration);

    /// <summary>
    /// Close test module
    /// </summary>
    /// <returns>Task instance </returns>
    Task CloseAsync();

    /// <summary>
    /// Close test module
    /// </summary>
    /// <param name="duration">Duration of the test module</param>
    /// <returns>Task instance </returns>
    Task CloseAsync(TimeSpan? duration);

    /// <summary>
    /// Create a new test suite for this session
    /// </summary>
    /// <param name="name">Name of the test suite</param>
    /// <returns>Test suite instance</returns>
    ITestSuite GetOrCreateSuite(string name);

    /// <summary>
    /// Create a new test suite for this session
    /// </summary>
    /// <param name="name">Name of the test suite</param>
    /// <param name="startDate">Test suite start date</param>
    /// <returns>Test suite instance</returns>
    ITestSuite GetOrCreateSuite(string name, DateTimeOffset? startDate);
}
