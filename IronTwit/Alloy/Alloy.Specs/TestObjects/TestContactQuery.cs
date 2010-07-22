using System.Collections.Generic;
using System.Linq;
using Unite.Messaging;
using Unite.Messaging.Services;

namespace Unite.Specs.TestObjects
{
    public class TestContactQuery : IContactQuery
    {
        public IEnumerable<IIdentity> SearchFor(string name)
        {
            return Enumerable.Empty<IIdentity>();
        }
    }
}