namespace Unite.Messaging.Extras
{
    public interface IMessageFormatter
    {
        void MessageToSend(string recipient, string message);
    }
}