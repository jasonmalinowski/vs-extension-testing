// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for more information.

namespace Xunit.Harness
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Xunit.Sdk;
    using Xunit.v3;

    /// <summary>
    /// The implementation of <see cref="ITestFrameworkExecutor"/> that is invoked by xunit in the test process.
    /// </summary>
    public class IdeTestFrameworkExecutor : XunitTestFrameworkExecutor
    {
        private readonly ITestFrameworkDiscoveryOptions _discoveryOptions;

        public IdeTestFrameworkExecutor(IXunitTestAssembly assembly, ITestFrameworkDiscoveryOptions discoveryOptions)
            : base(assembly)
        {
            _discoveryOptions = discoveryOptions;
        }

        public override async ValueTask RunTestCases(IReadOnlyCollection<IXunitTestCase> testCases, IMessageSink executionMessageSink, ITestFrameworkExecutionOptions executionOptions, CancellationToken cancellationToken)
        {
            var assemblyRunner = new IdeTestAssemblyRunner(executionMessageSink, _discoveryOptions, executionOptions);
            await assemblyRunner.Run(TestAssembly, testCases, executionMessageSink, executionOptions, cancellationToken);
        }
    }
}
