﻿// <copyright file="TracerFlareRequestFactoryTests.cs" company="Datadog">
// Unless explicitly stated otherwise all files in this repository are licensed under the Apache 2 License.
// This product includes software developed at Datadog (https://www.datadoghq.com/). Copyright 2017 Datadog, Inc.
// </copyright>

using System.IO;
using System.Text;
using System.Threading.Tasks;
using Datadog.Trace.Logging.TracerFlare;
using FluentAssertions;
using Xunit;

namespace Datadog.Trace.Tests.Logging.TracerFlare;

public class TracerFlareRequestFactoryTests
{
    [Fact]
    public async Task GetRequestBody_GeneratesExpectedRequest()
    {
        var flareBytes = new byte[50];
        for (var i = 0; i < flareBytes.Length; i++)
        {
            flareBytes[i] = 43;
        }

        using var flare = new MemoryStream(flareBytes);

        using var requestStream = new MemoryStream();

        var caseId = "12345";

        await TracerFlareRequestFactory.WriteRequestBody(requestStream, stream => flare.CopyToAsync(stream), caseId);

        var deserializedBytes = Encoding.UTF8.GetString(requestStream.ToArray());

        deserializedBytes.Should().Be("""
                                      --83CAD6AA-8A24-462C-8B3D-FF9CC683B51B
                                      Content-Disposition: form-data; name="source"

                                      tracer_dotnet
                                      --83CAD6AA-8A24-462C-8B3D-FF9CC683B51B
                                      Content-Disposition: form-data; name="case_id"

                                      12345
                                      --83CAD6AA-8A24-462C-8B3D-FF9CC683B51B
                                      Content-Disposition: form-data; name="flare_file"; filename="debug_logs.zip"
                                      Content-Type: application/octet-stream

                                      ++++++++++++++++++++++++++++++++++++++++++++++++++
                                      --83CAD6AA-8A24-462C-8B3D-FF9CC683B51B--
                                      """);
    }
}
