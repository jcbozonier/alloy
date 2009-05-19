using NUnit.Framework;
using Rhino.Mocks;
using SpecUnit;
using Unite.Messaging;
using Unite.Messaging.Messages;
using Unite.Specs.FakeSpecObjects;
using Unite.UI.ViewModels;

namespace Unite.Specs.sending_message_specs
{
    [TestFixture]
    public class Sending_a_message_in_general : context
    {
        [Test]
        public void It_should_be_sent()
        {
            MessageSent.ShouldNotBeNull();
        }

        [Test]
        public void It_should_NOT_refresh_its_messages_just_because_we_sent_one()
        {
            FakeRepo.FakeMessagePlugin.AssertWasNotCalled(x=>x.GetMessages());
        }

        [Test]
        public void It_should_be_able_to_send_the_message()
        {
            ViewModel.SendMessage.CanExecute(null).ShouldBeTrue();
        }

        [Test]
        public void The_message_sent_should_match_the_one_provided_by_the_user()
        {
            MessageSent.ShouldEqual(MessageToBeSent);
        }

        [Test]
        public void It_should_be_sent_to_the_correct_recipient()
        {
            ActualRecipient.UserName.ShouldEqual(IntendedRecipient);
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

        protected override void Context()
        {
            FakeRepo.FakePluginFinder
                .Assume_a_single_messaging_service_is_found();

            FakeRepo.FakeMessagePlugin
                .Assume_it_can_find_any_address()
                .Assume_a_message_will_be_sent_and_deliver_the_arguments_to(SendingMessageCallBack);

            ViewModel.MessageToSend = MessageToBeSent;
            ViewModel.Recipient = IntendedRecipient;
        }

        protected override void BecauseOf()
        {
            ViewModel.SendMessage.Execute(null);
        }
    }

    public abstract class context
    {
        [TestFixtureSetUp]
        public void Setup()
        {
            IntendedRecipient = "@darkxanthos";
            MessageToBeSent = "Test message";

            FakeRepo = ScenarioRepository.CreateStubbedInstance();

            ViewModel = FakeRepo.GetMainView();

            Context();
            BecauseOf();
        }

        protected abstract void BecauseOf();

        protected abstract void Context();

        protected void SendingMessageCallBack(IIdentity recipient, string message)
        {
            ActualRecipient = recipient;
            MessageSent = message;
        }

        protected ScenarioRepository FakeRepo;
        protected MainView ViewModel;
        protected IMessagingService FakeMessagingService;
        protected string MessageSent;
        protected string IntendedRecipient;
        protected IIdentity ActualRecipient;
        protected string MessageToBeSent;
    }
}