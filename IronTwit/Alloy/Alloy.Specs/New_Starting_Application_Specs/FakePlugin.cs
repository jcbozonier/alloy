using System;
using System.Collections.Generic;
using System.Linq;
using Unite.Messaging;
using Unite.Messaging.Entities;
using Unite.Messaging.Messages;
using Unite.Messaging.Services;

namespace Unite.Specs.New_Starting_Application_Specs
{
    public class FakePlugin : IMessagingService
    {
        public virtual event EventHandler<CredentialEventArgs> AuthorizationFailed;
        public virtual event EventHandler<CredentialEventArgs> CredentialsRequested;
        public virtual event EventHandler<MessagesReceivedEventArgs> MessagesReceived;

        public virtual bool CanAccept(Credentials credentials)
        {
            return true;
        }

        public virtual List<IMessage> GetMessages()
        {
            return new List<IMessage>();
        }

        public virtual void SendMessage(IIdentity recipient, string message)
        {
            
        }

        public virtual void SetCredentials(Credentials credentials)
        {
            
        }

        public virtual bool CanFind(string address)
        {
            return true;
        }

        readonly ServiceInformation _ServiceInfo = new ServiceInformation() { ServiceID = Guid.NewGuid(), ServiceName = "Fake" };
        public virtual ServiceInformation GetInformation()
        {
            return _ServiceInfo;
        }

        public virtual void StartReceiving()
        {
            
        }

        public virtual void StopReceiving()
        {
            
        }
    }
}
