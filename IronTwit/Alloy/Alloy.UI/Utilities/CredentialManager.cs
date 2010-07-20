using System;
using System.Collections.Generic;
using Unite.Messaging;
using Unite.Messaging.Entities;
using Unite.Messaging.Messages;

namespace unite.ui.utilities
{
    public class CredentialAuthorizationController
    {
        private readonly IUnifiedMessagingService _MessagingService;
        private Dictionary<ServiceInformation, bool> _RetryOnAuthFailure;
        private readonly IInteractionContext _Interactions;

        public CredentialAuthorizationController(IUnifiedMessagingService messagingService, IInteractionContext uiContext)
        {
            _Interactions = uiContext;
            _MessagingService = messagingService;
            _MessagingService.CredentialsRequested += _MessagingService_CredentialsRequested;
            _MessagingService.AuthorizationFailed += _MessagingService_AuthorizationFailed;
        }

        public event EventHandler<CredentialEventArgs> CredentialsRequested;
        public event EventHandler<CredentialEventArgs> AuthorizationFailed;

        public void SetCredentials(Credentials credentials)
        {
             _MessagingService.SetCredentials(credentials);
        }

        void _MessagingService_CredentialsRequested(object sender, CredentialEventArgs e)
        {
            var credentials = _Interactions.GetCredentials(e.ServiceInfo);
            SetCredentials(credentials);
        }

        void _MessagingService_AuthorizationFailed(object sender, CredentialEventArgs e)
        {
            // This needs to be refactored out of the view model for sure and placed
            // into the credential manager.
            _RetryOnAuthFailure = _RetryOnAuthFailure ?? new Dictionary<ServiceInformation, bool>();

            if (!_RetryOnAuthFailure.ContainsKey(e.ServiceInfo))
                _RetryOnAuthFailure[e.ServiceInfo] = _Interactions.AuthenticationFailedRetryQuery();

            if (!_RetryOnAuthFailure[e.ServiceInfo]) return;

            _MessagingService_CredentialsRequested(this, e);
        }
    }
}
