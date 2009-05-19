using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using NUnit.Framework;
using Rhino.Mocks;
using SpecUnit;
using Unite.Messaging.Entities;
using Unite.Messaging.Messages;
using Unite.Messaging.Services;
using Unite.Specs.FakeSpecObjects;
using Unite.UI.Utilities;
using Unite.UI.ViewModels;

namespace Unite.Specs.New_Starting_Application_Specs
{
    [TestFixture]
    public class When_a_message_is_received_while_messages_already_exist
    {
        private MainView View;
        private List<IMessage> PreexistingMessages;
        private FakeReceivingMessagePlugin FakeMessagePlugin;

        [Test]
        public void It_should_increase_the_number_of_viewable_messages_by_one()
        {
            View.Messages.Count().ShouldBeGreaterThan(PreexistingMessages.Count);
        }

        [Test]
        public void It_should_place_the_most_recent_message_at_the_top_of_the_list()
        {
            View.Messages.First().TimeStamp.ShouldBeGreaterThan(View.Messages.ToArray()[1].TimeStamp);
        }

        [TestFixtureSetUp]
        public void Context()
        {
            FakeMessagePlugin = new FakeReceivingMessagePlugin();

            var repo = new ScenarioRepository(FakeMessagePlugin);

            repo.FakePluginFinder
                .Assume_a_single_messaging_service_is_found();

            View = repo.GetMainView();
            
            PreexistingMessages = new List<IMessage>(View.Messages);

            // Sleep for a minute so that the time spans don't match.
            
            Because();
        }

        private void Because()
        {
            FakeMessagePlugin.PretendThatYouJustReceivedAMessage(); 
            //Give us time to receive it...
            Thread.Sleep(100);
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
