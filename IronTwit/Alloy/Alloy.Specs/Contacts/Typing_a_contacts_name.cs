using System.Linq;
using NUnit.Framework;
using SpecUnit;
using Unite.Specs.FakeSpecObjects;
using Unite.UI.ViewModels;

namespace Unite.Specs.Contacts
{
    [TestFixture]
    public class When_only_part_of_a_recipients_name_is_entered_and_no_messages_have_been_received : partial_contact_context
    {
        [Test]
        public void It_should_not_have_any_suggested_recipients()
        {
            ViewModel.SuggestedRecipients.Count().ShouldEqual(0);
        }

        protected override void Because()
        {
            ViewModel.Recipient = "@dark";
        }

        protected override void Context()
        {
            FakeRepo.FakePluginFinder
                   .Assume_a_single_messaging_service_is_found();
            
            ViewModel = FakeRepo.GetMainView();
        }
    }

    [TestFixture]
    public class When_only_part_of_a_recipients_name_is_entered_with_a_message_from_a_similar_contact_received : partial_contact_context
    {
        [Test]
        public void It_should_provide_a_name_suggestion()
        {
            ViewModel.SuggestedRecipients.Single().UserName.ShouldEqual("@darkxanthos");
        }

        protected override void Because()
        {
            ViewModel.Recipient = "@dark";
        }

        protected override void Context()
        {
            FakeRepo.FakePluginFinder
                .Assume_a_single_messaging_service_is_found();

            ViewModel = FakeRepo.GetMainView();
            Plugin.Pretend_you_received_contacts_for("@darkxanthos", "@imerickson");
        }
    }

    public abstract class partial_contact_context
    {
        protected ScenarioRepository FakeRepo;
        protected MessagingViewModel ViewModel;
        protected FakeReceivingMessagePlugin Plugin;

        [TestFixtureSetUp]
        public void Setup()
        {
            Plugin = new FakeReceivingMessagePlugin();
            FakeRepo = new ScenarioRepository(Plugin);

            Context();
            Because();
        }

        protected abstract void Because();

        protected abstract void Context();
    }
}
