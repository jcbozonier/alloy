using System;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Unite.Messaging;
using Unite.Messaging.Extras;
using Unite.Messaging.Messages;
using Unite.Messaging.Services;
using Unite.Specs.TestObjects;

namespace Unite.Specs.UnitTests
{
    [TestFixture]
    public class UnifiedMessagingController_Tests
    {
        [Test]
        public void When_setting_the_incoming_message_channel()
        {
            var messagingService = new TestUnifiedMessagingService();
            var messagingController = new UnifiedMessagingController(
                                           messagingService, 
                                           new MessageRepository(),
                                           new TestMessageFormatter(), 
                                           new TestFiber());

            messagingController.SendReceivedMessagesTo(new TestMessageChannel());

            Assert.That(messagingService.StartReceiving_WasCalled, "It should start receiving from the messaging plugins.");
        }
    }

    public class TestMessageFormatter : IMessageFormatter
    {
        public string ApplyFormatting(string message)
        {
            throw new NotImplementedException();
        }
    }

    public class TestUnifiedMessagingService : IUnifiedMessagingService
    {
        public bool StartReceiving_WasCalled;

        public void SendMessage(string recipient, string message)
        {
            throw new NotImplementedException();
        }

        public void RequestMessages()
        {
            throw new NotImplementedException();
        }

        public void SetCredentials(Credentials credentials)
        {
            throw new NotImplementedException();
        }

        public event EventHandler<CredentialEventArgs> CredentialsRequested;
        public event EventHandler<CredentialEventArgs> AuthorizationFailed;
        public void StartReceiving()
        {
            StartReceiving_WasCalled = true;
        }

        public event EventHandler<MessagesReceivedEventArgs> MessagesReceived;
    }
}
