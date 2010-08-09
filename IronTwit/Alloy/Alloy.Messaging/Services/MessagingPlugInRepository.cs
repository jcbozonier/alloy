using System;
using System.Collections.Generic;
using StructureMap;
using Unite.Messaging.Messages;

namespace Unite.Messaging.Services
{
    public class MessagingPlugInRepository : IServiceProvider, IMessagingPlugInRepository

    {
        private ICredentialsRequestedObserver _CredentialUpdates;
        private readonly List<IMessagingService> _Services;

        public MessagingPlugInRepository()
        {
            _Services = new List<IMessagingService>();
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

            _Services.Add(service);
        }

        public void OnCredentialsRequestedNotify(ICredentialsRequestedObserver credentialUpdates)
        {
            _CredentialUpdates = credentialUpdates;
        }

        public void Add(Type messagingServiceType)
        {
            var service = (IMessagingService)ObjectFactory.GetInstance(messagingServiceType);
            _Add(service);
        }

        private void service_AuthorizationFailed(object sender, CredentialEventArgs e)
        {
        }

        private void _GetCredentials(object sender, CredentialEventArgs e)
        {
            if (_CredentialUpdates != null)
                _CredentialUpdates.CredentialsRequested(e.ServiceInfo);
        }

        public IEnumerable<IMessagingService> GetAllServices()
        {
            return _Services;
        }

        public void ForEachPlugIn(Action<IMessagingService> takeAction)
        {
            foreach(var plugin in _Services)
            {
                takeAction(plugin);
            }
        }
    }
}