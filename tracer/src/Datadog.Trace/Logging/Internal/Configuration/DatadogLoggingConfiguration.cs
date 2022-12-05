﻿// <copyright file="DatadogLoggingConfiguration.cs" company="Datadog">
// Unless explicitly stated otherwise all files in this repository are licensed under the Apache 2 License.
// This product includes software developed at Datadog (https://www.datadoghq.com/). Copyright 2017 Datadog, Inc.
// </copyright>

#nullable enable
namespace Datadog.Trace.Logging.Internal.Configuration;

internal readonly struct DatadogLoggingConfiguration
{
    public readonly int RateLimit;
    public readonly FileLoggingConfiguration? File;
    public readonly TelemetryLoggingConfiguration? Telemetry;

    public DatadogLoggingConfiguration(int rateLimit, FileLoggingConfiguration? file, TelemetryLoggingConfiguration? telemetry)
    {
        RateLimit = rateLimit;
        File = file;
        Telemetry = telemetry;
    }
}
