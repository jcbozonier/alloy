using Unite.Messaging.Entities;

namespace Unite.Messaging.Services
{
    public interface IPlugInDetection
    {
        ServiceInformation GetService(string address);
    }
}
