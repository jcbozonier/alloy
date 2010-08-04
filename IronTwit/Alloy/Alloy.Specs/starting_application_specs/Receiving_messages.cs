using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Unite.Specs.TestObjects;
using Unite.UI.ViewModels;

namespace Unite.Specs.New_Starting_Application_Specs
{
    [TestFixture]
    public class When_a_message_is_received_while_messages_already_exist
    {
        private MessagingViewModel View;
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
            TestMessenger = new TestMessagingController();
            ReceivedMessages = new[] {new Message()};
            TestMessenger.MessagesReceived = ReceivedMessages;

            View = new MessagingViewModel(TestMessenger);

            Because();
        }

        private void Because()
        {
            View.ReceivedMessages(ReceivedMessages);
        }
    }
}
