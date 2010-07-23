﻿using System;
using System.Collections.Generic;
using System.Linq;
using Unite.Messaging.Entities;
using Unite.Messaging.Messages;
using Unite.Messaging.Services;

namespace Unite.Specs.TestObjects
{
    public class TestMessagingController : IUnifiedMessagingController
    {
        public int GetAllMessagesCalledCount;
        public string SentMessage;
        public string MessageRecipient;
        public IEnumerable<IMessage> MessagesReceived = Enumerable.Empty<IMessage>();

        public void MessageToSend(string recipient, string message)
        {
            SentMessage = message;
            MessageRecipient = recipient;
        }

        public void RequestMessageUpdate()
        {

        }

        public void GetAllMessages()
        {
            GetAllMessagesCalledCount++;
        }

        public event EventHandler NewMessagesReceived;
        public void SetMessageChannel(IMessageChannel messageChannel)
        {
            
        }

        public void NewMessagesReceived_Occurred()
        {
            NewMessagesReceived(null, EventArgs.Empty);
        }
    }
}