using System;
using System.Collections.Generic;
using Unite.Messaging.Entities;

namespace Unite.Messaging.Services
{
    public interface IUnifiedMessagingController
    {
        void MessageToSend(string recipient, string message);
        void RequestMessageUpdate();
        IEnumerable<IMessage> GetAllMessages();
        event EventHandler NewMessagesReceived;
    }
}