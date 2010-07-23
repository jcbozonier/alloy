using System;
using Unite.Messaging.Services;

namespace Unite.Messaging.Messages
{
    public interface IUnifiedMessagingService
    {
        void SendMessage(string recipient, string message);
        void GetMessages();
        void SetCredentials(Credentials credentials);
        event EventHandler<CredentialEventArgs> CredentialsRequested;
        event EventHandler<CredentialEventArgs> AuthorizationFailed;
        void StartReceiving();
        event EventHandler<MessagesReceivedEventArgs> MessagesReceived;
    }
}