// <copyright file="ConfigurationTelemetryCollector.cs" company="Datadog">
// Unless explicitly stated otherwise all files in this repository are licensed under the Apache 2 License.
// This product includes software developed at Datadog (https://www.datadoghq.com/). Copyright 2017 Datadog, Inc.
// </copyright>

using System;
using System.Collections.Generic;
using System.Threading;
using Datadog.Trace.AppSec;
using Datadog.Trace.ClrProfiler;
using Datadog.Trace.Configuration;
using Datadog.Trace.ContinuousProfiler;
using Datadog.Trace.Iast.Settings;
using Datadog.Trace.PlatformHelpers;

namespace Datadog.Trace.Telemetry
{
    internal class ConfigurationTelemetryCollector
    {
        private int _tracerInstanceCount = 0;
        private int _hasChangesFlag = 0;
        private volatile CurrentSettings _settings;
        private volatile SecuritySettings _securitySettings;
        private volatile IastSettings _iastSettings;
        private volatile Profiler _profiler;
        private volatile bool _isTracerInitialized = false;
        private HostTelemetryData _hostData = null;

        public void RecordTracerSettings(
            ImmutableTracerSettings tracerSettings,
            string defaultServiceName)
        {
            // Increment number of times this has been called
            var reconfigureCount = Interlocked.Increment(ref _tracerInstanceCount);
            var appData = new ApplicationTelemetryData(
                serviceName: defaultServiceName,
                env: tracerSettings.EnvironmentInternal,
                tracerVersion: TracerConstants.AssemblyVersion,
                languageName: TracerConstants.Language,
                languageVersion: FrameworkDescription.Instance.ProductVersion)
            {
                ServiceVersion = tracerSettings.ServiceVersionInternal,
                RuntimeName = FrameworkDescription.Instance.Name,
            };

            _settings = new CurrentSettings(tracerSettings, appData);

            // The remaining properties can't change, so only need to set them the first time
            if (reconfigureCount != 1)
            {
                SetHasChanges();
                return;
            }

            var host = HostMetadata.Instance;
            _hostData = new HostTelemetryData
            {
                ContainerId = ContainerMetadata.GetContainerId(),
                Os = FrameworkDescription.Instance.OSPlatform,
                OsVersion = Environment.OSVersion.ToString(),
                Hostname = host.Hostname,
                KernelName = host.KernelName,
                KernelRelease = host.KernelRelease,
                KernelVersion = host.KernelVersion,
            };

            _isTracerInitialized = true;
            SetHasChanges();
        }

        public void RecordSecuritySettings(SecuritySettings securitySettings)
        {
            _securitySettings = securitySettings;
            SetHasChanges();
        }

        public void RecordIastSettings(IastSettings iastSettings)
        {
            _iastSettings = iastSettings;
            SetHasChanges();
        }

        public void RecordProfilerSettings(Profiler profiler)
        {
            _profiler = profiler;
            SetHasChanges();
        }

        public bool HasChanges()
        {
            return _isTracerInitialized && _hasChangesFlag == 1;
        }

        /// <summary>
        /// Get the application data. Will be null if not yet initialized.
        /// </summary>
        public ApplicationTelemetryData GetApplicationData()
        {
            var settings = _settings;
            return settings?.ApplicationData;
        }

        /// <summary>
        /// Get the application data. Will be null if not yet initialized.
        /// </summary>
        public HostTelemetryData GetHostData()
        {
            return _hostData;
        }

