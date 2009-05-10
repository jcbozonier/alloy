using System;
using System.Collections.Generic;
using System.Linq;
using Unite.Messaging.Entities;
using Unite.Messaging.Messages;

namespace Unite.Messaging.Services
{
    public class ServicesManager : IMessagingServiceManager, IContactService
    {
        private readonly ServiceInformation _ServiceInfo = new ServiceInformation()
        {
            ServiceID = new Guid("{FC1DF655-BBA0-4036-B352-CA98E1B56001}"),
            ServiceName = "Service Manager"
        };

        private readonly IServiceProvider _Provider;
        private readonly IServiceResolver _Resolver;

        private readonly IEnumerable<IMessagingService> _Services;

        /// <summary>
        /// Invoked when the users credentials were received and used to 
        /// authenticate but were signaled to be invalid by the service.
        /// </summary>
        public event EventHandler<CredentialEventArgs> AuthorizationFailed;

        /// <summary>
        /// Invoked when new credentials are required to log into a service.
        /// </summary>
        public event EventHandler<CredentialEventArgs> CredentialsRequested;

        /// <summary>
        /// Invoked whenever new messages are received from any one of the
        /// loaded services.
        /// </summary>
        public event EventHandler<MessagesReceivedEventArgs> MessagesReceived;

        public event EventHandler<ContactEventArgs> OnContactsReceived;

        public ServicesManager(IServiceProvider provider)
        {
            _Provider = provider;
            _Provider.CredentialsRequested += Provider_CredentialsRequested;
            _Provider.AuthorizationFailed += Provider_AuthorizationFailed;
            _Resolver = new ServiceResolver(_Provider);

            _Services = _Provider.GetServices();

        }

        void Provider_AuthorizationFailed(object sender, CredentialEventArgs e)
        {
            if (AuthorizationFailed != null)
                AuthorizationFailed(sender, e);
        }

        void Provider_CredentialsRequested(object sender, CredentialEventArgs e)
        {
            if (CredentialsRequested != null)
                CredentialsRequested(sender, e); 
        }

        /// <summary>
        /// TODO: This doesn't appear to alter any specs. Maybe not important?
        /// </summary>
        /// <param name="credentials"></param>
        /// <returns></returns>
        public bool CanAccept(Credentials credentials)
        {
            return true;
        }

        /// <summary>
        /// Gets an aggregated list of messages received from all plugins.
        /// </summary>
        /// <returns></returns>
        public void GetMessages()
        {
            var messages = new List<IMessage>();
            var services = _Services;

            foreach (var service in services)
            {
                messages.AddRange(service.GetMessages());
            }

            if(MessagesReceived != null)
                MessagesReceived(this, new MessagesReceivedEventArgs(messages));
        }

        /// <summary>
        /// Call this to send the provided message using any and
        /// all services that know how to broadcast to the 
        /// provided address/username.
        /// </summary>
        /// <param name="recipient"></param>
        /// <param name="message"></param>
        public void SendMessage(string recipient, string message)
        {
            var servicesToUse = _Provider.GetServices(recipient);
            foreach (var service in servicesToUse)
                // TODO: If I remove the service info NO specs are impacted. Is this really
                // important then?
                service.SendMessage(new Identity(recipient, service.GetInformation()), message);
        }

        /// <summary>
        /// TODO: Need specs that cover the use of this method.
        /// </summary>
        /// <param name="credentials"></param>
        public void SetCredentials(Credentials credentials)
        {
            var services = _Services;

            foreach (var service in services)
            {
                if(service.CanAccept(credentials))
                    service.SetCredentials(credentials);
            }
        }

        /// <summary>
        /// Tells whether or not the given messaging recipient can be
        /// found using any known service.
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public bool CanFind(string address)
        {
            var foundService = _Resolver.GetService(address);
            return foundService != null;
        }

        /// <summary>
        /// Gets this service's identifying information.
        /// </summary>
        /// <returns></returns>
        public ServiceInformation GetInformation()
        {
            return _ServiceInfo;
        }

        /// <summary>
        /// Call to start up each service.
        /// </summary>
        public void StartReceiving()
        {
            var services = _Services;

            foreach (var service in services)
            {
                service.ContactsReceived += service_ContactsReceived;
                service.MessagesReceived += service_MessagesReceived;
                service.StartReceiving();
            }
        }

        private void service_ContactsReceived(object sender, ContactEventArgs e)
        {
            if (OnContactsReceived != null)
                OnContactsReceived(this, e);
        }

        /// <summary>
        /// Shuts down all of the plugins. Usually used in prep
        /// for shutting down the application.
        /// </summary>
        public void StopReceiving()
        {
            var services = _Services;

            foreach (var service in services)
            {
                service.MessagesReceived -= service_MessagesReceived;
                service.StopReceiving();
            }
        }

        private void service_MessagesReceived(object sender, MessagesReceivedEventArgs e)
        {
            if (MessagesReceived != null)
                MessagesReceived(sender, e);
        }
    }
}