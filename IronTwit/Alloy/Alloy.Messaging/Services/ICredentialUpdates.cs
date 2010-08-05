using Unite.Messaging.Messages;

namespace Unite.Messaging.Services
{
    public interface ICredentialUpdates
    {
        void CredentialsRequested(CredentialEventArgs e);
        void AuthorizationFailed(CredentialEventArgs e);
    }
}