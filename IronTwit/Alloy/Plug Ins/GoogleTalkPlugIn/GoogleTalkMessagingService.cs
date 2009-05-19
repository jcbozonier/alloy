using System;
using System.Collections.Generic;
using StructureMap;
using Unite.Messaging;
using Unite.Messaging.Entities;
using Unite.Messaging.Messages;
using Unite.Messaging.Services;

namespace GoogleTalkPlugIn
{
    public class GoogleTalkMessagingService : IMessagingService
    {
        private readonly IGoogleTalkDataAccess _DataAccess;

        private Credentials _Credentials;
        private readonly CredentialEventArgs _CredEventArgs;

        private static readonly ServiceInformation _ServiceInformation = new ServiceInformation()
                                                             {
                                                                 ServiceID = new Guid("{D17E41F2-8D70-49fa-95C7-1DE34CB55DE3}"),
                                                                 ServiceName = "GoogleTalk"
                                                             };

        [DefaultConstructor]
        public GoogleTalkMessagingService()
            : this(new GoogleTalkDataAccess())
        {
        }

        public GoogleTalkMessagingService(IGoogleTalkDataAccess dataAccess)
        {
            _DataAccess = dataAccess;
            _DataAccess.OnMessage += _DataAccess_OnMessage;
            _DataAccess.OnAuthError += _DataAccess_OnAuthError;
            _DataAccess.OnContactsReceived += _DataAccess_OnContactsReceived;

            _CredEventArgs = new CredentialEventArgs()
            {
                ServiceInfo = _ServiceInformation
            };
        }

        private void _DataAccess_OnContactsReceived(object sender, ContactEventArgs e)
        {
            var localizedContacts = new List<IIdentity>();
            foreach(var contact in e.ReceivedContacts)
            {
                localizedContacts.Add(new Identity(contact.UserName, GetInformation()));
            }

            if (ContactsReceived != null)
                ContactsReceived(this, new ContactEventArgs(localizedContacts));
        }


        void _DataAccess_OnAuthError(object sender, EventArgs e)
        {
            if(_Credentials != null)
                _DataAccess.Login(_Credentials.UserName, _Credentials.Password);

            if (AuthorizationFailed != null && _Credentials == null)
                AuthorizationFailed(this, _CredEventArgs);
        }

        void _DataAccess_OnMessage(object sender, GTalkMessageEventArgs e)
        {
            _ReceiveMessage(e.User ?? _Credentials.UserName, e.Message, e.MessageReceivedTimeStamp.ToUniversalTime());
        }

        private void _ReceiveMessage(string username, string message, DateTime messageReceivedTimeStamp)
        {
            var user = new GTalkUser()
                           {
                               ServiceInfo = _ServiceInformation,
                               UserName = username
                           };
            var messageReceived = new GTalkMessage()
                                      {
                                          Address = user,
                                          Text = message,
                                          TimeStamp = messageReceivedTimeStamp
                                      };
            if (MessagesReceived != null)
                MessagesReceived(
                    this,
                    new MessagesReceivedEventArgs(new[] { messageReceived }));
        }

        public bool CanAccept(Credentials credentials)
        {
            return credentials.ServiceInformation.ServiceID.Equals(_ServiceInformation.ServiceID);
        }

        public List<IMessage> GetMessages()
        {
            return new List<IMessage>();
        }

        public void SendMessage(IIdentity recipient, string message)
        {
            _AuthenticateIfNeeded();
            
            if(_Credentials == null)
                throw new Exception("Your credentials can not still be null. This should NEVER happen.");

            if (recipient == null || String.IsNullOrEmpty(recipient.UserName))
                _DataAccess.SetAvailableMessage(message);
            else
            {
                _SendInstantMessage(recipient, message);
                _ReceiveMessage(_Credentials.UserName, message, DateTime.Now.ToUniversalTime());
            }
        }

        private void _SendInstantMessage(IIdentity recipient, string message)
        {
            var realUsername = !recipient.UserName.ToLowerInvariant().EndsWith("@gmail.com")
                                   ? recipient.UserName + "@gmail.com"
                                   : recipient.UserName;

            _DataAccess.Message(realUsername, message);
        }

        public void SetCredentials(Credentials credentials)
        {
            _Credentials = credentials;
        }

        public bool CanFind(string address)
        {
            // If the address contains an ampersand it can't start with it
            // or it just shouldn't have one at all.
            if(String.IsNullOrEmpty(address))
                return true;
            if ((!address.StartsWith("@") && address.Contains("@")) || !address.Contains("@"))
                return true;

            return false;
        }

        public ServiceInformation GetInformation()
        {
            return _ServiceInformation;
        }

        public void StartReceiving()
        {
            _AuthenticateIfNeeded();
        }

        private void _AuthenticateIfNeeded()
        {
            if(!_DataAccess.IsConnected)
            {
                if (CredentialsRequested != null)
                    CredentialsRequested(this, _CredEventArgs);

                if(!String.IsNullOrEmpty(_Credentials.UserName) || _Credentials.Password != null)
                    _DataAccess.Login(_Credentials.UserName, _Credentials.Password);
            }   
        }

        public void StopReceiving()
        {
            _DataAccess.Logoff();
        }

        public event EventHandler<CredentialEventArgs> AuthorizationFailed;
        public event EventHandler<CredentialEventArgs> CredentialsRequested;
        public event EventHandler<MessagesReceivedEventArgs> MessagesReceived;
        public event EventHandler<ContactEventArgs> ContactsReceived;
    }
}
