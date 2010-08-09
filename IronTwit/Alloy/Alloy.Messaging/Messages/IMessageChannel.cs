using System.Collections.Generic;
using Unite.Messaging.Entities;

namespace Unite.Messaging.Messages
{
    public interface IMessageChannel
    {
        void ReceivedMessages(IEnumerable<IMessage> messages);
    }
}
