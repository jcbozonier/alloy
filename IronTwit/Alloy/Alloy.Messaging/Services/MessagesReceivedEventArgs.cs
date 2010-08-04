﻿using System;
using System.Collections.Generic;
using Unite.Messaging.Entities;

namespace Unite.Messaging.Services
{
    public class MessagesReceivedEventArgs : EventArgs
    {
        public MessagesReceivedEventArgs(IEnumerable<IMessage> messages)
        {
            Messages = messages;
        }

        public IEnumerable<IMessage> Messages
        {
            get; set;
        }
    }
}
