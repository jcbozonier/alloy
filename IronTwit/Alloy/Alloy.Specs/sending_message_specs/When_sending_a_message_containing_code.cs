using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Unite.Messaging.Extras;
using Unite.Specs.TestObjects;

namespace Unite.Specs.sending_message_specs
{
    [TestFixture]
    public class When_sending_a_message_containing_a_code_sample
    {
        [Test]
        public void The_sent_message_should_contain_a_url()
        {
            var testCodePasteService = new TestCodePasteService();
            var urlFormatter = new AutoFormatCodePastesAsUrls(testCodePasteService);
            var codeSample = @"
public class Foo
{
    public void Bar()
    {

    }
}";
            var formattedMessage = urlFormatter.ApplyFormatting(codeSample);


            Assert.That(formattedMessage, Is.EqualTo(testCodePasteService.CodePasteText), "It should replace the message with the value from th code paste service.");
        }

        [Test]
        public void The_sent_message_should_NOT_contain_the_code_sample()
        {
            
        }

        
    }
}
