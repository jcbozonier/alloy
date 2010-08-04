using System.Collections.Generic;
using Unite.Messaging.Entities;
using Unite.Messaging.Messages;

namespace Unite.Specs.TestObjects
{
    public class TestMessageChannel : IMessageChannel
    {
        public void ReceivedMessages(IEnumerable<IMessage> messages)
        {
            
        }
    }
}