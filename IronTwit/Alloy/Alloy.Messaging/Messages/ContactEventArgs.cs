using System;
using System.Collections.Generic;

namespace Unite.Messaging.Messages
{
    public class ContactEventArgs : EventArgs
    {
        public IEnumerable<IIdentity> ReceivedContacts;

        public ContactEventArgs()
        {
        }

        public ContactEventArgs(IEnumerable<IIdentity> contacts)
        {
            ReceivedContacts = contacts;
        }
    }
}
