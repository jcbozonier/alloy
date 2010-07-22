using System.Text;
using Unite.Messaging;
using Unite.Messaging.Entities;

namespace Unite.Specs.TestObjects
{
    public class TestInteractionContext : IInteractionContext
    {
        public Credentials GetCredentials(IServiceInformation serviceInformation)
        {
            return new Credentials();
        }

        public bool AuthenticationFailedRetryQuery()
        {
            return false;
        }
    }
}
