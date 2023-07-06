﻿// <auto-generated/>
#nullable enable

using System.Threading;

namespace Datadog.Trace.Telemetry;
internal partial class MetricsTelemetryCollector
{
    // These can technically overflow, but it's _very_ unlikely as we reset every minute
    // Negative values are normalized during polling
    public void RecordCountLogCreated(Datadog.Trace.Telemetry.Metrics.MetricTags.LogLevel tag, int increment = 1)
    {
        var index = 0 + (int)tag;
        Interlocked.Add(ref _buffer.Counts[index].Value, increment);
    }

    public void RecordCountIntegrationsError(Datadog.Trace.Telemetry.Metrics.MetricTags.IntegrationName tag1, Datadog.Trace.Telemetry.Metrics.MetricTags.InstrumentationError tag2, int increment = 1)
    {
        var index = 4 + ((int)tag1 * 3) + (int)tag2;
        Interlocked.Add(ref _buffer.Counts[index].Value, increment);
    }

    public void RecordCountSpanCreated(Datadog.Trace.Telemetry.Metrics.MetricTags.IntegrationName tag, int increment = 1)
    {
        var index = 154 + (int)tag;
        Interlocked.Add(ref _buffer.Counts[index].Value, increment);
    }

    public void RecordCountSpanFinished(int increment = 1)
    {
        Interlocked.Add(ref _buffer.Counts[204].Value, increment);
    }

    public void RecordCountSpanEnqueuedForSerialization(Datadog.Trace.Telemetry.Metrics.MetricTags.SpanEnqueueReason tag, int increment = 1)
    {
        var index = 205 + (int)tag;
        Interlocked.Add(ref _buffer.Counts[index].Value, increment);
    }

    public void RecordCountSpanDropped(Datadog.Trace.Telemetry.Metrics.MetricTags.DropReason tag, int increment = 1)
    {
        var index = 208 + (int)tag;
        Interlocked.Add(ref _buffer.Counts[index].Value, increment);
    }

    public void RecordCountTraceSegmentCreated(Datadog.Trace.Telemetry.Metrics.MetricTags.TraceContinuation tag, int increment = 1)
    {
        var index = 212 + (int)tag;
        Interlocked.Add(ref _buffer.Counts[index].Value, increment);
    }

    public void RecordCountTraceChunkEnqueued(Datadog.Trace.Telemetry.Metrics.MetricTags.TraceChunkEnqueueReason tag, int increment = 1)
    {
        var index = 214 + (int)tag;
        Interlocked.Add(ref _buffer.Counts[index].Value, increment);
    }

    public void RecordCountTraceChunkDropped(Datadog.Trace.Telemetry.Metrics.MetricTags.DropReason tag, int increment = 1)
    {
        var index = 216 + (int)tag;
        Interlocked.Add(ref _buffer.Counts[index].Value, increment);
    }

    public void RecordCountTraceChunkSent(int increment = 1)
    {
        Interlocked.Add(ref _buffer.Counts[220].Value, increment);
    }

    public void RecordCountTraceSegmentsClosed(int increment = 1)
    {
        Interlocked.Add(ref _buffer.Counts[221].Value, increment);
    }

    public void RecordCountTraceApiRequests(int increment = 1)
    {
        Interlocked.Add(ref _buffer.Counts[222].Value, increment);
    }

    public void RecordCountTraceApiResponses(Datadog.Trace.Telemetry.Metrics.MetricTags.StatusCode tag, int increment = 1)
    {
        var index = 223 + (int)tag;
        Interlocked.Add(ref _buffer.Counts[index].Value, increment);
    }

    public void RecordCountTraceApiErrors(Datadog.Trace.Telemetry.Metrics.MetricTags.ApiError tag, int increment = 1)
    {
        var index = 245 + (int)tag;
        Interlocked.Add(ref _buffer.Counts[index].Value, increment);
    }

    public void RecordCountTracePartialFlush(Datadog.Trace.Telemetry.Metrics.MetricTags.PartialFlushReason tag, int increment = 1)
    {
        var index = 248 + (int)tag;
        Interlocked.Add(ref _buffer.Counts[index].Value, increment);
    }

    public void RecordCountContextHeaderStyleInjected(Datadog.Trace.Telemetry.Metrics.MetricTags.ContextHeaderStyle tag, int increment = 1)
    {
        var index = 250 + (int)tag;
        Interlocked.Add(ref _buffer.Counts[index].Value, increment);
    }

