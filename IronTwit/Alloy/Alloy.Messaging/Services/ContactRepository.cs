using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Unite.Messaging.Services
{
    public class ContactRepository
    {
        private List<IIdentity> _Contacts;

        public ContactRepository()
        {
            _Contacts = new List<IIdentity>();
        }

        public void Add(IEnumerable<IIdentity> contacts)
        {
            _Contacts.AddRange(contacts);
        }

        public bool IsEmpty { get; private set; }

        public IEnumerable<IIdentity> SearchFor(string name)
        {
            return (from contact in _Contacts
                   where contact.UserName.StartsWith(name)
                   select contact).ToArray();
        }
    }
}
