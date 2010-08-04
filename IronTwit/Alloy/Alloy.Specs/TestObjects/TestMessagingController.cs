using System;
using Unite.Messaging.Messages;
using Unite.Messaging.Services;

namespace Unite.Specs.TestObjects
{
    public class TestMessagingController : IUnifiedMessagingController
    {
        public string SentMessage;
        public string MessageRecipient;

        public void MessageToSend(string recipient, string message)
        {
            SentMessage = message;
            MessageRecipient = recipient;
        }

        public void RequestMessageUpdate()
        {

        }
    }
}