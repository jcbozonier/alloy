using System;
using GoogleTalkPlugIn;
using NUnit.Framework;
using Rhino.Mocks;
using Rhino.Mocks.Constraints;
using SpecUnit;
using Unite.Messaging;
using Unite.Messaging.Entities;

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

        [Test]
        public void The_plugin_should_say_it_can_send_message()
        {
            Plugin.CanFind(null).ShouldBeTrue();
        }

        public void Because()
        {
            Plugin.SendMessage(Recipient, MessageToSend);
        }

        [TestFixtureSetUp]
        public void Context()
        {
            MessageToSend = "Just a test tweet";
            Recipient = new Identity(RecipientAddress, ServiceInfo);

            GTalkDataAccess = MockRepository.GenerateStub<IGoogleTalkDataAccess>();
            GTalkDataAccess
                .Stub(x => x.SetAvailableMessage(null))
                .Constraints(Is.Anything())
                .WhenCalled(x => MessageSent = (string)x.Arguments[0]);

            Plugin = new GoogleTalkMessagingService(GTalkDataAccess);
            Plugin.SetCredentials(new Credentials() { UserName = "darkxanthos" });

            Because();
        }

        private GoogleTalkMessagingService Plugin;
        private IGoogleTalkDataAccess GTalkDataAccess;
        private string MessageToSend;
        private string MessageSent;
        private Identity Recipient;
        private const string RecipientAddress = null;
        private readonly ServiceInformation ServiceInfo = new ServiceInformation() { ServiceID = Guid.NewGuid() };
    }
}
