using System;

namespace Unite.Messaging.Services
{
    public interface IMessagingPlugInRepository
    {
        void Add(Type messagingServiceType);
    }
}