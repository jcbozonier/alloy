using System;
using Unite.Messaging;
using Unite.Messaging.Messages;

namespace unite.ui.utilities
{
    public class CredentialManager
    {
        private IMessagingServiceManager _MessagingService;

        public CredentialManager(IMessagingServiceManager messagingService)
        {
            _MessagingService = messagingService;
            _MessagingService.CredentialsRequested += _MessagingService_CredentialsRequested;
            _MessagingService.AuthorizationFailed += _MessagingService_AuthorizationFailed;
        }

        void _MessagingService_AuthorizationFailed(object sender, CredentialEventArgs e)
        {
            var del = AuthorizationFailed;
            if (del != null)
                del(this, e);
        }

        void _MessagingService_CredentialsRequested(object sender, CredentialEventArgs e)
        {
            var del = CredentialsRequested;
            if (del != null)
                del(this, e);
        }

        public event EventHandler<CredentialEventArgs> CredentialsRequested;
        public event EventHandler<CredentialEventArgs> AuthorizationFailed;

        public void SetCredentials(Credentials credentials)
        {
             _MessagingService.SetCredentials(credentials);
        }
    }
}
