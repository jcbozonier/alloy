using System;
using NUnit.Framework;
using Unite.Messaging.Extras;
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

            messagingController.StartReceiving();

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
}
