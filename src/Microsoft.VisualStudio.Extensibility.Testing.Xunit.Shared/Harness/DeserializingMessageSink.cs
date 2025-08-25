// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for more information.

namespace Xunit.Harness
{
    using Xunit.Runner.Common;
    using Xunit.Sdk;
    using Xunit.Threading;

    public sealed class DeserializingMessageSink : LongLivedMarshalByRefObject
    {
        private IMessageSink _executionMessageSink;

        public DeserializingMessageSink(IMessageSink executionMessageSink)
        {
            _executionMessageSink = executionMessageSink;
        }

        public bool OnMessage(string json)
        {
            return _executionMessageSink.OnMessage(MessageSinkMessageDeserializer.Deserialize(json, diagnosticMessageSink: null)!);
        }
    }
}
