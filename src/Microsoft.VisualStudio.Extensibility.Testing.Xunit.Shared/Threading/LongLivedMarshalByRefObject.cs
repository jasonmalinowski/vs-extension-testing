// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for more information.

namespace Xunit.Threading
{
    using System;

    public class LongLivedMarshalByRefObject : MarshalByRefObject
    {
        public override object InitializeLifetimeService()
        {
            return base.InitializeLifetimeService();
        }
    }
}
