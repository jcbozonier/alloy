using IronTwitterPlugIn;
using NUnit.Framework;
using Rhino.Mocks;
using SpecUnit;
using Unite.Messaging;
using Unite.Specs.FakeSpecObjects;

namespace TwitterPlugIn.Specs
{
    [TestFixture]
    public class When_asking_if_a_plugin_can_accept_a_credential_for_another_service
    {

        [Test]
        public void The_plugin_should_reply_that_it_cant()
        {
            CredentialsAccepted.ShouldBeFalse();
        }

        private void BecauseOf()
        {
            // Just creating some arbitrary credentials here that
            // obviously don't come from Twitter plug in and thus
            // can't possibly be acceptable.
            CredentialsAccepted = Twitter.CanAccept(CredentialForDifferentService);
        }

        [TestFixtureSetUp]
        public void Context()
        {
            TwitterDal = MockRepository.GenerateMock<ITwitterDataAccess>();
            Twitter = new TwitterUtilities(TwitterDal);
            CredentialForDifferentService = new ScenarioRepository().CreateFakeCredentials();

            BecauseOf();
        }

        private TwitterUtilities Twitter;
        private ITwitterDataAccess TwitterDal;
        private bool CredentialsAccepted;
        private Credentials CredentialForDifferentService;
    }
}
