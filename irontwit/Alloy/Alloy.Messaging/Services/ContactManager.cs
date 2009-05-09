using System.Collections.Generic;

namespace Unite.Messaging.Services
{
    public class ContactManager
    {
        private readonly ContactRepository _ContactRepository;
        private readonly IMessagingServiceManager _ServicesManager;

        public ContactManager(IMessagingServiceManager servicesManager, ContactRepository contactRepository)
        {
            _ServicesManager = servicesManager;
            _ContactRepository = contactRepository;

            _ServicesManager.OnContactsReceived += (sndr, e) => _ContactRepository.Add(e.ReceivedContacts);
        }

        public IEnumerable<IIdentity> SearchFor(string name)
        {
            if(_ContactRepository.IsEmpty)
                return new IIdentity[0];
            return _ContactRepository.SearchFor(name);
        }
    }
}
