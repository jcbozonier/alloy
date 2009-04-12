using GoogleTalkPlugIn;
using NUnit.Framework;
using Rhino.Mocks;
using Rhino.Mocks.Constraints;
using SpecUnit;
using Unite.Messaging;

namespace GoogleTalkPlugin.Specs
{
    [TestFixture]
    public class When_sending_a_recipientless_message
    {
        [Test]
        public void It_should_set_the_available_message_to_be_the_message()
        {
            MessageSent.ShouldEqual(MessageToSend);
        }

        private GoogleTalkMessagingService Plugin;
        private IGoogleTalkDataAccess GTalkDataAccess;
        private string MessageToSend;
        private string MessageSent;

        public void Because()
        {
            Plugin.SendMessage(null, MessageToSend);
        }

        [TestFixtureSetUp]
        public void Context()
        {
            MessageToSend = "Just a test tweet";

            GTalkDataAccess = MockRepository.GenerateStub<IGoogleTalkDataAccess>();
            GTalkDataAccess
                .Stub(x => x.SetAvailableMessage(null))
                .Constraints(Is.Anything())
                .WhenCalled(x => MessageSent = (string)x.Arguments[0]);

            Plugin = new GoogleTalkMessagingService(GTalkDataAccess);
            Plugin.SetCredentials(new Credentials() { UserName = "darkxanthos" });

            Because();
        }
    }
}
