using System;
using jabber.client;
using jabber.protocol.client;
using Unite.Messaging;
using Unite.Messaging.Entities;
using Unite.Messaging.Messages;

namespace GoogleTalkPlugIn
{
    public class GoogleTalkDataAccess : IGoogleTalkDataAccess
    {
        private readonly JabberClient _Client;
        public bool IsConnected{ get; set;}
        public event EventHandler<ContactEventArgs> OnContactsReceived;

        public GoogleTalkDataAccess()
        {
            var client = new JabberClient();

            client.AutoLogin = true;
            client.AutoPresence = true;
            client.AutoRoster = false;
            client.AutoReconnect = 1;

            client.Server = "gmail.com";
            client.Port = 5222;
            client.Resource = "Alloy";
            client.OnAuthenticate += client_OnAuthenticate; 

            client.OnConnect += (s, e) =>
                                    {
                                        IsConnected = e.Connected;
                                        
                                    };

            client.OnError += (s, e) =>
            {
                throw new Exception(e.Message);
            };
            client.OnAuthError += (s, e) => OnAuthError(s, null);
            client.OnMessage += (s, e) =>
                                    {
                                        OnMessage(s, new GTalkMessageEventArgs(e.From.User, e.Body, DateTime.Now));
                                    };


            client.OnPresence += (sndr, e) =>
                                     {
                                         if (OnContactsReceived != null)
                                             OnContactsReceived(this, new ContactEventArgs()
                                                                          {
                                                                              ReceivedContacts =
                                                                                  new IIdentity[]
                                                                                      {new Identity(e.From.User, null)}
                                                                          });
 
                                     }; 
            _Client = client;


        }

        void client_OnAuthenticate(object sender)
        {
            _Client.GetRoster();
        }

        public event EventHandler OnAuthError;
        public event EventHandler<GTalkMessageEventArgs> OnMessage;

        public void Message(string name, string message)
        {
            if(!_Client.IsAuthenticated && OnAuthError != null)
            {
                OnAuthError(this, EventArgs.Empty);
            }
            else
                _Client.Message(name, message);

        }

        public void SetAvailableMessage(string message)
        {
            _Client.Presence(PresenceType.available, message, message, 1);
        }

        public void Login(string name, string password)
        {
            _Client.User = name;
            _Client.Password = password;
            _Connect();
        }

        private void _Connect()
        {
            _Client.Connect();
        }

        public void Logoff()
        {
            _Client.Close();
        }
    }

    public class GTalkMessageEventArgs : EventArgs
    {
        public GTalkMessageEventArgs(string user, string message, DateTime messageReceivedTimeStamp)
        {
            Message = message;
            User = user;
            MessageReceivedTimeStamp = messageReceivedTimeStamp;
        }

        public string Message;
        public string User;
        public DateTime MessageReceivedTimeStamp;
    }
}
