using System;

namespace Samples.Probes.TestRuns
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor)]
    public class MethodProbeTestDataAttribute : ProbeAttributeBase
    {
        public MethodProbeTestDataAttribute(string returnTypeName = null, string[] parametersTypeName = null, bool skip = false, int phase = 1, bool unlisted = false, int expectedNumberOfSnapshots = 1, bool useFullTypeName = true, bool captureSnapshot = true, int evaluateAt = 1, params string[] skipOnFramework)
            : base(skip, phase, unlisted, expectedNumberOfSnapshots, skipOnFramework, captureSnapshot, evaluateAt)
        {

            ReturnTypeName = returnTypeName;
            ParametersTypeName = parametersTypeName;
            UseFullTypeName = useFullTypeName;
        }

        public string ReturnTypeName { get; }
        public string[] ParametersTypeName { get; }
        public bool UseFullTypeName { get; }
    }
}
