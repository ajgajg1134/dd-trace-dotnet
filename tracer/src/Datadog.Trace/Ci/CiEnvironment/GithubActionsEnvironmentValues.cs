﻿// <copyright file="GithubActionsEnvironmentValues.cs" company="Datadog">
// Unless explicitly stated otherwise all files in this repository are licensed under the Apache 2 License.
// This product includes software developed at Datadog (https://www.datadoghq.com/). Copyright 2017 Datadog, Inc.
// </copyright>
#nullable enable

using System.Collections.Generic;

namespace Datadog.Trace.Ci.CiEnvironment;

internal sealed class GithubActionsEnvironmentValues<TValueProvider>(TValueProvider valueProvider) : CIEnvironmentValues<TValueProvider>(valueProvider)
    where TValueProvider : struct, IValueProvider
{
    protected override void OnInitialize(GitInfo gitInfo)
    {
        Log.Information("CIEnvironmentValues: GitHub Actions detected");

        IsCI = true;
        Provider = "github";

        var serverUrl = ValueProvider.GetValue(Constants.GitHubServerUrl);
        if (string.IsNullOrWhiteSpace(serverUrl))
        {
            serverUrl = "https://github.com";
        }

        serverUrl = RemoveSensitiveInformationFromUrl(serverUrl);

        var rawRepository = $"{serverUrl}/{ValueProvider.GetValue(Constants.GitHubRepository)}";
        Repository = $"{rawRepository}.git";
        Commit = ValueProvider.GetValue(Constants.GitHubSha);

        var headRef = ValueProvider.GetValue(Constants.GitHubHeadRef);
        var ghRef = !string.IsNullOrEmpty(headRef) ? headRef : ValueProvider.GetValue(Constants.GitHubRef);
        if (ghRef?.Contains("tags") == true)
        {
            Tag = ghRef;
        }
        else
        {
            Branch = ghRef;
        }

        SourceRoot = ValueProvider.GetValue(Constants.GitHubWorkspace);
        WorkspacePath = ValueProvider.GetValue(Constants.GitHubWorkspace);
        PipelineId = ValueProvider.GetValue(Constants.GitHubRunId);
        PipelineNumber = ValueProvider.GetValue(Constants.GitHubRunNumber);
        PipelineName = ValueProvider.GetValue(Constants.GitHubWorkflow);
        var attempts = ValueProvider.GetValue(Constants.GitHubRunAttempt);
        if (string.IsNullOrWhiteSpace(attempts))
        {
            PipelineUrl = $"{rawRepository}/actions/runs/{PipelineId}";
        }
        else
        {
            PipelineUrl = $"{rawRepository}/actions/runs/{PipelineId}/attempts/{attempts}";
        }

        JobUrl = $"{serverUrl}/{ValueProvider.GetValue(Constants.GitHubRepository)}/commit/{Commit}/checks";
        JobName = ValueProvider.GetValue(Constants.GitHubJob);

        VariablesToBypass = new Dictionary<string, string?>();
        SetVariablesIfNotEmpty(
            VariablesToBypass,
            [
                Constants.GitHubServerUrl,
                Constants.GitHubRepository,
                Constants.GitHubRunId,
                Constants.GitHubRunAttempt
            ],
            kvp =>
            {
                if (kvp.Key == Constants.GitHubServerUrl)
                {
                    return RemoveSensitiveInformationFromUrl(kvp.Value);
                }

                return kvp.Value;
            });
    }
}
