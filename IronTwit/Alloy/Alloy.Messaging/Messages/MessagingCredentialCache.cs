using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Unite.Messaging.Entities;
using Unite.Messaging.Services;

namespace Unite.Messaging.Messages
{
    public class MessagingCredentialCache : ICredentialsRequestedObserver, ICredentialsProvidedObserver
    {
        private readonly string _cacheFile;
        private readonly Services.IServiceProvider _serviceProvider;
        private ICredentialsRequestedObserver _credentialsRequestedObserver;

        private Dictionary<Guid, CachedCredential> _cache;
        private ICredentialsProvidedObserver _CredentialsProvidedObserver;

        public MessagingCredentialCache(Services.IServiceProvider serviceProvider)
        {
            //_CredentialsObserver
            _serviceProvider = serviceProvider;
            var userAppData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            _cacheFile = Path.Combine(userAppData, "CredentialCache.xml");
            if (!Directory.Exists(userAppData))
                Directory.CreateDirectory(userAppData);
            if (File.Exists(_cacheFile))
                LoadCache();
            else
                InitializeCache();
        }

        private void LoadCache()
        {
            var serializer = new XmlSerializer(typeof (List<CachedCredential>));
            List<CachedCredential> cachedCredentials;
            using (var fs = File.Open(_cacheFile, FileMode.Open))
                cachedCredentials = (List<CachedCredential>) serializer.Deserialize(fs);
            _cache = new Dictionary<Guid, CachedCredential>(cachedCredentials.Count);
            foreach (var cachedCredential in cachedCredentials)
                _cache[cachedCredential.ServiceId] = cachedCredential;
        }

        private void InitializeCache()
        {
            _cache = new Dictionary<Guid, CachedCredential>();
            SaveCache();
        }

        private void SaveCache()
        {
            var cachedCredentials = new List<CachedCredential>(_cache.Values);
            var serializer = new XmlSerializer(typeof (List<CachedCredential>));
            using (var fs = File.Open(_cacheFile, FileMode.Create))
                serializer.Serialize(fs, cachedCredentials);
        }


        private Credentials _Get(IServiceInformation serviceInformation)
        {
            if (!_cache.ContainsKey(serviceInformation.ServiceID))
                return null;
            var cachedCredential = _cache[serviceInformation.ServiceID];
            return new Credentials()
                {
                    ServiceInformation = serviceInformation,
                    UserName = cachedCredential.UserName,
                    Password = cachedCredential.Password,
                    IsPasswordCachingAllowed = cachedCredential.IsPasswordCached
                };
        }

        public bool _Contains(Guid serviceId)
        {
            return _cache.ContainsKey(serviceId);
        }

        private static string GetNormalizedUserName(string rawUserName)
        {
            var normalizedUserName = rawUserName.ToLower();
            return normalizedUserName;
        }

        /// <summary>
        /// Needs to remain public so LoadCache can instantiate these during deserialization and serialization.
        /// </summary>
        public class CachedCredential
        {
            public Guid ServiceId { get; set; }
            public string UserName { get; set; }
            public string Password { get; set; }
            public bool IsPasswordCached { get; set; }
        }

        public void OnCredentialsRequestedNotify(ICredentialsRequestedObserver credentialsRequestedObserver)
        {
            _credentialsRequestedObserver = credentialsRequestedObserver;
        }

        public void OnCredentialsProvidedNotify(ICredentialsProvidedObserver credentialsProvidedObserver)
        {
            _CredentialsProvidedObserver = credentialsProvidedObserver;
        }

        public void CredentialsProvided(Credentials credentials)
        {
            var serviceId = credentials.ServiceInformation.ServiceID;
            var userName = credentials.UserName;
            var normalizedUserName = GetNormalizedUserName(userName);
            var password = credentials.Password;
            if (!_cache.ContainsKey(serviceId))
                _cache[serviceId] = new CachedCredential
                                        {
                                            ServiceId = serviceId,
                                            UserName = normalizedUserName,
                                            Password = password,
                                            IsPasswordCached = credentials.IsPasswordCachingAllowed
                                        };
            SaveCache();
        }

        public void CredentialsRequested(IServiceInformation serviceInformation)
        {
            if (_Contains(serviceInformation.ServiceID))
            {
                var cachedCredential = _Get(serviceInformation);
                _CredentialsProvidedObserver.CredentialsProvided(cachedCredential);
            }
            else
            {
                _credentialsRequestedObserver.CredentialsRequested(serviceInformation);
            }
        }
    }
}
