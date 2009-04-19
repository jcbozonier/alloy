using System;
using NUnit.Framework;
using Rhino.Mocks;
using Unite.Messaging.Messages;
using Unite.Specs.FakeSpecObjects;
using Unite.UI.ViewModels;

namespace Unite.Specs.starting_application_specs
{
    [TestFixture]
    public class When_messaging_service_needs_credentials : cached_credentials_no_settings
    {
        [Test]
        public void It_should_ask_ui_for_credentials()
        {
            FakeRepo.FakeUIContext
                .AssertWasCalled(x => x.GetCredentials(null));
        }

        protected override void Because()
        {
            ViewModel.Init();
        }

        protected override void Context()
        {
            FakeRepo.FakePluginFinder
                .Assume_a_single_messaging_service_is_found();

            FakeRepo.FakeUIContext
                .Assume_some_credential_is_provided();

            ViewModel = FakeRepo.GetMainView();
        }
    }

    public class GetCredentialsPlugIn : FakePlugin
    {
        public override event EventHandler<CredentialEventArgs> CredentialsRequested;

        public override void StartReceiving()
        {
            CredentialsRequested(this, new CredentialEventArgs());
        }
    }

    public abstract class cached_credentials_no_settings
    {
        protected MainView ViewModel;
        protected ScenarioRepository FakeRepo;
        protected IMessagingService FakeMessagingPlugin;

        [TestFixtureSetUp]
        public void Setup()
        {
            FakeMessagingPlugin = new GetCredentialsPlugIn();

            FakeRepo = new ScenarioRepository(FakeMessagingPlugin);

            Context();
            Because();
        }

        protected abstract void Because();

        protected abstract void Context();
    }
}