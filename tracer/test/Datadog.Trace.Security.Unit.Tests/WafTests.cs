// <copyright file="WafTests.cs" company="Datadog">
// Unless explicitly stated otherwise all files in this repository are licensed under the Apache 2 License.
// This product includes software developed at Datadog (https://www.datadoghq.com/). Copyright 2017 Datadog, Inc.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Datadog.Trace.AppSec;
using Datadog.Trace.AppSec.Waf;
using Datadog.Trace.AppSec.Waf.NativeBindings;
using Datadog.Trace.AppSec.Waf.ReturnTypes.Managed;
using Datadog.Trace.Security.Unit.Tests.Utils;
using Datadog.Trace.Vendors.Newtonsoft.Json;
using FluentAssertions;
using Xunit;

namespace Datadog.Trace.Security.Unit.Tests
{
    public class WafTests : WafLibraryRequiredTest
    {
        public const int TimeoutMicroSeconds = 1_000_000;

        public WafTests(WafLibraryInvokerFixture wafLibraryInvokerFixture)
            : base(wafLibraryInvokerFixture)
        {
        }

        [Theory]
        [InlineData("[$ne]", "arg", "nosql_injection", "crs-942-290")]
        [InlineData("attack", "appscan_fingerprint", "security_scanner", "crs-913-120")]
        [InlineData("key", "<script>", "xss", "crs-941-110")]
        [InlineData("value", "sleep(10)", "sql_injection", "crs-942-160")]
        public void QueryStringAttack(string key, string attack, string flow, string rule)
        {
            Execute(
                AddressesConstants.RequestQuery,
                new Dictionary<string, string[]> { { key, new string[] { attack } } },
                flow,
                rule);
        }

        [Theory]
        [InlineData("something", "appscan_fingerprint", "security_scanner", "crs-913-120")]
        [InlineData("something", "/.htaccess", "lfi", "crs-930-120")]
        public void PathParamsAttack(string key, string attack, string flow, string rule)
        {
            Execute(
                AddressesConstants.RequestPathParams,
                new Dictionary<string, string[]> { { key, new string[] { attack } } },
                flow,
                rule);
        }

        [Fact]
        public void UrlRawAttack()
        {
            Execute(
                AddressesConstants.RequestUriRaw,
                "http://localhost:54587/waf/0x5c0x2e0x2e0x2f",
                "lfi",
                "crs-930-100");
        }

        [Theory]
        [InlineData("user-agent", "Arachni/v1", "security_scanner", "ua0-600-12x")]
        [InlineData("referer", "<script >", "xss", "crs-941-110")]
        [InlineData("x-file-name", "routing.yml", "command_injection", "crs-932-180")]
        [InlineData("x-filename", "routing.yml", "command_injection", "crs-932-180")]
        [InlineData("x_filename", "routing.yml", "command_injection", "crs-932-180")]
        public void HeadersAttack(string header, string content, string flow, string rule)
        {
            Execute(
                AddressesConstants.RequestHeaderNoCookies,
                new Dictionary<string, string> { { header, content } },
                flow,
                rule);
        }

        [Theory(Skip = "Cookies rules has been removed in rules version 1.2.7. Test on cookies are now done on custom rules scenario. Once we have rules with cookie back in the default rules set, we can re-use this class to validated this feature")]
        [InlineData("attack", ".htaccess", "lfi", "crs-930-120")]
        [InlineData("value", "/*!*/", "sql_injection", "crs-942-100")]
        [InlineData("value", ";shutdown--", "sql_injection", "crs-942-280")]
        [InlineData("key", ".cookie-;domain=", "http_protocol_violation", "crs-943-100")]
        [InlineData("x-attack", " var_dump ()", "php_code_injection", "crs-933-160")]
        [InlineData("x-attack", "o:4:\"x\":5:{d}", "php_code_injection", "crs-933-170")]
        [InlineData("key", "<script>", "xss", "crs-941-110")]
        public void CookiesAttack(string key, string content, string flow, string rule)
        {
            Execute(
                AddressesConstants.RequestCookies,
                new Dictionary<string, List<string>> { { key, new List<string> { content } } },
                flow,
                rule);
        }

        [Theory]
        [InlineData("/.adsensepostnottherenonobook", "security_scanner", "crs-913-120")]
        public void BodyAttack(string body, string flow, string rule) => Execute(AddressesConstants.RequestBody, body, flow, rule);

        [Theory]
        [InlineData("x_filename", "routing.yml")]
        public void Test(string header, string content)
        {
            var args = new Dictionary<string, string> { { header, content } };
            var argsroot = new Dictionary<string, object> { { AddressesConstants.RequestHeaderNoCookies, args } };
            var initResult = Waf.Create(WafLibraryInvoker, string.Empty, string.Empty, @"C:\Repositories\dd-trace-dotnet\tracer\src\Datadog.Trace\AppSec\Waf\rule-set.json");
            using var waf = initResult.Waf;
            waf.Should().NotBeNull();
            using var context = waf.CreateContext() as Context;
            var result = Encoder.EncodeInternal2(argsroot, WafLibraryInvoker);
            var pnt = Marshal.AllocHGlobal(Marshal.SizeOf(result));
            Marshal.StructureToPtr(result, pnt, false);
            var resultwaf = context.Run2(pnt, TimeoutMicroSeconds);
        }

        private void Execute(string address, object value, string flow, string rule)
        {
            var args = new Dictionary<string, object> { { address, value } };
            if (!args.ContainsKey(AddressesConstants.RequestUriRaw))
            {
                args.Add(AddressesConstants.RequestUriRaw, "http://localhost:54587/");
            }

            if (!args.ContainsKey(AddressesConstants.RequestMethod))
            {
                args.Add(AddressesConstants.RequestMethod, "GET");
            }

            var initResult = Waf.Create(WafLibraryInvoker, string.Empty, string.Empty);
            using var waf = initResult.Waf;
            waf.Should().NotBeNull();
            using var context = waf.CreateContext();
            var result = context.Run(args, TimeoutMicroSeconds);
            result.ReturnCode.Should().Be(ReturnCode.Match);
            var resultData = JsonConvert.DeserializeObject<WafMatch[]>(result.Data).FirstOrDefault();
            resultData.Rule.Tags.Type.Should().Be(flow);
            resultData.Rule.Id.Should().Be(rule);
            resultData.RuleMatches[0].Parameters[0].Address.Should().Be(address);
        }
    }
}
