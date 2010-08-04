namespace Unite.Messaging.Entities
{
    public class Credentials
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public IServiceInformation ServiceInformation { get; set; }
        public bool IsPasswordCachingAllowed { get; set; }
    }
}
