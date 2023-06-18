﻿// <copyright file="TracerSettingsSnapshotGeneratorTests.cs" company="Datadog">
// Unless explicitly stated otherwise all files in this repository are licensed under the Apache 2 License.
// This product includes software developed at Datadog (https://www.datadoghq.com/). Copyright 2017 Datadog, Inc.
// </copyright>

using Datadog.Trace.SourceGenerators.TracerSettingsSnapshot;
using Datadog.Trace.SourceGenerators.TracerSettingsSnapshot.Diagnostics;
using Xunit;

namespace Datadog.Trace.SourceGenerators.Tests;

public class TracerSettingsSnapshotGeneratorTests
{
    [Fact]
    public void CanGenerateExpectedSnapshot()
    {
        const string input =
            """
            #nullable enable

            using System;
            using System.Collections.Concurrent;
            using System.Collections.Generic;
            using System.Linq;
            using System.Text.RegularExpressions;
            using System.Threading;
            using Datadog.Trace.SourceGenerators

            namespace Datadog.Trace.Configuration;
            
            [GenerateSnapshot]
            public partial class TracerSettings
            {
                [ConfigKey(ConfigurationKeys.Environment)]
                internal string? EnvironmentInternal { get; private set; }

                [ConfigKey(ConfigurationKeys.ServiceName)]
                internal string? ServiceNameInternal { get; set; }

                [ConfigKey(nameof(GitRepositoryUrl))]
                internal string? GitRepositoryUrl { get; }

                [ConfigKey("GitCommitSha")]
                internal string? GitCommitSha { get; }

                [ConfigKey("DisabledIntegrationNamesInternal")]
                internal HashSet<string> DisabledIntegrationNamesInternal { get; private set; }

                [ConfigKey("GlobalTagsInternal")]
                internal IDictionary<string, string> GlobalTagsInternal { get; private set; }

                [ConfigKey("GlobalTagsInternal2")]
                internal Dictionary<string, string> GlobalTagsInternal2 { get; private set; }

                [ConfigKey("SomeCollection")]
                internal string[] SomeCollection { get; private set; }

                [IgnoreForSnapshot]
                internal Dictionary<string, string> ThisShouldBeIgnored { get; private set; }
            }
            
            internal static partial class ConfigurationKeys
            {
                public const string ConfigurationFileName = "DD_TRACE_CONFIG_FILE";
                public const string Environment = "DD_ENV";
                public const string ServiceName = "DD_SERVICE";
            }
            """;

        const string expected =
            """
            // <auto-generated />

            using Datadog.Trace.Configuration.Telemetry;

            #nullable enable
            #pragma warning disable CS0618 // Type is obsolete

            namespace Datadog.Trace.Configuration;

            internal partial class TracerSettingsSnapshot : SettingsSnapshotBase
            {
                internal TracerSettingsSnapshot(Datadog.Trace.Configuration.TracerSettings settings)
                {
                    EnvironmentInternal = settings.EnvironmentInternal;
                    ServiceNameInternal = settings.ServiceNameInternal;
                    DisabledIntegrationNamesInternal = GetHashSet(settings.DisabledIntegrationNamesInternal);
                    GlobalTagsInternal = GetDictionary(settings.GlobalTagsInternal);
                    GlobalTagsInternal2 = GetDictionary(settings.GlobalTagsInternal2);
                    SomeCollection = settings.SomeCollection;
                    AdditionalInitialization(settings);
                }

                private string? EnvironmentInternal { get; }
                private string? ServiceNameInternal { get; }
                private System.Collections.Generic.HashSet<string>? DisabledIntegrationNamesInternal { get; }
                private System.Collections.Generic.IDictionary<string, string>? GlobalTagsInternal { get; }
                private System.Collections.Generic.Dictionary<string, string>? GlobalTagsInternal2 { get; }
                private string[] SomeCollection { get; }

                internal void RecordChanges(Datadog.Trace.Configuration.TracerSettings settings, IConfigurationTelemetry telemetry)
                {
                    RecordIfChanged(telemetry, "DD_ENV", EnvironmentInternal, settings.EnvironmentInternal);
                    RecordIfChanged(telemetry, "DD_SERVICE", ServiceNameInternal, settings.ServiceNameInternal);
                    RecordIfChanged(telemetry, "DisabledIntegrationNamesInternal", DisabledIntegrationNamesInternal, GetHashSet(settings.DisabledIntegrationNamesInternal));
                    RecordIfChanged(telemetry, "GlobalTagsInternal", GlobalTagsInternal, GetDictionary(settings.GlobalTagsInternal));
                    RecordIfChanged(telemetry, "GlobalTagsInternal2", GlobalTagsInternal2, GetDictionary(settings.GlobalTagsInternal2));
                    RecordIfChanged(telemetry, "SomeCollection", SomeCollection, settings.SomeCollection);
                    RecordAdditionalChanges(settings, telemetry);
                }

                partial void AdditionalInitialization(Datadog.Trace.Configuration.TracerSettings settings);
                partial void RecordAdditionalChanges(Datadog.Trace.Configuration.TracerSettings settings, IConfigurationTelemetry telemetry);
            }
            """;

        var (diagnostics, output) = TestHelpers.GetGeneratedOutput<TracerSettingsSnapshotGenerator>(input);
        Assert.Equal(expected, output);
        Assert.Empty(diagnostics);
    }

    [Fact]
    public void HasErrorIfConfigKeyIsNotProvided()
    {
        const string input =
            """
            using System;
            using Datadog.Trace.SourceGenerators

            namespace Datadog.Trace.Configuration;
            
            [GenerateSnapshot]
            public partial class TracerSettings
            {
                internal string? EnvironmentInternal { get; private set; }
            }
            """;

        var (diagnostics, output) = TestHelpers.GetGeneratedOutput<TracerSettingsSnapshotGenerator>(input);
        Assert.Contains(diagnostics, diag => diag.Id == MissingConfigKeyAttributeDiagnostic.Id);
    }
}
