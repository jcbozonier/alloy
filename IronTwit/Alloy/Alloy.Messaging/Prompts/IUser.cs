using Unite.Messaging.Entities;

namespace Unite.Messaging.Prompts
{
    public interface IUser
    {
        void PromptForCredentials(IServiceInformation serviceInformation);
        bool AuthenticationFailedRetryQuery();
    }
}
