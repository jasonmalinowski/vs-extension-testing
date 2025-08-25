// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for more information.

namespace Xunit.Harness
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;
    using Xunit.Sdk;
    using Xunit.Threading;
    using Xunit.v3;

    public class InProcessIdeTestAssemblyRunner : LongLivedMarshalByRefObject
    {
        public Tuple<int, int, int, decimal> RunTestCollection(string testAssembly, HashSet<string> testCaseUniqueIds, DeserializingMessageSink executionMessageSink, ITestFrameworkDiscoveryOptions discoveryOptions, ITestFrameworkExecutionOptions executionOptions)
        {
            // Remoting doesn't support async methods, so we'll have to do a blocking operation here
#pragma warning disable VSTHRD002
            return RunTestCollectionAsync(testAssembly, testCaseUniqueIds, executionMessageSink, discoveryOptions, executionOptions).GetAwaiter().GetResult();
#pragma warning restore VSTHRD002
        }

        private async Task<Tuple<int, int, int, decimal>> RunTestCollectionAsync(string testAssembly, HashSet<string> testCaseUniqueIds, DeserializingMessageSink executionMessageSink, ITestFrameworkDiscoveryOptions discoveryOptions, ITestFrameworkExecutionOptions executionOptions)
        {
            using (var cancellationTokenSource = new CancellationTokenSource())
            {
                var assembly = Assembly.LoadFrom(testAssembly);
                var xunitAssembly = new XunitTestAssembly(assembly);

                // We'll need to rediscover the same tests in-process
                TestContext.SetForInitialization(null, false, false);

                var discoverer = new XunitTestFrameworkDiscoverer(xunitAssembly, null);
                var testCases = new List<IXunitTestCase>();

                await discoverer.Find(
                    testCase =>
                    {
                        if (testCaseUniqueIds.Contains(testCase.UniqueID))
                        {
                            testCases.Add((IXunitTestCase)testCase);
                        }

                        return new ValueTask<bool>(true);
                    },
                    discoveryOptions);

                var result = await XunitTestAssemblyRunner.Instance.Run(xunitAssembly, testCases, new SerializingMessageSink(executionMessageSink), executionOptions, cancellationTokenSource.Token);
                return Tuple.Create(result.Total, result.Failed, result.Skipped, result.Time);
            }
        }

        private class SerializingMessageSink(DeserializingMessageSink executionMessageSink) : IMessageSink
        {
            public bool OnMessage(IMessageSinkMessage message)
            {
                return executionMessageSink.OnMessage(message.ToJson()!);
            }
        }
    }
}
