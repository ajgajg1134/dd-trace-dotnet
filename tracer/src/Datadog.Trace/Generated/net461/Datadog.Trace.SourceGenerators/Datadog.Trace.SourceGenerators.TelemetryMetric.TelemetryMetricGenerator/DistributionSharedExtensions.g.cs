﻿// <auto-generated/>
#nullable enable

namespace Datadog.Trace.Telemetry.Metrics;
internal static partial class DistributionSharedExtensions
{
    /// <summary>
    /// The number of separate metrics in the <see cref="Datadog.Trace.Telemetry.Metrics.DistributionShared" /> metric.
    /// </summary>
    public const int Length = 1;

    /// <summary>
    /// Gets the metric name for the provided metric
    /// </summary>
    /// <param name="metric">The metric to get the name for</param>
    /// <returns>The datadog metric name</returns>
    public static string GetName(this Datadog.Trace.Telemetry.Metrics.DistributionShared metric)
        => metric switch
        {
            Datadog.Trace.Telemetry.Metrics.DistributionShared.InitTime => "init_time",
            _ => null!,
        };

    /// <summary>
    /// Gets whether the metric is a "common" metric, used by all tracers
    /// </summary>
    /// <param name="metric">The metric to check</param>
    /// <returns>True if the metric is a "common" metric, used by all languages</returns>
    public static bool IsCommon(this Datadog.Trace.Telemetry.Metrics.DistributionShared metric)
        => metric switch
        {
            _ => true,
        };

    /// <summary>
    /// Gets the custom namespace for the provided metric
    /// </summary>
    /// <param name="metric">The metric to get the name for</param>
    /// <returns>The datadog metric name</returns>
    public static string? GetNamespace(this Datadog.Trace.Telemetry.Metrics.DistributionShared metric)
        => metric switch
        {
            Datadog.Trace.Telemetry.Metrics.DistributionShared.InitTime => "general",
            _ => null,
        };
}