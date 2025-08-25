// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for more information.

namespace Xunit.Threading
{
    using System;
    using System.Runtime.ExceptionServices;
    using System.Threading.Tasks;
    using Xunit.v3;

    public class ErrorReportingIdeTestRunner : XunitTestRunner
    {
        private readonly Exception _exception;

        public ErrorReportingIdeTestRunner(Exception exception)
        {
            _exception = exception;
        }

        protected override ValueTask<TimeSpan> RunTest(XunitTestRunnerContext ctxt)
        {
            ExceptionDispatchInfo.Capture(_exception).Throw();

            // The line above would have thrown, but the compiler doesn't know that
            return default;
        }
    }
}
