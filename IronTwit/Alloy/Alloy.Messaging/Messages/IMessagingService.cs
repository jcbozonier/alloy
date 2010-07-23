using System;
using System.Collections.Generic;
using Unite.Messaging.Entities;
using Unite.Messaging.Services;

namespace Unite.Messaging.Messages
{
    public interface IMessagingService
    {
        List<IMessage> GetMessages();
        void SendMessage(IIdentity recipient, string message);
        event EventHandler<CredentialEventArgs> CredentialsRequested;
        event EventHandler<CredentialEventArgs> AuthorizationFailed;
        ServiceInformation GetInformation();
        void StartReceiving();
        event EventHandler<MessagesReceivedEventArgs> MessagesReceived;
        void IfCanAcceptSet(Credentials credentials);
    }
}