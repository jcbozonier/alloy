using System;
using Unite.Messaging.Entities;

namespace Unite.Specs.TestObjects
{
    public class Message : IMessage
    {
        public string Text
        {
            get;
            set;
        }

        public IIdentity Address
        {
            get;
            set;
        }

        public DateTime TimeStamp
        {
            get;
            set;
        }
    }
}
