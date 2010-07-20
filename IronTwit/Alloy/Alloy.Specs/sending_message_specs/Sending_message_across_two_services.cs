using NUnit.Framework;
using Rhino.Mocks;
using SpecUnit;
using StructureMap;
using Unite.Messaging;
using Unite.Specs.FakeSpecObjects;
using Unite.UI.ViewModels;

namespace Unite.Specs.sending_message_specs
{
    [TestFixture]
    public class When_sending_message_that_is_valid_for_multiple_services : multiple_service_context
    {
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

        protected override void Context()
        {
            FakesRepo.FakeUIContext
                .Assume_valid_credentials_are_provided_for_the_correct_service();
            FakesRepo.FakePluginFinder
                .Assume_that_two_different_plugins_are_found();

            View = FakesRepo.GetMainViewDontIoC();
            View.MessageToSend = MessageToSend;
            View.Recipient = null;
        }

        protected override void Because()
        {
            View.SendMessage.Execute(null);
        }
    }

    public abstract class multiple_service_context
    {
        protected MessagingViewModel View;
        protected string MessageToSend;
        protected FakeTwitterPlugin FakeTwitter;
        protected FakeGTalkPlugin FakeGTalk;
        protected IInteractionContext FakeUI;
        protected ScenarioRepository FakesRepo;

        [TestFixtureSetUp]
        public void Setup()
        {
            FakesRepo = ScenarioRepository.CreateUnstubbedInstance();

            FakesRepo.InitializeIoC();
            FakeTwitter = new FakeTwitterPlugin();
            FakeGTalk = new FakeGTalkPlugin();
            ObjectFactory.Inject(FakeTwitter);
            ObjectFactory.Inject(FakeGTalk);

            MessageToSend = "Test message";

            Context();
            Because();
        }

        protected abstract void Because();

        protected abstract void Context();
    }
}