﻿// <auto-generated/>
#nullable enable

namespace Datadog.Trace;
partial class SpanContext
{

        /// <summary>
        /// Gets the parent context.
        /// </summary>
    [Datadog.Trace.SourceGenerators.PublicApi]
    public Datadog.Trace.ISpanContext Parent
    {
        get
        {
            Datadog.Trace.Telemetry.TelemetryFactory.Metrics.Record(
                (Datadog.Trace.Telemetry.Metrics.PublicApiUsage)28);
            return ParentInternal;
        }
    }

        /// <summary>
        /// Gets the span id of the parent span.
        /// </summary>
    [Datadog.Trace.SourceGenerators.PublicApi]
    public ulong? ParentId
    {
        get
        {
            Datadog.Trace.Telemetry.TelemetryFactory.Metrics.Record(
                (Datadog.Trace.Telemetry.Metrics.PublicApiUsage)29);
            return ParentIdInternal;
        }
    }

        /// <summary>
        /// Gets or sets the service name to propagate to child spans.
        /// </summary>
    [Datadog.Trace.SourceGenerators.PublicApi]
    public string ServiceName
    {
        get
        {
            Datadog.Trace.Telemetry.TelemetryFactory.Metrics.Record(
                (Datadog.Trace.Telemetry.Metrics.PublicApiUsage)30);
            return ServiceNameInternal;
        }
        set
        {
            Datadog.Trace.Telemetry.TelemetryFactory.Metrics.Record(
                (Datadog.Trace.Telemetry.Metrics.PublicApiUsage)31);
            ServiceNameInternal = value;
        }
    }
}