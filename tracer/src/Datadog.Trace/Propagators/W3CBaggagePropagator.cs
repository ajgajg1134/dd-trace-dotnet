// <copyright file="W3CBaggagePropagator.cs" company="Datadog">
// Unless explicitly stated otherwise all files in this repository are licensed under the Apache 2 License.
// This product includes software developed at Datadog (https://www.datadoghq.com/). Copyright 2017 Datadog, Inc.
// </copyright>

using Datadog.Trace.Telemetry;
using Datadog.Trace.Telemetry.Metrics;

#nullable enable

namespace Datadog.Trace.Propagators;

// https://www.w3.org/TR/baggage/
internal class W3CBaggagePropagator : IContextInjector, IContextExtractor
{
    public const string BaggageHeaderName = "baggage";

    public static readonly W3CBaggagePropagator Instance = new();

    private W3CBaggagePropagator()
    {
    }

    public void Inject<TCarrier, TCarrierSetter>(
        SpanContext context,
        TCarrier carrier,
        TCarrierSetter carrierSetter)
        where TCarrierSetter : struct, ICarrierSetter<TCarrier>
    {
        TelemetryFactory.Metrics.RecordCountContextHeaderStyleInjected(MetricTags.ContextHeaderStyle.TraceContext);


        var baggage = CreateBaggageHeader(context);
        carrierSetter.Set(carrier, BaggageHeaderName, baggage);
    }

    public bool TryExtract<TCarrier, TCarrierGetter>(
        TCarrier carrier,
        TCarrierGetter carrierGetter,
        out SpanContext? spanContext)
        where TCarrierGetter : struct, ICarrierGetter<TCarrier>
    {
        spanContext = null;
        var baggageHeaders = carrierGetter.Get(carrier, BaggageHeaderName);

        // TODO: Implement baggage extraction

        throw new System.NotImplementedException();
    }

    internal static string CreateBaggageHeader(SpanContext context)
    {
        // TODO: Implement baggage injection
        throw new System.NotImplementedException();
    }
}
