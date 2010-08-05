using System;
using System.Collections.Generic;
using Unite.Messaging.Messages;

namespace Unite.Messaging.Services
{
    public interface IServiceProvider
    {
        IEnumerable<IMessagingService> GetAllServices();
        void ForEachPlugIn(Action<IMessagingService> takeAction);
    }
}