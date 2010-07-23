﻿using System;
using System.Collections.Generic;
using Unite.Messaging.Entities;
using Unite.Messaging.Messages;

namespace Unite.Messaging.Services
{
    public interface IServiceProvider
    {
        void Add(params IMessagingService[] services);
        IEnumerable<IMessagingService> GetAllServices();
        event EventHandler<CredentialEventArgs> CredentialsRequested;
        event EventHandler<CredentialEventArgs> AuthorizationFailed;
    }
}