using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unite.Messaging.Entities;
using Unite.Messaging.Messages;
using Unite.Messaging.Services;

namespace Unite.Messaging
{
    public interface IMessagingServiceManager
    {
        void SendMessage(string recipient, string message);
        bool CanAccept(Credentials credentials);
        List<IMessage> GetMessages();
        void SetCredentials(Credentials credentials);
        event EventHandler<CredentialEventArgs> CredentialsRequested;
        event EventHandler<CredentialEventArgs> AuthorizationFailed;
        bool CanFind(string address);
        ServiceInformation GetInformation();
        void StartReceiving();
        void StopReceiving();
        event EventHandler<MessagesReceivedEventArgs> MessagesReceived; 
        event EventHandler<ContactEventArgs> OnContactsReceived;
    }

    public interface IContactService
    {
        event EventHandler<ContactEventArgs> OnContactsReceived;
    }

    public class ContactEventArgs : EventArgs
    {
        public IEnumerable<IIdentity> ReceivedContacts;

        public ContactEventArgs()
        {
        }

        public ContactEventArgs(IEnumerable<IIdentity> contacts)
        {
            ReceivedContacts = contacts;
        }
    }
}
