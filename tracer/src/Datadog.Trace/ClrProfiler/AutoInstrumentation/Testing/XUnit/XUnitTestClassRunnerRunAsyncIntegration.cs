// <copyright file="XUnitTestClassRunnerRunAsyncIntegration.cs" company="Datadog">
// Unless explicitly stated otherwise all files in this repository are licensed under the Apache 2 License.
// This product includes software developed at Datadog (https://www.datadoghq.com/). Copyright 2017 Datadog, Inc.
// </copyright>
#nullable enable

using System;
using System.ComponentModel;
using Datadog.Trace.Ci;
using Datadog.Trace.ClrProfiler.CallTarget;
using Datadog.Trace.DuckTyping;

namespace Datadog.Trace.ClrProfiler.AutoInstrumentation.Testing.XUnit;

/// <summary>
/// Xunit.Sdk.TestClassRunner`1.RunAsync calltarget instrumentation
/// </summary>
[InstrumentMethod(
    AssemblyNames = ["xunit.execution.dotnet", "xunit.execution.desktop"],
    TypeName = "Xunit.Sdk.TestClassRunner`1",
    MethodName = "RunAsync",
    ReturnTypeName = "System.Threading.Tasks.Task`1[Xunit.Sdk.RunSummary]",
    MinimumVersion = "2.2.0",
    MaximumVersion = "2.*.*",
    IntegrationName = XUnitIntegration.IntegrationName)]
[Browsable(false)]
[EditorBrowsable(EditorBrowsableState.Never)]
public static class XUnitTestClassRunnerRunAsyncIntegration
{
    /// <summary>
    /// OnMethodBegin callback
    /// </summary>
    /// <typeparam name="TTarget">Type of the target</typeparam>
    /// <param name="instance">Instance value, aka `this` of the instrumented method.</param>
    /// <returns>Calltarget state value</returns>
    internal static CallTargetState OnMethodBegin<TTarget>(TTarget instance)
    {
        if (!XUnitIntegration.IsEnabled || instance is null)
        {
            return CallTargetState.GetDefault();
        }

        if (TestModule.Current is { } testModule &&
            instance.TryDuckCast<ITestClassRunner>(out var classRunnerInstance))
        {
            if (XUnitIntegration.CheckIfSuiteIsSkippable(classRunnerInstance) || true)
            {
                var messageBus = classRunnerInstance.MessageBus;
                var duckMessageBus = messageBus.DuckCast<IMessageBus>();
                var messageBusInterfaceType = messageBus.GetType().GetInterface("IMessageBus")!;
                var suiteSkipMessageBus = new SuiteSkipMessageBus(duckMessageBus);
                classRunnerInstance.MessageBus = suiteSkipMessageBus.DuckImplement(messageBusInterfaceType);
            }

            return new CallTargetState(null, testModule.InternalGetOrCreateSuite(classRunnerInstance.TestClass.Class.Name ?? string.Empty));
        }

        Common.Log.Warning("Test module cannot be found.");
        return CallTargetState.GetDefault();
    }

    /// <summary>
    /// OnMethodEnd callback
    /// </summary>
    /// <typeparam name="TTarget">Type of the target</typeparam>
    /// <typeparam name="TResult">TestResult type</typeparam>
    /// <param name="instance">Instance value, aka `this` of the instrumented method.</param>
    /// <param name="returnValue">Original method return value</param>
    /// <param name="exception">Exception instance in case the original code threw an exception.</param>
    /// <param name="state">Calltarget state value</param>
    /// <returns>Return value of the method</returns>
    internal static CallTargetReturn<TResult> OnMethodEnd<TTarget, TResult>(TTarget instance, TResult returnValue, Exception exception, in CallTargetState state)
    {
        if (state.State == TestSuite.Current)
        {
            // Restore the AsyncLocal set
            // This is used to mimic the ExecutionContext copy from the StateMachine
            // CallTarget integrations does this automatically when using a normal `Scope`
            // in this case we have to do it manually.
            TestSuite.Current = null;
        }

        return new CallTargetReturn<TResult>(returnValue);
    }

    /// <summary>
    /// OnAsyncMethodEnd callback
    /// </summary>
    /// <typeparam name="TTarget">Type of the target</typeparam>
    /// <typeparam name="TReturn">Type of the return type</typeparam>
    /// <param name="instance">Instance value, aka `this` of the instrumented method.</param>
    /// <param name="returnValue">Return value</param>
    /// <param name="exception">Exception instance in case the original code threw an exception.</param>
    /// <param name="state">Calltarget state value</param>
    /// <returns>A response value, in an async scenario will be T of Task of T</returns>
    internal static TReturn OnAsyncMethodEnd<TTarget, TReturn>(TTarget instance, TReturn returnValue, Exception exception, in CallTargetState state)
    {
        if (state.State is TestSuite testSuite)
        {
            testSuite.Close();
        }

        return returnValue;
    }

    internal class SuiteSkipMessageBus : IMessageBus
    {
        private readonly IMessageBus _innerMessageBus;

        public SuiteSkipMessageBus(IMessageBus innerMessageBus)
        {
            _innerMessageBus = innerMessageBus;
        }

        [DuckReverseMethod]
        public void Dispose()
        {
            _innerMessageBus.Dispose();
        }

        [DuckReverseMethod]
        public bool QueueMessage(object? message)
        {
            _innerMessageBus.QueueMessage(message);

            if (message is null)
            {
                return false;
            }

            var messageType = message.GetType();
            if (messageType.Name == "TestClassStarting" &&
                message.TryDuckCast<TestClassStartingStruct>(out var testClassStarting))
            {
                if (messageType.Assembly.GetType("Xunit.Sdk.TestClassFinished") is { } testClassFinishedType)
                {
                    var classFinishedMessage = Activator.CreateInstance(
                        testClassFinishedType,
                        testClassStarting.TestCases,
                        testClassStarting.TestClass,
                        0M,
                        0,
                        0,
                        10);
                    _innerMessageBus.QueueMessage(classFinishedMessage);
                }
            }

            return false;
        }

        [DuckCopy]
        internal struct TestClassStartingStruct
        {
            public object? TestCases;
            public object? TestClass;
        }
    }
}
