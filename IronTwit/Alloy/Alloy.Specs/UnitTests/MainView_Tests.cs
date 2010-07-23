using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Unite.Specs.TestObjects;

namespace Unite.Specs.UnitTests
{
    [TestFixture]
    public class MainView_Tests
    {
        [Test]
        public void When_setting_messages()
        {
            var updatedProperty = "";
            var messagingView = new Unite.UI.ViewModels.MessagingViewModel(new TestMessagingController());

            messagingView.PropertyChanged += (sndr, arg) => updatedProperty = arg.PropertyName;
            var receivedMessages = new []{new Message()};
            messagingView.ReceivedMessages(receivedMessages);

            Assert.That(messagingView.Messages, Is.EquivalentTo(receivedMessages), "The received messages should match the ones in the UI.");
            Assert.That(updatedProperty, Is.EqualTo("Messages"), "It should update the messages in the UI");
        }
    }
}
