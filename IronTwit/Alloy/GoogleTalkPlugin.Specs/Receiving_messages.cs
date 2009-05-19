using System;
using System.Linq;
using GoogleTalkPlugIn;
using NUnit.Framework;
using Rhino.Mocks;
using Rhino.Mocks.Interfaces;
using SpecUnit;
using Unite.Messaging;
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
            Data_access_OnMessage_event.Raise(FakeDataAccess, new GTalkMessageEventArgs("darkxanthos", "testing",TimeMessageWasReceived));
        }

        protected override void Context()
        {
            User = "darkxanthos";
            Message = "testing";
            TimeMessageWasReceived = DateTime.Now;
            GoogleMessagingService.CredentialsRequested += (sndr,e)=>GoogleMessagingService.SetCredentials(new Credentials());
            GoogleMessagingService.MessagesReceived += (sndr, e) => MessageReceived = e.Messages.First();
            GoogleMessagingService.StartReceiving();
        }
    }

   

    public abstract class receiving_messages_context
    {
        protected IGoogleTalkDataAccess FakeDataAccess;
        protected GoogleTalkMessagingService GoogleMessagingService;
        protected IEventRaiser Data_access_OnMessage_event;

        [TestFixtureSetUp]
        public void Setup()
        {

            FakeDataAccess = MockRepository.GenerateMock<IGoogleTalkDataAccess>();
            Data_access_OnMessage_event = FakeDataAccess.GetEventRaiser(x=>x.OnMessage += null);
            GoogleMessagingService = new GoogleTalkMessagingService(FakeDataAccess);

            Context();
            Because();
        }

        protected abstract void Because();
        protected abstract void Context();
    }
}
