using System;
using System.Linq;
using GoogleTalkPlugIn;
using GoogleTalkPlugin.Specs.FakeSpecObjects;
using NUnit.Framework;
using SpecUnit;
using Unite.Messaging.Entities;

namespace GoogleTalkPlugin.Specs
{
    [TestFixture]
    public class When_a_message_is_received_from_google_talk : receiving_messages_context
    {
        private IMessage MessageReceived;
        private string User;
        private string Message;
        private DateTime TimeMessageWasReceived;

        [Test]
        public void The_message_should_be_received_in_UTC_time()
        {
            MessageReceived.TimeStamp.ShouldEqual(TimeMessageWasReceived.ToUniversalTime());
        }

        [Test]
        public void The_correct_message_should_be_received_from_the_service()
        {
            MessageReceived.Text.ShouldEqual(Message);
        }

        [Test]
        public void The_message_should_be_received_from_the_correct_user()
        {
            MessageReceived.Address.UserName.ShouldEqual(User);
        }

        protected override void Because()
        {
            FakeDataAccess.Pretend_that_a_message_was_received("darkxanthos", "testing", TimeMessageWasReceived);
        }

        protected override void Context()
        {
            User = "darkxanthos";
            Message = "testing";
            TimeMessageWasReceived = DateTime.Now;
            GoogleMessagingService.MessagesReceived += (sndr, e) => MessageReceived = e.Messages.First();
            GoogleMessagingService.StartReceiving();
        }
    }

    public abstract class receiving_messages_context
    {
        protected FakeDataAccess FakeDataAccess;
        protected GoogleTalkMessagingService GoogleMessagingService;

        [TestFixtureSetUp]
        public void Setup()
        {

            FakeDataAccess = new FakeDataAccess(); 
            GoogleMessagingService = new GoogleTalkMessagingService(FakeDataAccess);

            Context();
            Because();
        }

        protected abstract void Because();
        protected abstract void Context();
    }

    
}
