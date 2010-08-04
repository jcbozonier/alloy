using GoogleTalkPlugIn;
using NUnit.Framework;
using Rhino.Mocks;
using Unite.Messaging;
using Unite.Messaging.Entities;

namespace GoogleTalkPlugin.Specs
{
    [TestFixture]
    public class When_authenticating_and_user_provides_no_credentials : authentication_context
    {
        [Test]
        public void It_should_never_login_to_the_data_access_layer()
        {
            FakeDataAccess.AssertWasNotCalled(x=>x.Login(null, null));
        }

        protected override void BecauseOf()
        {
            Plugin.StartReceiving();
        }

        protected override void Context()
        {
            Plugin.SetCredentials(new Credentials(){UserName = null, Password = null});
        }
    }

    public abstract class authentication_context
    {
        protected IGoogleTalkDataAccess FakeDataAccess;
        protected GoogleTalkMessagingService Plugin;
        protected string UsernameSet;
        protected string PasswordSet;

        [TestFixtureSetUp]
        public void Setup()
        {
            FakeDataAccess = MockRepository.GenerateMock<IGoogleTalkDataAccess>();
            FakeDataAccess.Stub(x => x.Login(null, null));
            Plugin = new GoogleTalkMessagingService(FakeDataAccess);

            Context();
            BecauseOf();
        }

        protected abstract void BecauseOf();

        protected abstract void Context();
    }
}
