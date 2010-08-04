namespace Unite.Messaging.Entities
{
    public interface IIdentity
    {
        string UserName { get; set; }
        ServiceInformation ServiceInfo { get; set; }
    }
}