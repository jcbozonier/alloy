using System;
using System.Collections.Generic;
using Unite.Messaging.Messages;

namespace Unite.Messaging.Services
{
    public interface IServiceProvider
    {
        IEnumerable<IMessagingService> GetAllServices();
        event EventHandler<CredentialEventArgs> CredentialsRequested;
        event EventHandler<CredentialEventArgs> AuthorizationFailed;
        void ForEachPlugIn(Action<IMessagingService> takeAction);
    }
}