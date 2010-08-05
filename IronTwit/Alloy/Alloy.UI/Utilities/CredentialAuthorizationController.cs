using System;
using System.Collections.Generic;
using Unite.Messaging.Entities;
using Unite.Messaging.Messages;
using Unite.Messaging.Prompts;
using Unite.Messaging.Services;

namespace unite.ui.utilities
{
    public class CredentialAuthorizationController : ICredentialsObserver, ICredentialUpdates, ICredentialsProvidedObserver
    {
        private readonly IUnifiedMessagingService _MessagingService;
        private ICredentialsRequestedObserver _CredentialsRequestedObserver;

        public CredentialAuthorizationController(IUnifiedMessagingService messagingService)
        {
            _MessagingService = messagingService;
        }

        public void SetCredentials(Credentials credentials)
        {
             _MessagingService.SetCredentials(credentials);
        }

        public void CredentialsRequested(CredentialEventArgs e)
        {
            _CredentialsRequestedObserver.CredentialsNeeded(e.ServiceInfo);
        }

        public void AuthorizationFailed(CredentialEventArgs e)
        {
            
        }

        public void CredentialsProvided(Credentials credentials)
        {
            _MessagingService.SetCredentials(credentials);
        }

        public void OnCredentialsRequestedNotify(ICredentialsRequestedObserver credentialsRequestedObserver)
        {
            _CredentialsRequestedObserver = credentialsRequestedObserver;
        }
    }
}
