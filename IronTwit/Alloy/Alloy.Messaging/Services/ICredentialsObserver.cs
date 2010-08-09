using Unite.Messaging.Entities;

namespace Unite.Messaging.Services
{
    public interface ICredentialsProvidedObserver
    {
        void CredentialsProvided(Credentials credentials);
    }

    public interface ICredentialsRequestedObserver
    {
        void CredentialsRequested(IServiceInformation serviceInformation);
    }

    public interface ICredentialRetryObserver
    {
        void StopRetrying(IServiceInformation serviceInformation);
    }
}