﻿// <auto-generated/>
#nullable enable

using Datadog.Trace.Processors;
using Datadog.Trace.Tagging;
using System;

namespace Datadog.Trace.Ci.Tagging
{
    partial class TestSuiteSpanTags
    {
        // SuiteBytes = System.Text.Encoding.UTF8.GetBytes("test.suite");
#if NETCOREAPP
        private static ReadOnlySpan<byte> SuiteBytes => new byte[] { 116, 101, 115, 116, 46, 115, 117, 105, 116, 101 };
#else
        private static readonly byte[] SuiteBytes = new byte[] { 116, 101, 115, 116, 46, 115, 117, 105, 116, 101 };
#endif

        public override string? GetTag(string key)
        {
            return key switch
            {
                "test.suite" => Suite,
                _ => base.GetTag(key),
            };
        }

        public override void SetTag(string key, string value)
        {
            switch(key)
            {
                case "test.suite": 
                    Suite = value;
                    break;
                default: 
                    base.SetTag(key, value);
                    break;
            }
        }

        public override void EnumerateTags<TProcessor>(ref TProcessor processor)
        {
            if (Suite is not null)
            {
                processor.Process(new TagItem<string>("test.suite", Suite, SuiteBytes));
            }

            base.EnumerateTags(ref processor);
        }

        protected override void WriteAdditionalTags(System.Text.StringBuilder sb)
        {
            if (Suite is not null)
            {
                sb.Append("test.suite (tag):")
                  .Append(Suite)
                  .Append(',');
            }

            base.WriteAdditionalTags(sb);
        }
    }
}
