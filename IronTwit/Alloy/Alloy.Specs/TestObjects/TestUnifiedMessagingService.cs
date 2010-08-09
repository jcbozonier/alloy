using System;
using Unite.Messaging.Entities;
using Unite.Messaging.Messages;
using Unite.Messaging.Services;

namespace Unite.Specs.TestObjects
{
    public class TestUnifiedMessagingService : IUnifiedMessagingService
    {
        public bool StartReceiving_WasCalled;

        public void SendMessage(string recipient, string message)
        {
            
        }

        public void RequestMessages()
        {
            
        }

        public void CredentialsProvided(Credentials credentials)
        {
            
        }

        public event EventHandler<CredentialEventArgs> CredentialsRequested;
        public event EventHandler<CredentialEventArgs> AuthorizationFailed;
        public void StartReceiving()
        {
            StartReceiving_WasCalled = true;
        }

        public event EventHandler<MessagesReceivedEventArgs> MessagesReceived;
    }
}