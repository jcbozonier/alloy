using NUnit.Framework;
using SpecUnit;
using Unite.Specs.TestObjects;
using Unite.UI.ViewModels;

namespace Unite.Specs.sending_message_specs
{
    [TestFixture]
    public class Sending_a_message_in_general
    {
        private MessagingViewModel ViewModel;
        private string MessageToBeSent = "message to be sent";
        private string IntendedRecipient = "intended recipient";
        private TestMessagingController TestMessagingController;

        [Test]
        public void It_should_be_able_to_send_the_message()
        {
            ViewModel.SendMessage.CanExecute(null).ShouldBeTrue();
        }

        [Test]
        public void The_message_sent_should_match_the_one_provided_by_the_user()
        {
            TestMessagingController.SentMessage.ShouldEqual(MessageToBeSent);
        }

        [Test]
        public void It_should_be_sent_to_the_correct_recipient()
        {
            TestMessagingController.MessageRecipient.ShouldEqual(IntendedRecipient);
        }

        [Test]
        public void It_should_NOT_clear_the_recipient_field()
        {
            ViewModel.Recipient.ShouldNotEqual("");
        }

        [Test]
        public void It_should_clear_the_message_field()
        {
            ViewModel.MessageToSend.ShouldEqual("");
        }

        [TestFixtureSetUp]
        public void Setup()
        {
            Context();
            BecauseOf();
        }

        protected void Context()
        {
            TestMessagingController = new TestMessagingController();

            ViewModel = new MessagingViewModel(TestMessagingController);

            ViewModel.MessageToSend = MessageToBeSent;
            ViewModel.Recipient = IntendedRecipient;
        }

        protected void BecauseOf()
        {
            ViewModel.SendMessage.Execute(null);
        }
    }
}