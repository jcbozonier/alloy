using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using SpecUnit;
using Unite.Messaging;
using Unite.Messaging.Entities;
using Unite.Messaging.Services;
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
        public void It_should_NOT_refresh_its_messages_just_because_we_sent_one()
        {
            Assert.That(TestMessagingController.GetAllMessagesCalledCount, Is.EqualTo(1), "It should only get the messages once, at start up.");
        }

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

            ViewModel = new MessagingViewModel(new TestInteractionContext(), new TestContactQuery(), TestMessagingController);

            ViewModel.MessageToSend = MessageToBeSent;
            ViewModel.Recipient = IntendedRecipient;
        }

        protected void BecauseOf()
        {
            ViewModel.SendMessage.Execute(null);
        }
    }

    public class TestMessagingController : IUnifiedMessagingController
    {
        public int GetAllMessagesCalledCount;
        public string SentMessage;
        public string MessageRecipient;

        public void MessageToSend(string recipient, string message)
        {
            SentMessage = message;
            MessageRecipient = recipient;
        }

        public void RequestMessageUpdate()
        {
            
        }

        public IEnumerable<IMessage> GetAllMessages()
        {
            GetAllMessagesCalledCount++;
            return Enumerable.Empty<IMessage>();
        }

        public event EventHandler NewMessagesReceived;
    }

    public class TestContactQuery : IContactQuery
    {
        public IEnumerable<IIdentity> SearchFor(string name)
        {
            return Enumerable.Empty<IIdentity>();
        }
    }

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