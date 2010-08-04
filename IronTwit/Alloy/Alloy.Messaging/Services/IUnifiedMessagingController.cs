using System;

namespace Unite.Messaging.Services
{
    public interface IUnifiedMessagingController
    {
        void MessageToSend(string recipient, string message);
        void RequestMessageUpdate();
    }
}