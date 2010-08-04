using Unite.Messaging.Entities;

namespace Unite.Specs.FakeSpecObjects
{
    public class FakeUser : IIdentity
    {
        public string UserName
        {
            get;
            set;
        }

        public ServiceInformation ServiceInfo
        {
            get;
            set;
        }
    }
}
