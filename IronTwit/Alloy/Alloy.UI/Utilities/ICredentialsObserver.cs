using Unite.Messaging.Entities;

namespace unite.ui.utilities
{
    public interface ICredentialsObserver
    {
        void SetCredentials(Credentials credentials);
    }
}