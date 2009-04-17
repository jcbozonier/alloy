using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Rhino.Mocks;
using SpecUnit;
using StructureMap;
using Unite.Messaging;
using Unite.Messaging.Messages;
using Unite.Specs.FakeSpecObjects;
using Unite.UI.ViewModels;

namespace Unite.Specs.New_sending_message_specs
{
    [TestFixture]
    public class When_sending_message_that_is_valid_for_multiple_services
    {
        private MainView View;
        private string MessageToSend;
        private FakeTwitterPlugin FakeTwitter;
        private FakeGTalkPlugin FakeGTalk;

        [Test]
        public void It_should_send_message_using_each_service()
        {
            FakeTwitter.MessageSent.ShouldNotBeNull();
            FakeGTalk.MessageSent.ShouldNotBeNull();
        }

        [Test]
        public void It_should_send_the_message_that_we_wanted_sent()
        {
            FakeTwitter.MessageSent.ShouldEqual(MessageToSend);
            FakeGTalk.MessageSent.ShouldEqual(MessageToSend);
        }

        [TestFixtureSetUp]
        public void Context()
        {
            MessageToSend = "Test message";

            var fakesRepo = new ScenarioRepository();
            fakesRepo.FakePluginFinder
                .Stub(x => x.GetAllPlugins())
                .Return(new[] {typeof (FakeTwitterPlugin), typeof (FakeGTalkPlugin)});

            FakeTwitter = new FakeTwitterPlugin();
            FakeGTalk = new FakeGTalkPlugin();

            fakesRepo.InitializeIoC();
            ObjectFactory.Inject(FakeTwitter);
            ObjectFactory.Inject(FakeGTalk);

            View = fakesRepo.GetMainViewDontIoC();

            View.Init();

            View.MessageToSend = MessageToSend;
            View.Recipient = null;

            Because();
        }

        private void Because()
        {
            View.SendMessage.Execute(null);
        }
    }

    public class FakeTwitterPlugin : FakePlugin
    {
        public string MessageSent;

        public override void SendMessage(IIdentity recipient, string message)
        {
            MessageSent = message;
        }
    }

    public class FakeGTalkPlugin : FakePlugin
    {
        public string MessageSent;

        public override void SendMessage(IIdentity recipient, string message)
        {
            MessageSent = message;
        }
    }
}
