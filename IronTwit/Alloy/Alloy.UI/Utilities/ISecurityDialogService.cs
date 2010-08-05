using Unite.Messaging.Entities;

namespace Unite.UI.Utilities
{
    public interface ISecurityDialogService
    {
        void RequestCredentialsFor(string messagingServiceName, string suggestedUserName, IServiceInformation serviceInformation);
        void AuthenticationFailedRetryQuery(ServiceInformation serviceInfo);
    }
}