using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoogleTalkPlugIn;
using NUnit.Framework;
using Rhino.Mocks;
using SpecUnit;
using Unite.Messaging;
using Unite.Messaging.Entities;
using Unite.Specs.FakeSpecObjects;

namespace GoogleTalkPlugin.Specs
{
   [TestFixture]
    public class When_sending_a_message : sending_message_context
   {
       private IEnumerable<IMessage> MessagesReceived;
       private IIdentity TheRecipient;
       private string TheMessage;

       /// <summary>
       /// This will enable the UI to show the sent messages
       /// </summary>
       [Test]
       public void It_should_make_sent_message_be_received()
       {
           MessagesReceived.Single().Text.ShouldEqual(TheMessage);
       }

       [Test]
       public void It_should_make_the_message_from_the_current_credentialed_user()
       {
           MessagesReceived.Single().Address.UserName.ShouldEqual("darkxanthos");
       }

       protected override void Because()
       {
           GooglePlugin.SendMessage(TheRecipient, TheMessage);
       }

       protected override void Context()
       {
           TheRecipient = new Identity("imerickson", GooglePlugin.GetInformation());
           TheMessage = "Testing yada yada";
           GooglePlugin.MessagesReceived += (s, e) => MessagesReceived = e.Messages; 
       }
   }

    public abstract class sending_message_context
    {
        protected ScenarioRepository FakeRepo;
        protected IGoogleTalkDataAccess GoogleDataAccessLayer;
        protected GoogleTalkMessagingService GooglePlugin;

        [TestFixtureSetUp]
        public void Setup()
        {
            GoogleDataAccessLayer = MockRepository.GenerateMock<IGoogleTalkDataAccess>();
            GooglePlugin = new GoogleTalkPlugIn.GoogleTalkMessagingService(GoogleDataAccessLayer);
            GooglePlugin.SetCredentials(
                new Credentials()
                    {
                        UserName = "darkxanthos",
                        Password = "",
                        ServiceInformation = GooglePlugin.GetInformation()
                    });
            Context();
            Because();
        }

        protected abstract void Because();

        protected abstract void Context();
    }
}