    public void RecordCountContextHeaderStyleExtracted(Datadog.Trace.Telemetry.Metrics.MetricTags.ContextHeaderStyle tag, int increment = 1)
    {
        var index = 254 + (int)tag;
        Interlocked.Add(ref _buffer.Counts[index].Value, increment);
    }

    public void RecordCountStatsApiRequests(int increment = 1)
    {
        Interlocked.Add(ref _buffer.Counts[258].Value, increment);
    }

    public void RecordCountStatsApiResponses(Datadog.Trace.Telemetry.Metrics.MetricTags.StatusCode tag, int increment = 1)
    {
        var index = 259 + (int)tag;
        Interlocked.Add(ref _buffer.Counts[index].Value, increment);
    }

    public void RecordCountStatsApiErrors(Datadog.Trace.Telemetry.Metrics.MetricTags.ApiError tag, int increment = 1)
    {
        var index = 281 + (int)tag;
        Interlocked.Add(ref _buffer.Counts[index].Value, increment);
    }

    public void RecordCountTelemetryApiRequests(Datadog.Trace.Telemetry.Metrics.MetricTags.TelemetryEndpoint tag, int increment = 1)
    {
        var index = 284 + (int)tag;
        Interlocked.Add(ref _buffer.Counts[index].Value, increment);
    }

    public void RecordCountTelemetryApiResponses(Datadog.Trace.Telemetry.Metrics.MetricTags.TelemetryEndpoint tag1, Datadog.Trace.Telemetry.Metrics.MetricTags.StatusCode tag2, int increment = 1)
    {
        var index = 286 + ((int)tag1 * 22) + (int)tag2;
        Interlocked.Add(ref _buffer.Counts[index].Value, increment);
    }

    public void RecordCountTelemetryApiErrors(Datadog.Trace.Telemetry.Metrics.MetricTags.TelemetryEndpoint tag1, Datadog.Trace.Telemetry.Metrics.MetricTags.ApiError tag2, int increment = 1)
    {
        var index = 330 + ((int)tag1 * 3) + (int)tag2;
        Interlocked.Add(ref _buffer.Counts[index].Value, increment);
    }

    public void RecordCountVersionConflictTracerCreated(int increment = 1)
    {
        Interlocked.Add(ref _buffer.Counts[336].Value, increment);
    }

    public void RecordCountDirectLogLogs(Datadog.Trace.Telemetry.Metrics.MetricTags.IntegrationName tag, int increment = 1)
    {
        var index = 337 + (int)tag;
        Interlocked.Add(ref _buffer.Counts[index].Value, increment);
    }

    public void RecordCountDirectLogApiRequests(int increment = 1)
    {
        Interlocked.Add(ref _buffer.Counts[387].Value, increment);
    }

    public void RecordCountDirectLogApiResponses(Datadog.Trace.Telemetry.Metrics.MetricTags.StatusCode tag, int increment = 1)
    {
        var index = 388 + (int)tag;
        Interlocked.Add(ref _buffer.Counts[index].Value, increment);
    }

    public void RecordCountDirectLogApiErrors(Datadog.Trace.Telemetry.Metrics.MetricTags.ApiError tag, int increment = 1)
    {
        var index = 410 + (int)tag;
        Interlocked.Add(ref _buffer.Counts[index].Value, increment);
    }

    public void RecordCountWafInit(int increment = 1)
    {
        Interlocked.Add(ref _buffer.Counts[413].Value, increment);
    }

    public void RecordCountWafUpdates(int increment = 1)
    {
        Interlocked.Add(ref _buffer.Counts[414].Value, increment);
    }

    public void RecordCountWafRequests(Datadog.Trace.Telemetry.Metrics.MetricTags.WafAnalysis tag, int increment = 1)
    {
        var index = 415 + (int)tag;
        Interlocked.Add(ref _buffer.Counts[index].Value, increment);
    }

