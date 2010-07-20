using NUnit.Framework;
using Rhino.Mocks;
using Rhino.Mocks.Constraints;
using SpecUnit;
using Unite.Messaging;
using Unite.Specs.FakeSpecObjects;
using Unite.UI.ViewModels;

namespace Unite.Specs.sending_message_specs
{
    [TestFixture]
    public class When_sending_a_message_containing_a_code_sample : code_sample_context
    {
        [Test]
        public void The_sent_message_should_contain_a_url()
        {
            MessageSent.Contains(CodeSampleUrl).ShouldBeTrue();
        }

        [Test]
        public void The_sent_message_should_NOT_contain_the_code_sample()
        {
            MessageSent.Contains(MinimalCodeSample).ShouldBeFalse();
        }

        protected override void Because_of_this()
        {
            ViewModel.SendMessage.Execute(null);
        }

        protected override void In_this_context()
        {
            FakeRepo.FakeMessagePlugin
                .Assume_a_message_will_be_sent_and_deliver_the_arguments_to
                (
                    (recipient, message) => MessageSent = message
                );

            FakeRepo.FakeCodeFormatter
                .Assume_code_was_pasted_and_return(CodeSampleUrl);

            // This is the definition of a code sample.
            // Any message that contains a newline and a tab character
            // immediately after at least one new line.
            MinimalCodeSample = "\n\t";
            MessageToSend = MinimalCodeSample;

            ViewModel.MessageToSend = MessageToSend;
        }
    }

    public abstract class code_sample_context
    {
        protected ScenarioRepository FakeRepo;
        protected MessagingViewModel ViewModel;
        protected string MinimalCodeSample;
        protected string MessageToSend;
        protected string MessageSent;
        protected string CodeSampleUrl;

        [TestFixtureSetUp]
        public void Setup()
        {
            CodeSampleUrl = "[gestures with hand] 'I **AM** a URL'";

            FakeRepo = ScenarioRepository.CreateStubbedInstance();

            ViewModel = FakeRepo.GetMainView();

            In_this_context();
            Because_of_this();
        }

        protected abstract void Because_of_this();

        protected abstract void In_this_context();
    }
}
