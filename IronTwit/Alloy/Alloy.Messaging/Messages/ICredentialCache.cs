using System;

namespace Unite.Messaging.Messages
{
    public interface ICredentialCache
    {
        void Add(Credentials credentials);
        Credentials Get(Guid serviceId);
        void Remove(Guid serviceId);
        bool Contains(Guid serviceId);
        void Clear();
    }
}