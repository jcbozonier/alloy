using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Rhino.Mocks;
using SpecUnit;
using Unite.Messaging.Entities;
using Unite.Messaging.Messages;
using Unite.Messaging.Services;
using Unite.Specs.FakeSpecObjects;
using Unite.Specs.TestObjects;
using Unite.UI.Utilities;
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

        [Ignore("This test is not applicable on this object but the behavior needs verification.")]
        [Test]
        public void It_should_place_the_most_recent_message_at_the_top_of_the_list()
        {
            View.Messages.First().TimeStamp.ShouldBeGreaterThan(View.Messages.ToArray()[1].TimeStamp);
        }

        [TestFixtureSetUp]
        public void Context()
        {
            FakeMessagePlugin = new FakeReceivingMessagePlugin();

            TestMessenger = new TestMessagingController();
            ReceivedMessages = new[] {new Message()};
            TestMessenger.MessagesReceived = ReceivedMessages;

            View = new MessagingViewModel(new TestInteractionContext(), new TestContactQuery(), TestMessenger);
            
            PreexistingMessages = new List<IMessage>(View.Messages);

            // Sleep for a minute so that the time spans don't match.
            
            Because();
        }

        private void Because()
        {
            TestMessenger.NewMessagesReceived_Occurred();
        }
    }

    public class FakeReceivingMessagePlugin : FakePlugin
    {
        public override event EventHandler<MessagesReceivedEventArgs> MessagesReceived;

        public string Test;

        public void PretendThatYouJustReceivedAMessage()
        {
            var eventArgs = new MessagesReceivedEventArgs(
                new List<IMessage>(){new ScenarioRepository().GetMessage(DateTime.Now)});

            MessagesReceived(this, eventArgs);
        }

        public override void StartReceiving()
        {
            var eventArgs = new MessagesReceivedEventArgs(
                new []{new ScenarioRepository().GetMessage(DateTime.Now)});

            MessagesReceived(this, eventArgs);
        }
    }
    
}
