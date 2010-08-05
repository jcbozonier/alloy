using System;
using Unite.Messaging.Entities;
using Unite.Messaging.Services;

namespace Unite.Messaging.Messages
{
    public interface IUnifiedMessagingService
    {
        void SendMessage(string recipient, string message);
        void RequestMessages();
        void SetCredentials(Credentials credentials);
        void StartReceiving();
        event EventHandler<MessagesReceivedEventArgs> MessagesReceived;
    }
}