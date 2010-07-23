using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unite.Messaging.Entities;

namespace Unite.Messaging.Messages
{
    public interface IMessageChannel
    {
        void ReceivedMessages(IEnumerable<IMessage> messages);
    }
}
