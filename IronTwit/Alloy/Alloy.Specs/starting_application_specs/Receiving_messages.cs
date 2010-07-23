using System.Collections.Generic;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Unite.Messaging.Entities;
using Unite.Specs.FakeSpecObjects;
using Unite.Specs.TestObjects;
using Unite.UI.ViewModels;

namespace Unite.Specs.New_Starting_Application_Specs
{
    [TestFixture]
    public class When_a_message_is_received_while_messages_already_exist
    {
        private MessagingViewModel View;
        private List<IMessage> PreexistingMessages;
        private FakeReceivingMessagePlugin FakeMessagePlugin;
        private TestMessagingController TestMessenger;
        private Message[] ReceivedMessages;

        [Test]
        public void It_should_increase_the_number_of_viewable_messages_by_one()
        {
            Assert.That(View.Messages, Is.EquivalentTo(ReceivedMessages), "It should receive the messages provided.");
        }

        [TestFixtureSetUp]
        public void Context()
        {
            FakeMessagePlugin = new FakeReceivingMessagePlugin();

            TestMessenger = new TestMessagingController();
            ReceivedMessages = new[] {new Message()};
            TestMessenger.MessagesReceived = ReceivedMessages;

            View = new MessagingViewModel(TestMessenger);
            
            PreexistingMessages = new List<IMessage>(View.Messages);

            // Sleep for a minute so that the time spans don't match.
            
            Because();
        }

        private void Because()
        {
            TestMessenger.NewMessagesReceived_Occurred();
        }
    }
}
