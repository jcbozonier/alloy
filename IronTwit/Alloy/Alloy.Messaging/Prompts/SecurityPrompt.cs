using Unite.Messaging.Entities;

namespace Unite.Messaging.Prompts
{
    public interface SecurityPrompt
    {
        void CredentialsNeeded(IServiceInformation serviceInformation);
        void AuthenticationFailedRetryQuery(ServiceInformation serviceInfo);
    }
}