        /// <summary>
        /// Get the latest data to send to the intake.
        /// </summary>
        /// <returns>Null if there are no changes, or the collector is not yet initialized</returns>
        public ICollection<TelemetryValue> GetConfigurationData()
        {
            var hasChanges = Interlocked.CompareExchange(ref _hasChangesFlag, 0, 1) == 1;
            if (!_isTracerInitialized || !hasChanges)
            {
                return null;
            }

            var settings = _settings.Settings;

            var data = new List<TelemetryValue>(29 + (settings.IsRunningInAzureAppService ? 5 : 0))
            {
                new(ConfigTelemetryData.Platform, value: FrameworkDescription.Instance.ProcessArchitecture),
                new(ConfigTelemetryData.Enabled, value: settings.TraceEnabledInternal),
                new(ConfigTelemetryData.AgentUrl, value: settings.ExporterInternal.AgentUriInternal.ToString()),
                new(ConfigTelemetryData.AgentTraceTransport, value: settings.ExporterInternal.TracesTransport.ToString()),
                new(ConfigTelemetryData.Debug, value: GlobalSettings.Instance.DebugEnabledInternal),
                new(ConfigTelemetryData.NativeTracerVersion, value: Instrumentation.GetNativeTracerVersion()),
#pragma warning disable CS0618
                new(ConfigTelemetryData.AnalyticsEnabled, value: settings.AnalyticsEnabledInternal),
#pragma warning restore CS0618
                new(ConfigTelemetryData.SampleRate, value: settings.GlobalSamplingRateInternal),
                new(ConfigTelemetryData.SamplingRules, value: settings.CustomSamplingRulesInternal),
                new(ConfigTelemetryData.LogInjectionEnabled, value: settings.LogsInjectionEnabledInternal),
                new(ConfigTelemetryData.RuntimeMetricsEnabled, value: settings.RuntimeMetricsEnabled),
                new(ConfigTelemetryData.RoutetemplateResourcenamesEnabled, value: settings.RouteTemplateResourceNamesEnabled),
                new(ConfigTelemetryData.RoutetemplateExpansionEnabled, value: settings.ExpandRouteTemplatesEnabled),
                new(ConfigTelemetryData.PartialflushEnabled, value: settings.ExporterInternal.PartialFlushEnabledInternal),
                new(ConfigTelemetryData.PartialflushMinspans, value: settings.ExporterInternal.PartialFlushMinSpansInternal),
                new(ConfigTelemetryData.TracerInstanceCount, value: _tracerInstanceCount),
                new(ConfigTelemetryData.SecurityEnabled, value: _securitySettings?.Enabled),
                new(ConfigTelemetryData.IastEnabled, value: _iastSettings?.Enabled),
                new(ConfigTelemetryData.FullTrustAppDomain, value: AppDomain.CurrentDomain.IsFullyTrusted),
                new(ConfigTelemetryData.TraceMethods, value: settings.TraceMethods),
                new(ConfigTelemetryData.OpenTelemetryEnabled, value: settings.IsActivityListenerEnabled),
                new(ConfigTelemetryData.ProfilerLoaded, value: _profiler?.Status.IsProfilerReady),
                new(ConfigTelemetryData.CodeHotspotsEnabled, value: _profiler?.ContextTracker.IsEnabled),
                new(ConfigTelemetryData.StatsComputationEnabled, value: settings.StatsComputationEnabledInternal),
                new(ConfigTelemetryData.WcfObfuscationEnabled, value: settings.WcfObfuscationEnabled),
                new(ConfigTelemetryData.DataStreamsMonitoringEnabled, value: settings.IsDataStreamsMonitoringEnabled),
                new(ConfigTelemetryData.SpanSamplingRules, value: settings.SpanSamplingRules),
                new(ConfigTelemetryData.PropagationStyleExtract, value: string.Join(",", settings.PropagationStyleExtract)),
                new(ConfigTelemetryData.PropagationStyleInject, value: string.Join(",", settings.PropagationStyleInject)),
            };

            if (settings.IsRunningInAzureAppService)
            {
                data.Add(new(ConfigTelemetryData.AasConfigurationError, value: settings.AzureAppServiceMetadata.IsUnsafeToTrace));
                data.Add(new(name: ConfigTelemetryData.CloudHosting, "Azure"));
                data.Add(new(name: ConfigTelemetryData.AasSiteExtensionVersion, settings.AzureAppServiceMetadata.SiteExtensionVersion));
                data.Add(new(name: ConfigTelemetryData.AasAppType, settings.AzureAppServiceMetadata.SiteType));
                data.Add(new(name: ConfigTelemetryData.AasFunctionsRuntimeVersion, settings.AzureAppServiceMetadata.FunctionsExtensionVersion));
            }

            // data.Configuration["agent_reachable"] = agentError == null;
            // data.Configuration["agent_error"] = agentError ?? string.Empty;

            // Global tags?
            // Agent reachable
            // Agent error
            // Is CallTarget
            // Is Docker
            // Is Fargate etc

            // additional values
            // Native metrics

            return data;
        }

        private void SetHasChanges()
        {
            Interlocked.Exchange(ref _hasChangesFlag, 1);
        }

        private class CurrentSettings
        {
            public CurrentSettings(ImmutableTracerSettings settings, ApplicationTelemetryData applicationData)
            {
                Settings = settings;
                ApplicationData = applicationData;
            }

            public ImmutableTracerSettings Settings { get; }

            public ApplicationTelemetryData ApplicationData { get; }
        }
    }
}
