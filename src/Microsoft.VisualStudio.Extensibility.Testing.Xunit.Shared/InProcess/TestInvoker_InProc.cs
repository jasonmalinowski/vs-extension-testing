// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for more information.

namespace Xunit.InProcess
{
    using System;
    using Xunit.Harness;

    internal class TestInvoker_InProc : InProcComponent
    {
        private TestInvoker_InProc()
        {
            AppDomain.CurrentDomain.AssemblyResolve += VisualStudioInstanceFactory.AssemblyResolveHandler;
        }

        // NOTE: This is called by OutOfProcComponent.CreateInProcComponent using Activator.
        public static TestInvoker_InProc Create()
            => new TestInvoker_InProc();

        public InProcessIdeTestAssemblyRunner CreateTestAssemblyRunner()
        {
            return new InProcessIdeTestAssemblyRunner();
        }
    }
}
