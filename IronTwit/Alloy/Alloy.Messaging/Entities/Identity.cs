
using Unite.Messaging.Entities;

namespace Unite.Messaging
{
    public interface IIdentity
    {
        string UserName { get; set; }
        ServiceInformation ServiceInfo { get; set; }
    }

    public class Identity : IIdentity
    {
       public Identity(string userName, ServiceInformation serviceInformation)
        {
            UserName = userName;
            ServiceInfo = serviceInformation;
        }

        public string UserName { get; set; }
        public ServiceInformation ServiceInfo { get; set; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(Identity)) return false;
            return Equals((Identity)obj);
        }

        public bool Equals(Identity obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return Equals(obj.UserName, UserName) && Equals(obj.ServiceInfo, ServiceInfo);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (UserName.GetHashCode() * 397) ^ ServiceInfo.GetHashCode();
            }
        }
    }
}
