using System;
using System.Collections.Generic;
using GoogleTalkPlugIn;
using Unite.Messaging.Messages;

namespace GoogleTalkPlugin.Specs
{
    public class TestGoogleTalkDataAccess : IGoogleTalkDataAccess
    {
        public List<KeyValuePair<string, string>> SentMessages = new List<KeyValuePair<string, string>>();
        public event EventHandler<EventArgs> OnAuthenticated;
        public event EventHandler<ContactEventArgs> OnContactsReceived;
        public event EventHandler<EventArgs> OnAuthError;
        public event EventHandler<GTalkMessageEventArgs> OnMessage;

        public void Message(string name, string message)
        {
            SentMessages.Add(new KeyValuePair<string, string>(name, message));
        }

        public void Login(string name, string password)
        {
            OnAuthenticated.SafelyInvoke(this, EventArgs.Empty);
        }

        public void Logoff()
        {
            
        }

        public void SetAvailableMessage(string message)
        {
            
        }

        public bool IsConnected { get; set; }
    }
}