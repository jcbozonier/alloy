using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoogleTalkPlugIn;
using Unite.Messaging;

namespace GoogleTalkPlugin.Specs.FakeSpecObjects
{
    public class FakeDataAccess : IGoogleTalkDataAccess
    {
        public event EventHandler OnAuthError;
        public event EventHandler<GTalkMessageEventArgs> OnMessage;

        public FakeDataAccess()
        {
            IsConnected = true;
        }

        public void Message(string name, string message)
        {
        }

        public void Pretend_that_a_message_was_received(string user, string message, DateTime messageReceivedTimeStamp)
        {
            OnMessage(this, new GTalkMessageEventArgs(user, message, messageReceivedTimeStamp));
        }

        public void Login(string name, string password)
        {

        }

        public void Logoff()
        {
        }

        public void SetAvailableMessage(string message)
        {
            throw new System.NotImplementedException();
        }

        public bool IsConnected
        {
            get;
            set;
        }

        public event EventHandler<ContactEventArgs> OnContactsReceived;
    }
}
