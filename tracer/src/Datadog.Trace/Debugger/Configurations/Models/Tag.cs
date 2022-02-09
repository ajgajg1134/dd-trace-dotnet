// <copyright file="Tag.cs" company="Datadog">
// Unless explicitly stated otherwise all files in this repository are licensed under the Apache 2 License.
// This product includes software developed at Datadog (https://www.datadoghq.com/). Copyright 2017 Datadog, Inc.
// </copyright>

namespace Datadog.Trace.Debugger.Configurations.Models;

internal class Tag
{
    public string Key { get; set; }

    public string Value { get; set; }
}
