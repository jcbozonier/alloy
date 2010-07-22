using System.Collections.Generic;

namespace Unite.Messaging.Services
{
    public interface IContactQuery
    {
        IEnumerable<IIdentity> SearchFor(string name);
    }

    public class ContactQuery : IContactQuery
    {
        private readonly ContactRepository _ContactRepository;
        private readonly IUnifiedMessagingService _ServicesManager;

        public ContactQuery(IUnifiedMessagingService servicesManager, ContactRepository contactRepository)
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
