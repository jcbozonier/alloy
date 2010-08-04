using Unite.Messaging.Entities;

namespace Unite.Messaging.Prompts
{
    public interface IInteractionContext
    {
        Credentials GetCredentials(IServiceInformation serviceInformation);
        bool AuthenticationFailedRetryQuery();
    }
}
