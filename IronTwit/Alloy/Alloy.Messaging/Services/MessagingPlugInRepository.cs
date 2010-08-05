using System;
using System.Collections.Generic;
using StructureMap;
using Unite.Messaging.Messages;

namespace Unite.Messaging.Services
{
    public class MessagingPlugInRepository : IServiceProvider, IMessagingPlugInRepository

    {
        private ICredentialUpdates _CredentialUpdates;
        private readonly List<IMessagingService> Services;

        public MessagingPlugInRepository()
        {
            Services = new List<IMessagingService>();
        }

        private void _AddServiceProvider(Type serviceType)
        {
            var service = (IMessagingService)ObjectFactory.GetInstance(serviceType);
            _Add(service);
        }

        public void Add(params IMessagingService[] services)
        {
            foreach(var service in services)
            {
                _Add(service);
            }
        }

        private void _Add(IMessagingService service)
        {
            service.CredentialsRequested += _GetCredentials;
            service.AuthorizationFailed += service_AuthorizationFailed;

            Services.Add(service);
        }

        public void OnCredentialUpdatesNotify(ICredentialUpdates credentialUpdates)
        {
            _CredentialUpdates = credentialUpdates;
        }

        public void Add(Type messagingServiceType)
        {
            _AddServiceProvider(messagingServiceType);
        }

        private void service_AuthorizationFailed(object sender, CredentialEventArgs e)
        {
            if (_CredentialUpdates != null)
                _CredentialUpdates.AuthorizationFailed(e);
        }

        private void _GetCredentials(object sender, CredentialEventArgs e)
        {
            if (_CredentialUpdates != null)
                _CredentialUpdates.CredentialsRequested(e);
        }

        public IEnumerable<IMessagingService> GetAllServices()
        {
            return Services;
        }

        public void ForEachPlugIn(Action<IMessagingService> takeAction)
        {
            foreach(var plugin in Services)
            {
                takeAction(plugin);
            }
        }

        public event EventHandler<CredentialEventArgs> CredentialsRequested;
        public event EventHandler<CredentialEventArgs> AuthorizationFailed;
    }
}