    /// <summary>
    /// Creates the buffer for the <see cref="Datadog.Trace.Telemetry.Metrics.Count" /> values.
    /// </summary>
    private static MetricKey[] GetCountBuffer()
        => new MetricKey[]
        {
            // logs_created, index = 0
            new(new[] { "level:debug" }),
            new(new[] { "level:info" }),
            new(new[] { "level:warn" }),
            new(new[] { "level:error" }),
            // integration_errors, index = 4
            new(new[] { "integration_name:datadog", "error_type:duck_typing" }),
            new(new[] { "integration_name:datadog", "error_type:invoker" }),
            new(new[] { "integration_name:datadog", "error_type:execution" }),
            new(new[] { "integration_name:opentracing", "error_type:duck_typing" }),
            new(new[] { "integration_name:opentracing", "error_type:invoker" }),
            new(new[] { "integration_name:opentracing", "error_type:execution" }),
            new(new[] { "integration_name:httpmessagehandler", "error_type:duck_typing" }),
            new(new[] { "integration_name:httpmessagehandler", "error_type:invoker" }),
            new(new[] { "integration_name:httpmessagehandler", "error_type:execution" }),
            new(new[] { "integration_name:httpsocketshandler", "error_type:duck_typing" }),
            new(new[] { "integration_name:httpsocketshandler", "error_type:invoker" }),
            new(new[] { "integration_name:httpsocketshandler", "error_type:execution" }),
            new(new[] { "integration_name:winhttphandler", "error_type:duck_typing" }),
            new(new[] { "integration_name:winhttphandler", "error_type:invoker" }),
            new(new[] { "integration_name:winhttphandler", "error_type:execution" }),
            new(new[] { "integration_name:curlhandler", "error_type:duck_typing" }),
            new(new[] { "integration_name:curlhandler", "error_type:invoker" }),
            new(new[] { "integration_name:curlhandler", "error_type:execution" }),
            new(new[] { "integration_name:aspnetcore", "error_type:duck_typing" }),
            new(new[] { "integration_name:aspnetcore", "error_type:invoker" }),
            new(new[] { "integration_name:aspnetcore", "error_type:execution" }),
            new(new[] { "integration_name:adonet", "error_type:duck_typing" }),
            new(new[] { "integration_name:adonet", "error_type:invoker" }),
            new(new[] { "integration_name:adonet", "error_type:execution" }),
            new(new[] { "integration_name:aspnet", "error_type:duck_typing" }),
            new(new[] { "integration_name:aspnet", "error_type:invoker" }),
            new(new[] { "integration_name:aspnet", "error_type:execution" }),
            new(new[] { "integration_name:aspnetmvc", "error_type:duck_typing" }),
            new(new[] { "integration_name:aspnetmvc", "error_type:invoker" }),
            new(new[] { "integration_name:aspnetmvc", "error_type:execution" }),
            new(new[] { "integration_name:aspnetwebapi2", "error_type:duck_typing" }),
            new(new[] { "integration_name:aspnetwebapi2", "error_type:invoker" }),
            new(new[] { "integration_name:aspnetwebapi2", "error_type:execution" }),
            new(new[] { "integration_name:graphql", "error_type:duck_typing" }),
            new(new[] { "integration_name:graphql", "error_type:invoker" }),
            new(new[] { "integration_name:graphql", "error_type:execution" }),
            new(new[] { "integration_name:hotchocolate", "error_type:duck_typing" }),
            new(new[] { "integration_name:hotchocolate", "error_type:invoker" }),
            new(new[] { "integration_name:hotchocolate", "error_type:execution" }),
            new(new[] { "integration_name:mongodb", "error_type:duck_typing" }),
            new(new[] { "integration_name:mongodb", "error_type:invoker" }),
            new(new[] { "integration_name:mongodb", "error_type:execution" }),
            new(new[] { "integration_name:xunit", "error_type:duck_typing" }),
            new(new[] { "integration_name:xunit", "error_type:invoker" }),
            new(new[] { "integration_name:xunit", "error_type:execution" }),
            new(new[] { "integration_name:nunit", "error_type:duck_typing" }),
            new(new[] { "integration_name:nunit", "error_type:invoker" }),
            new(new[] { "integration_name:nunit", "error_type:execution" }),
            new(new[] { "integration_name:mstestv2", "error_type:duck_typing" }),
            new(new[] { "integration_name:mstestv2", "error_type:invoker" }),
            new(new[] { "integration_name:mstestv2", "error_type:execution" }),
            new(new[] { "integration_name:wcf", "error_type:duck_typing" }),
            new(new[] { "integration_name:wcf", "error_type:invoker" }),
            new(new[] { "integration_name:wcf", "error_type:execution" }),
            new(new[] { "integration_name:webrequest", "error_type:duck_typing" }),
            new(new[] { "integration_name:webrequest", "error_type:invoker" }),
            new(new[] { "integration_name:webrequest", "error_type:execution" }),
            new(new[] { "integration_name:elasticsearchnet", "error_type:duck_typing" }),
            new(new[] { "integration_name:elasticsearchnet", "error_type:invoker" }),
            new(new[] { "integration_name:elasticsearchnet", "error_type:execution" }),
            new(new[] { "integration_name:servicestackredis", "error_type:duck_typing" }),
            new(new[] { "integration_name:servicestackredis", "error_type:invoker" }),
            new(new[] { "integration_name:servicestackredis", "error_type:execution" }),
            new(new[] { "integration_name:stackexchangeredis", "error_type:duck_typing" }),
            new(new[] { "integration_name:stackexchangeredis", "error_type:invoker" }),
            new(new[] { "integration_name:stackexchangeredis", "error_type:execution" }),
            new(new[] { "integration_name:serviceremoting", "error_type:duck_typing" }),
            new(new[] { "integration_name:serviceremoting", "error_type:invoker" }),
            new(new[] { "integration_name:serviceremoting", "error_type:execution" }),
            new(new[] { "integration_name:rabbitmq", "error_type:duck_typing" }),
            new(new[] { "integration_name:rabbitmq", "error_type:invoker" }),
            new(new[] { "integration_name:rabbitmq", "error_type:execution" }),
            new(new[] { "integration_name:msmq", "error_type:duck_typing" }),
            new(new[] { "integration_name:msmq", "error_type:invoker" }),
            new(new[] { "integration_name:msmq", "error_type:execution" }),
            new(new[] { "integration_name:kafka", "error_type:duck_typing" }),
            new(new[] { "integration_name:kafka", "error_type:invoker" }),
            new(new[] { "integration_name:kafka", "error_type:execution" }),
            new(new[] { "integration_name:cosmosdb", "error_type:duck_typing" }),
            new(new[] { "integration_name:cosmosdb", "error_type:invoker" }),
            new(new[] { "integration_name:cosmosdb", "error_type:execution" }),
            new(new[] { "integration_name:awssdk", "error_type:duck_typing" }),
            new(new[] { "integration_name:awssdk", "error_type:invoker" }),
            new(new[] { "integration_name:awssdk", "error_type:execution" }),
            new(new[] { "integration_name:awssqs", "error_type:duck_typing" }),
            new(new[] { "integration_name:awssqs", "error_type:invoker" }),
            new(new[] { "integration_name:awssqs", "error_type:execution" }),
            new(new[] { "integration_name:awssns", "error_type:duck_typing" }),
            new(new[] { "integration_name:awssns", "error_type:invoker" }),
            new(new[] { "integration_name:awssns", "error_type:execution" }),
            new(new[] { "integration_name:ilogger", "error_type:duck_typing" }),
            new(new[] { "integration_name:ilogger", "error_type:invoker" }),
            new(new[] { "integration_name:ilogger", "error_type:execution" }),
            new(new[] { "integration_name:aerospike", "error_type:duck_typing" }),
            new(new[] { "integration_name:aerospike", "error_type:invoker" }),
            new(new[] { "integration_name:aerospike", "error_type:execution" }),
            new(new[] { "integration_name:azurefunctions", "error_type:duck_typing" }),
            new(new[] { "integration_name:azurefunctions", "error_type:invoker" }),
            new(new[] { "integration_name:azurefunctions", "error_type:execution" }),
            new(new[] { "integration_name:couchbase", "error_type:duck_typing" }),
            new(new[] { "integration_name:couchbase", "error_type:invoker" }),
            new(new[] { "integration_name:couchbase", "error_type:execution" }),
            new(new[] { "integration_name:mysql", "error_type:duck_typing" }),
            new(new[] { "integration_name:mysql", "error_type:invoker" }),
            new(new[] { "integration_name:mysql", "error_type:execution" }),
            new(new[] { "integration_name:npgsql", "error_type:duck_typing" }),
            new(new[] { "integration_name:npgsql", "error_type:invoker" }),
            new(new[] { "integration_name:npgsql", "error_type:execution" }),
            new(new[] { "integration_name:oracle", "error_type:duck_typing" }),
            new(new[] { "integration_name:oracle", "error_type:invoker" }),
            new(new[] { "integration_name:oracle", "error_type:execution" }),
            new(new[] { "integration_name:sqlclient", "error_type:duck_typing" }),
            new(new[] { "integration_name:sqlclient", "error_type:invoker" }),
            new(new[] { "integration_name:sqlclient", "error_type:execution" }),
            new(new[] { "integration_name:sqlite", "error_type:duck_typing" }),
            new(new[] { "integration_name:sqlite", "error_type:invoker" }),
            new(new[] { "integration_name:sqlite", "error_type:execution" }),
            new(new[] { "integration_name:serilog", "error_type:duck_typing" }),
            new(new[] { "integration_name:serilog", "error_type:invoker" }),
            new(new[] { "integration_name:serilog", "error_type:execution" }),
            new(new[] { "integration_name:log4net", "error_type:duck_typing" }),
            new(new[] { "integration_name:log4net", "error_type:invoker" }),
            new(new[] { "integration_name:log4net", "error_type:execution" }),
            new(new[] { "integration_name:nlog", "error_type:duck_typing" }),
            new(new[] { "integration_name:nlog", "error_type:invoker" }),
            new(new[] { "integration_name:nlog", "error_type:execution" }),
            new(new[] { "integration_name:traceannotations", "error_type:duck_typing" }),
            new(new[] { "integration_name:traceannotations", "error_type:invoker" }),
            new(new[] { "integration_name:traceannotations", "error_type:execution" }),
            new(new[] { "integration_name:grpc", "error_type:duck_typing" }),
            new(new[] { "integration_name:grpc", "error_type:invoker" }),
            new(new[] { "integration_name:grpc", "error_type:execution" }),
            new(new[] { "integration_name:process", "error_type:duck_typing" }),
            new(new[] { "integration_name:process", "error_type:invoker" }),
            new(new[] { "integration_name:process", "error_type:execution" }),
            new(new[] { "integration_name:hashalgorithm", "error_type:duck_typing" }),
            new(new[] { "integration_name:hashalgorithm", "error_type:invoker" }),
            new(new[] { "integration_name:hashalgorithm", "error_type:execution" }),
            new(new[] { "integration_name:symmetricalgorithm", "error_type:duck_typing" }),
            new(new[] { "integration_name:symmetricalgorithm", "error_type:invoker" }),
            new(new[] { "integration_name:symmetricalgorithm", "error_type:execution" }),
            new(new[] { "integration_name:opentelemetry", "error_type:duck_typing" }),
            new(new[] { "integration_name:opentelemetry", "error_type:invoker" }),
            new(new[] { "integration_name:opentelemetry", "error_type:execution" }),
            new(new[] { "integration_name:pathtraversal", "error_type:duck_typing" }),
            new(new[] { "integration_name:pathtraversal", "error_type:invoker" }),
            new(new[] { "integration_name:pathtraversal", "error_type:execution" }),
            new(new[] { "integration_name:aws_lambda", "error_type:duck_typing" }),
            new(new[] { "integration_name:aws_lambda", "error_type:invoker" }),
            new(new[] { "integration_name:aws_lambda", "error_type:execution" }),
            // spans_created, index = 154
            new(new[] { "integration_name:datadog" }),
            new(new[] { "integration_name:opentracing" }),
            new(new[] { "integration_name:httpmessagehandler" }),
            new(new[] { "integration_name:httpsocketshandler" }),
            new(new[] { "integration_name:winhttphandler" }),
            new(new[] { "integration_name:curlhandler" }),
            new(new[] { "integration_name:aspnetcore" }),
            new(new[] { "integration_name:adonet" }),
            new(new[] { "integration_name:aspnet" }),
            new(new[] { "integration_name:aspnetmvc" }),
            new(new[] { "integration_name:aspnetwebapi2" }),
            new(new[] { "integration_name:graphql" }),
            new(new[] { "integration_name:hotchocolate" }),
            new(new[] { "integration_name:mongodb" }),
            new(new[] { "integration_name:xunit" }),
            new(new[] { "integration_name:nunit" }),
            new(new[] { "integration_name:mstestv2" }),
            new(new[] { "integration_name:wcf" }),
            new(new[] { "integration_name:webrequest" }),
            new(new[] { "integration_name:elasticsearchnet" }),
            new(new[] { "integration_name:servicestackredis" }),
            new(new[] { "integration_name:stackexchangeredis" }),
            new(new[] { "integration_name:serviceremoting" }),
            new(new[] { "integration_name:rabbitmq" }),
            new(new[] { "integration_name:msmq" }),
            new(new[] { "integration_name:kafka" }),
            new(new[] { "integration_name:cosmosdb" }),
            new(new[] { "integration_name:awssdk" }),
            new(new[] { "integration_name:awssqs" }),
            new(new[] { "integration_name:awssns" }),
            new(new[] { "integration_name:ilogger" }),
            new(new[] { "integration_name:aerospike" }),
            new(new[] { "integration_name:azurefunctions" }),
            new(new[] { "integration_name:couchbase" }),
            new(new[] { "integration_name:mysql" }),
            new(new[] { "integration_name:npgsql" }),
            new(new[] { "integration_name:oracle" }),
            new(new[] { "integration_name:sqlclient" }),
            new(new[] { "integration_name:sqlite" }),
            new(new[] { "integration_name:serilog" }),
            new(new[] { "integration_name:log4net" }),
            new(new[] { "integration_name:nlog" }),
            new(new[] { "integration_name:traceannotations" }),
            new(new[] { "integration_name:grpc" }),
            new(new[] { "integration_name:process" }),
            new(new[] { "integration_name:hashalgorithm" }),
            new(new[] { "integration_name:symmetricalgorithm" }),
            new(new[] { "integration_name:opentelemetry" }),
            new(new[] { "integration_name:pathtraversal" }),
            new(new[] { "integration_name:aws_lambda" }),
            // spans_finished, index = 204
            new(null),
            // spans_enqueued_for_serialization, index = 205
            new(new[] { "reason:p0_keep" }),
            new(new[] { "reason:single_span_sampling" }),
            new(new[] { "reason:default" }),
            // spans_dropped, index = 208
            new(new[] { "reason:p0_drop" }),
            new(new[] { "reason:overfull_buffer" }),
            new(new[] { "reason:serialization_error" }),
            new(new[] { "reason:api_error" }),
            // trace_segments_created, index = 212
            new(new[] { "new_continued:new" }),
            new(new[] { "new_continued:continued" }),
            // trace_chunks_enqueued_for_serialization, index = 214
            new(new[] { "reason:p0_keep" }),
            new(new[] { "reason:default" }),
            // trace_chunks_dropped, index = 216
            new(new[] { "reason:p0_drop" }),
            new(new[] { "reason:overfull_buffer" }),
            new(new[] { "reason:serialization_error" }),
            new(new[] { "reason:api_error" }),
            // trace_chunks_sent, index = 220
            new(null),
            // trace_segments_closed, index = 221
            new(null),
            // trace_api.requests, index = 222
            new(null),
            // trace_api.responses, index = 223
            new(new[] { "status_code:200" }),
            new(new[] { "status_code:201" }),
            new(new[] { "status_code:202" }),
            new(new[] { "status_code:204" }),
            new(new[] { "status_code:2xx" }),
            new(new[] { "status_code:301" }),
            new(new[] { "status_code:302" }),
            new(new[] { "status_code:307" }),
            new(new[] { "status_code:308" }),
            new(new[] { "status_code:3xx" }),
            new(new[] { "status_code:400" }),
            new(new[] { "status_code:401" }),
            new(new[] { "status_code:403" }),
            new(new[] { "status_code:404" }),
            new(new[] { "status_code:405" }),
            new(new[] { "status_code:4xx" }),
            new(new[] { "status_code:500" }),
            new(new[] { "status_code:501" }),
            new(new[] { "status_code:502" }),
            new(new[] { "status_code:503" }),
            new(new[] { "status_code:504" }),
            new(new[] { "status_code:5xx" }),
            // trace_api.errors, index = 245
            new(new[] { "type:timeout" }),
            new(new[] { "type:network" }),
            new(new[] { "type:status_code" }),
            // trace_partial_flush.count, index = 248
            new(new[] { "reason:large_trace" }),
            new(new[] { "reason:single_span_ingestion" }),
            // context_header_style.injected, index = 250
            new(new[] { "header_style:tracecontext" }),
            new(new[] { "header_style:datadog" }),
            new(new[] { "header_style:b3multi" }),
            new(new[] { "header_style:b3single" }),
            // context_header_style.extracted, index = 254
            new(new[] { "header_style:tracecontext" }),
            new(new[] { "header_style:datadog" }),
            new(new[] { "header_style:b3multi" }),
            new(new[] { "header_style:b3single" }),
            // stats_api.requests, index = 258
            new(null),
            // stats_api.responses, index = 259
            new(new[] { "status_code:200" }),
            new(new[] { "status_code:201" }),
            new(new[] { "status_code:202" }),
            new(new[] { "status_code:204" }),
            new(new[] { "status_code:2xx" }),
            new(new[] { "status_code:301" }),
            new(new[] { "status_code:302" }),
            new(new[] { "status_code:307" }),
            new(new[] { "status_code:308" }),
            new(new[] { "status_code:3xx" }),
            new(new[] { "status_code:400" }),
            new(new[] { "status_code:401" }),
            new(new[] { "status_code:403" }),
            new(new[] { "status_code:404" }),
            new(new[] { "status_code:405" }),
            new(new[] { "status_code:4xx" }),
            new(new[] { "status_code:500" }),
            new(new[] { "status_code:501" }),
            new(new[] { "status_code:502" }),
            new(new[] { "status_code:503" }),
            new(new[] { "status_code:504" }),
            new(new[] { "status_code:5xx" }),
            // stats_api.errors, index = 281
            new(new[] { "type:timeout" }),
            new(new[] { "type:network" }),
            new(new[] { "type:status_code" }),
            // telemetry_api.requests, index = 284
            new(new[] { "endpoint:agent" }),
            new(new[] { "endpoint:agentless" }),
            // telemetry_api.responses, index = 286
            new(new[] { "endpoint:agent", "status_code:200" }),
            new(new[] { "endpoint:agent", "status_code:201" }),
            new(new[] { "endpoint:agent", "status_code:202" }),
            new(new[] { "endpoint:agent", "status_code:204" }),
            new(new[] { "endpoint:agent", "status_code:2xx" }),
            new(new[] { "endpoint:agent", "status_code:301" }),
            new(new[] { "endpoint:agent", "status_code:302" }),
            new(new[] { "endpoint:agent", "status_code:307" }),
            new(new[] { "endpoint:agent", "status_code:308" }),
            new(new[] { "endpoint:agent", "status_code:3xx" }),
            new(new[] { "endpoint:agent", "status_code:400" }),
            new(new[] { "endpoint:agent", "status_code:401" }),
            new(new[] { "endpoint:agent", "status_code:403" }),
            new(new[] { "endpoint:agent", "status_code:404" }),
            new(new[] { "endpoint:agent", "status_code:405" }),
            new(new[] { "endpoint:agent", "status_code:4xx" }),
            new(new[] { "endpoint:agent", "status_code:500" }),
            new(new[] { "endpoint:agent", "status_code:501" }),
            new(new[] { "endpoint:agent", "status_code:502" }),
            new(new[] { "endpoint:agent", "status_code:503" }),
            new(new[] { "endpoint:agent", "status_code:504" }),
            new(new[] { "endpoint:agent", "status_code:5xx" }),
            new(new[] { "endpoint:agentless", "status_code:200" }),
            new(new[] { "endpoint:agentless", "status_code:201" }),
            new(new[] { "endpoint:agentless", "status_code:202" }),
            new(new[] { "endpoint:agentless", "status_code:204" }),
            new(new[] { "endpoint:agentless", "status_code:2xx" }),
            new(new[] { "endpoint:agentless", "status_code:301" }),
            new(new[] { "endpoint:agentless", "status_code:302" }),
            new(new[] { "endpoint:agentless", "status_code:307" }),
            new(new[] { "endpoint:agentless", "status_code:308" }),
            new(new[] { "endpoint:agentless", "status_code:3xx" }),
            new(new[] { "endpoint:agentless", "status_code:400" }),
            new(new[] { "endpoint:agentless", "status_code:401" }),
            new(new[] { "endpoint:agentless", "status_code:403" }),
            new(new[] { "endpoint:agentless", "status_code:404" }),
            new(new[] { "endpoint:agentless", "status_code:405" }),
            new(new[] { "endpoint:agentless", "status_code:4xx" }),
            new(new[] { "endpoint:agentless", "status_code:500" }),
            new(new[] { "endpoint:agentless", "status_code:501" }),
            new(new[] { "endpoint:agentless", "status_code:502" }),
            new(new[] { "endpoint:agentless", "status_code:503" }),
            new(new[] { "endpoint:agentless", "status_code:504" }),
            new(new[] { "endpoint:agentless", "status_code:5xx" }),
            // telemetry_api.errors, index = 330
            new(new[] { "endpoint:agent", "type:timeout" }),
            new(new[] { "endpoint:agent", "type:network" }),
            new(new[] { "endpoint:agent", "type:status_code" }),
            new(new[] { "endpoint:agentless", "type:timeout" }),
            new(new[] { "endpoint:agentless", "type:network" }),
            new(new[] { "endpoint:agentless", "type:status_code" }),
            // version_conflict_tracers_created, index = 336
            new(null),
            // direct_log_logs, index = 337
            new(new[] { "integration_name:datadog" }),
            new(new[] { "integration_name:opentracing" }),
            new(new[] { "integration_name:httpmessagehandler" }),
            new(new[] { "integration_name:httpsocketshandler" }),
            new(new[] { "integration_name:winhttphandler" }),
            new(new[] { "integration_name:curlhandler" }),
            new(new[] { "integration_name:aspnetcore" }),
            new(new[] { "integration_name:adonet" }),
            new(new[] { "integration_name:aspnet" }),
            new(new[] { "integration_name:aspnetmvc" }),
            new(new[] { "integration_name:aspnetwebapi2" }),
            new(new[] { "integration_name:graphql" }),
            new(new[] { "integration_name:hotchocolate" }),
            new(new[] { "integration_name:mongodb" }),
            new(new[] { "integration_name:xunit" }),
            new(new[] { "integration_name:nunit" }),
            new(new[] { "integration_name:mstestv2" }),
            new(new[] { "integration_name:wcf" }),
            new(new[] { "integration_name:webrequest" }),
            new(new[] { "integration_name:elasticsearchnet" }),
            new(new[] { "integration_name:servicestackredis" }),
            new(new[] { "integration_name:stackexchangeredis" }),
            new(new[] { "integration_name:serviceremoting" }),
            new(new[] { "integration_name:rabbitmq" }),
            new(new[] { "integration_name:msmq" }),
            new(new[] { "integration_name:kafka" }),
            new(new[] { "integration_name:cosmosdb" }),
            new(new[] { "integration_name:awssdk" }),
            new(new[] { "integration_name:awssqs" }),
            new(new[] { "integration_name:awssns" }),
            new(new[] { "integration_name:ilogger" }),
            new(new[] { "integration_name:aerospike" }),
            new(new[] { "integration_name:azurefunctions" }),
            new(new[] { "integration_name:couchbase" }),
            new(new[] { "integration_name:mysql" }),
            new(new[] { "integration_name:npgsql" }),
            new(new[] { "integration_name:oracle" }),
            new(new[] { "integration_name:sqlclient" }),
            new(new[] { "integration_name:sqlite" }),
            new(new[] { "integration_name:serilog" }),
            new(new[] { "integration_name:log4net" }),
            new(new[] { "integration_name:nlog" }),
            new(new[] { "integration_name:traceannotations" }),
            new(new[] { "integration_name:grpc" }),
            new(new[] { "integration_name:process" }),
            new(new[] { "integration_name:hashalgorithm" }),
            new(new[] { "integration_name:symmetricalgorithm" }),
            new(new[] { "integration_name:opentelemetry" }),
            new(new[] { "integration_name:pathtraversal" }),
            new(new[] { "integration_name:aws_lambda" }),
            // direct_log_api.requests, index = 387
            new(null),
            // direct_log_api.responses, index = 388
            new(new[] { "status_code:200" }),
            new(new[] { "status_code:201" }),
            new(new[] { "status_code:202" }),
            new(new[] { "status_code:204" }),
            new(new[] { "status_code:2xx" }),
            new(new[] { "status_code:301" }),
            new(new[] { "status_code:302" }),
            new(new[] { "status_code:307" }),
            new(new[] { "status_code:308" }),
            new(new[] { "status_code:3xx" }),
            new(new[] { "status_code:400" }),
            new(new[] { "status_code:401" }),
            new(new[] { "status_code:403" }),
            new(new[] { "status_code:404" }),
            new(new[] { "status_code:405" }),
            new(new[] { "status_code:4xx" }),
            new(new[] { "status_code:500" }),
            new(new[] { "status_code:501" }),
            new(new[] { "status_code:502" }),
            new(new[] { "status_code:503" }),
            new(new[] { "status_code:504" }),
            new(new[] { "status_code:5xx" }),
            // direct_log_api.errors.responses, index = 410
            new(new[] { "type:timeout" }),
            new(new[] { "type:network" }),
            new(new[] { "type:status_code" }),
            // waf.init, index = 413
            new(null),
            // waf.updates, index = 414
            new(null),
            // waf.requests, index = 415
            new(new[] { "waf_version", "rule_triggered:false", "request_blocked:false", "waf_timeout:false", "request_excluded:false" }),
            new(new[] { "waf_version", "rule_triggered:true", "request_blocked:false", "waf_timeout:false", "request_excluded:false" }),
            new(new[] { "waf_version", "rule_triggered:true", "request_blocked:true", "waf_timeout:false", "request_excluded:false" }),
            new(new[] { "waf_version", "rule_triggered:false", "request_blocked:false", "waf_timeout:true", "request_excluded:false" }),
            new(new[] { "waf_version", "rule_triggered:false", "request_blocked:false", "waf_timeout:false", "request_excluded:true" }),
        };

    /// <summary>
    /// Gets an array of metric counts, indexed by integer value of the <see cref="Datadog.Trace.Telemetry.Metrics.Count" />.
    /// Each value represents the number of unique entries in the buffer returned by <see cref="GetCountBuffer()" />
    /// It is equal to the cardinality of the tag combinations (or 1 if there are no tags)
    /// </summary>
    private static int[] CountEntryCounts { get; }
        = new []{ 4, 150, 50, 1, 3, 4, 2, 2, 4, 1, 1, 1, 22, 3, 2, 4, 4, 1, 22, 3, 2, 44, 6, 1, 50, 1, 22, 3, 1, 1, 5, };
}