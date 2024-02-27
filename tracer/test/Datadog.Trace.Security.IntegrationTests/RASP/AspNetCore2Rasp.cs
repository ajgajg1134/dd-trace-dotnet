// <copyright file="AspNetCore2Rasp.cs" company="Datadog">
// Unless explicitly stated otherwise all files in this repository are licensed under the Apache 2 License.
// This product includes software developed at Datadog (https://www.datadoghq.com/). Copyright 2017 Datadog, Inc.
// </copyright>

#if NETCOREAPP2_1
#pragma warning disable SA1402 // File may only contain a single class
#pragma warning disable SA1649 // File name must match first type name

using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Datadog.Trace.Configuration;
using Datadog.Trace.Iast.Telemetry;
using Datadog.Trace.Security.IntegrationTests.IAST;
using Datadog.Trace.TestHelpers;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Xunit;
using Xunit.Abstractions;

namespace Datadog.Trace.Security.IntegrationTests.Rasp;

public class AspNetCore2RaspEnabledIastEnabled : AspNetCore2Rasp
{
    public AspNetCore2RaspEnabledIastEnabled(AspNetCoreTestFixture fixture, ITestOutputHelper outputHelper)
    : base(fixture, outputHelper, enableIast: true)
    {
    }
}

public class AspNetCore2RaspEnabledIastDisabled : AspNetCore2Rasp
{
    public AspNetCore2RaspEnabledIastDisabled(AspNetCoreTestFixture fixture, ITestOutputHelper outputHelper)
    : base(fixture, outputHelper, enableIast: false)
    {
    }
}

public abstract class AspNetCore2Rasp : AspNetBase, IClassFixture<AspNetCoreTestFixture>
{
    // This class is used to test RASP features either with IAST enabled or disabled. Since they both use common instrumentation
    // points, we should test that IAST works normally with or without RASP enabled.
    public AspNetCore2Rasp(AspNetCoreTestFixture fixture, ITestOutputHelper outputHelper, bool enableIast)
        : base("AspNetCore2", outputHelper, "/shutdown", testName: "AspNetCore2.SecurityEnabled")
    {
        EnableRasp();
        SetSecurity(true);
        EnableIast(enableIast);
        SetEnvironmentVariable(ConfigurationKeys.Iast.IsIastDeduplicationEnabled, "false");
        SetEnvironmentVariable(ConfigurationKeys.Iast.VulnerabilitiesPerRequest, "100");
        SetEnvironmentVariable(ConfigurationKeys.Iast.RequestSampling, "100");
        SetEnvironmentVariable(ConfigurationKeys.Iast.RedactionEnabled, "true");
        var externalRulesFile = "C:\\CommonFolder\\shared\\repos\\dd-trace-5\\tracer\\test\\Datadog.Trace.Security.Unit.Tests\\rasp-rule-set.json";
        SetEnvironmentVariable(ConfigurationKeys.AppSec.Rules, externalRulesFile);
        EnableEvidenceRedaction(false);
        EnableIastTelemetry((int)IastMetricsVerbosityLevel.Off);
        IastEnabled = enableIast;
        Fixture = fixture;
        Fixture.SetOutput(outputHelper);
    }

    protected bool IastEnabled { get; }

    protected AspNetCoreTestFixture Fixture { get; }

    public override void Dispose()
    {
        base.Dispose();
        Fixture.SetOutput(null);
    }

    public async Task TryStartApp()
    {
        await Fixture.TryStartApp(this, true);
        SetHttpPort(Fixture.HttpPort);
    }

    [SkippableTheory]
    [InlineData("/etc/password")]
    [InlineData("filename")]
    [Trait("RunOnWindows", "True")]
    public async Task TestRaspIastPathTraversalRequest(string file)
    {
        var methodName = "PathTraversal";
        var testName = IastEnabled ? "RaspIast.AspNetCore2" : "Rasp.AspNetCore2";
        var url = $"/Iast/GetFileContent?file={file}";
        IncludeAllHttpSpans = true;
        await TryStartApp();
        var agent = Fixture.Agent;
        var spans = await SendRequestsAsync(agent, new string[] { url });
        var spansFiltered = spans.Where(x => x.Type == SpanTypes.Web).ToList();
        var settings = VerifyHelper.GetSpanVerifierSettings();
        settings.UseParameters(file);
        settings.AddIastScrubbing();
        await VerifySpans(spansFiltered.ToImmutableList(), settings, testName: testName, methodNameOverride: methodName);
    }
}
#endif
