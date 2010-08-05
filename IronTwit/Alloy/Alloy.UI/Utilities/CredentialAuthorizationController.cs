﻿using System.Collections.Generic;
using Unite.Messaging.Entities;
using Unite.Messaging.Messages;
using Unite.Messaging.Prompts;

namespace unite.ui.utilities
{
    public class CredentialAuthorizationController : ICredentialsObserver
    {
        private readonly IUnifiedMessagingService _MessagingService;
        private Dictionary<ServiceInformation, bool> _RetryOnAuthFailure;
        private readonly IUser _User;

        public CredentialAuthorizationController(IUnifiedMessagingService messagingService, IUser user)
        {
            _User = user;
            _MessagingService = messagingService;
            _MessagingService.CredentialsRequested += _MessagingService_CredentialsRequested;
            _MessagingService.AuthorizationFailed += _MessagingService_AuthorizationFailed;
        }

        public void SetCredentials(Credentials credentials)
        {
             _MessagingService.SetCredentials(credentials);
        }

        void _MessagingService_CredentialsRequested(object sender, CredentialEventArgs e)
        {
            _User.PromptForCredentials(e.ServiceInfo);
        }

        void _MessagingService_AuthorizationFailed(object sender, CredentialEventArgs e)
        {
            // This needs to be refactored out of the view model for sure and placed
            // into the credential manager.
            _RetryOnAuthFailure = _RetryOnAuthFailure ?? new Dictionary<ServiceInformation, bool>();

            if (!_RetryOnAuthFailure.ContainsKey(e.ServiceInfo))
                _RetryOnAuthFailure[e.ServiceInfo] = _User.AuthenticationFailedRetryQuery();

            if (!_RetryOnAuthFailure[e.ServiceInfo]) return;

            _MessagingService_CredentialsRequested(this, e);
        }
    }
}
