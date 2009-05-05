using System;
using System.Collections.Generic;
using Unite.Messaging;
using Unite.Messaging.Entities;
using Unite.Messaging.Services;

namespace Unite.Specs.FakeSpecObjects
{
    public class FakeReceivingMessagePlugin : FakePlugin
    {
        public override event EventHandler<MessagesReceivedEventArgs> MessagesReceived;
        public string Test;

        public void Pretend_you_just_received_random_messages_from(params string[] addresses)
        {
            var messages = new List<IMessage>();
            foreach(var address in addresses)
            {
                messages.Add(new Message(){Address = new Identity(address, GetInformation()), Text="Testing"});
            }

            var eventArgs = new MessagesReceivedEventArgs(messages);

            MessagesReceived(this, eventArgs);
        }

        public override void StartReceiving()
        {
                       
        }
    }
}
