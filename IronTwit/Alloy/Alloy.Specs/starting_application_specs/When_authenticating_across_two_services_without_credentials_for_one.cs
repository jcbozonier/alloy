using System;
using NUnit.Framework;
using SpecUnit;
using StructureMap;
using Unite.Messaging;
using Unite.Messaging.Entities;
using Unite.Specs.FakeSpecObjects;
using Unite.UI.ViewModels;

namespace Unite.Specs.starting_application_specs
{
    [TestFixture]
    public class When_initiatializing_two_services_that_require_authentication : authenticating_two_services_context
    {
        [Test]
        public void Both_services_should_ask_to_authenticate()
        {
            FakeTwitter.FirstCredentials.ShouldNotBeNull();
            FakeGTalk.FirstCredentials.ShouldNotBeNull();
        }

        [Test]
        public void Both_services_should_be_passed_only_the_appropriate_credentials()
        {
            FakeTwitter.FirstCredentials.ShouldNotBeTheSameAs(FakeGTalk.FirstCredentials);
        }

        protected override void BecauseOf()
        {
        }

        protected override void Context()
        {
            FakesRepo.FakeUIContext
                .Assume_valid_credentials_are_provided_for_the_correct_service();
            FakesRepo.FakePluginFinder
                .Assume_these_plugins_were_found(typeof(FakeTwitterPlugin), typeof(FakeGTalkPlugin));

            View = FakesRepo.GetMainViewDontIoC();
        }
    }

    public abstract class authenticating_two_services_context
    {
        protected ScenarioRepository ScenarioRepo;
        protected MainView ViewModel;
        protected FakeTwitterPlugin FakeTwitter;
        protected FakeGTalkPlugin FakeGTalk;
        protected MainView View;

        protected bool TwitterPluginAuthenticated;
        protected bool GTalkPluginAuthenticated;
        protected ScenarioRepository FakesRepo;

        [TestFixtureSetUp]
        public void Setup()
        {
            FakesRepo = ScenarioRepository.CreateUnstubbedInstance();

            FakesRepo.InitializeIoC();

            FakeTwitter = new FakeTwitterPlugin();
            ObjectFactory.Inject(FakeTwitter);

            FakeGTalk = new FakeGTalkPlugin();
            ObjectFactory.Inject(FakeGTalk);

            Context();
            BecauseOf();
        }

        protected abstract void BecauseOf();

        protected abstract void Context();
    }

    public class FakeTwitterPlugin : FakePlugin
    {
        public string MessageSent;
        public Credentials FirstCredentials;
        private readonly ServiceInformation _ServiceInfo = new ServiceInformation() { ServiceID = Guid.NewGuid(), ServiceName = "FakeTwitterPlugin"};

        public override ServiceInformation GetInformation()
        {
            return _ServiceInfo;
        }

        public override void SetCredentials(Credentials credentials)
        {
            if (FirstCredentials == null)
                FirstCredentials = credentials;
        }

        public override void SendMessage(IIdentity recipient, string message)
        {
            MessageSent = message;
        }
    }

    public class FakeGTalkPlugin : FakePlugin
    {
        public string MessageSent;
        public Credentials FirstCredentials;
        private readonly ServiceInformation _ServiceInfo = new ServiceInformation() { ServiceID = Guid.NewGuid(), ServiceName = "FakeGTalkPlugin" };

        public override ServiceInformation GetInformation()
        {
            return _ServiceInfo;
        }

        public override void SetCredentials(Credentials credentials)
        {
            if (FirstCredentials == null)
                FirstCredentials = credentials;
        }

        public override void SendMessage(IIdentity recipient, string message)
        {
            MessageSent = message;
        }
    }
}
