using System.Linq;
using NUnit.Framework;
using Rhino.Mocks;
using Rhino.Mocks.Constraints;
using SpecUnit;
using StructureMap;
using Unite.Messaging;
using Unite.Messaging.Messages;
using Unite.Specs.FakeSpecObjects;
using Unite.UI.ViewModels;

namespace Unite.Specs.New_Starting_Application_Specs
{
    [TestFixture]
    public class When_application_starts : no_cached_credentials_no_settings
    {
        [Test]
        public void It_should_start_receiving_messages()
        {
            FakeRepo.FakeMessagePlugin.AssertWasCalled(x => x.StartReceiving());
        }

        protected override void Because()
        {
        }

        protected override void Context()
        {
            FakeRepo.FakePluginFinder
                .Assume_a_single_messaging_service_is_found();

            ViewModel = FakeRepo.GetMainView();
        }
    }

    public abstract class no_cached_credentials_no_settings
    {
        protected MessagingViewModel ViewModel;
        protected ScenarioRepository FakeRepo;

        [TestFixtureSetUp]
        public void Setup()
        {
            FakeRepo = new ScenarioRepository();
            
            Context();
            Because();
        }

        protected abstract void Because();

        protected abstract void Context();
    }
}