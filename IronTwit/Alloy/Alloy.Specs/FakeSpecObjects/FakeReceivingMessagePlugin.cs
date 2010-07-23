using System;
using Unite.Messaging.Messages;
using Unite.Messaging.Services;

namespace Unite.Specs.FakeSpecObjects
{
    public class FakeReceivingMessagePlugin : FakePlugin
    {
        public override event EventHandler<MessagesReceivedEventArgs> MessagesReceived;

        public override void StartReceiving()
        {
                       
        }
    }
}
