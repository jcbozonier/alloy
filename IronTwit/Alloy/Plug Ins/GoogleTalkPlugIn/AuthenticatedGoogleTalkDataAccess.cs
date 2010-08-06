using System;
using System.Collections.Generic;
using Unite.Messaging.Messages;

namespace GoogleTalkPlugIn
{
    public class AuthenticatedGoogleTalkDataAccess : IGoogleTalkDataAccess
    {
        private readonly Queue<Action> _ActionQueue;
        private bool _WasAuthenticated;
        private readonly IGoogleTalkDataAccess _GoogleTalkDataAccess;

        public AuthenticatedGoogleTalkDataAccess(IGoogleTalkDataAccess googleTalkDataAccess)
        {
            _ActionQueue = new Queue<Action>();
            _GoogleTalkDataAccess = googleTalkDataAccess;
            _GoogleTalkDataAccess.OnAuthError += OnAuthError_FromDownBelow;
            _GoogleTalkDataAccess.OnMessage += OnMessage_FromDownBelow;
            _GoogleTalkDataAccess.OnContactsReceived += OnContactsReceived_FromDownBelow;
            _GoogleTalkDataAccess.OnAuthenticated += _OnAuthenticated_FromDownBelow;
        }

        public event EventHandler<EventArgs> OnAuthenticated;
        public event EventHandler<EventArgs> OnAuthError;
        public event EventHandler<GTalkMessageEventArgs> OnMessage;
        public event EventHandler<ContactEventArgs> OnContactsReceived;

        public void Message(string name, string message)
        {
            DoAsSoonAsAuthenticated(() => _GoogleTalkDataAccess.Message(name, message));
        }

        private void DoAsSoonAsAuthenticated(Action action)
        {
            if (_WasAuthenticated)
                action();
            else
                _ActionQueue.Enqueue(action);
        }

        public void Login(string name, string password)
        {
            _GoogleTalkDataAccess.Login(name, password);
        }

        public void Logoff()
        {
            DoAsSoonAsAuthenticated(()=>_GoogleTalkDataAccess.Logoff());
        }

        public void SetAvailableMessage(string message)
        {
            DoAsSoonAsAuthenticated(()=>_GoogleTalkDataAccess.SetAvailableMessage(message));
        }

        public bool IsConnected
        {
            get { return _GoogleTalkDataAccess.IsConnected; }
            set { _GoogleTalkDataAccess.IsConnected = value; }
        }

        private void _OnAuthenticated_FromDownBelow(object sender, EventArgs e)
        {
            _OnAuthenticated();

            OnAuthenticated.SafelyInvoke(sender, e);
        }

        private void OnContactsReceived_FromDownBelow(object sender, ContactEventArgs e)
        {
            OnContactsReceived.SafelyInvoke(sender, e);
        }

        private void OnMessage_FromDownBelow(object sender, GTalkMessageEventArgs e)
        {
            OnMessage.SafelyInvoke(sender, e);
        }

        private void OnAuthError_FromDownBelow(object sender, EventArgs e)
        {
            OnAuthError.SafelyInvoke(sender, e);
        }

        private void _OnAuthenticated()
        {
            while (_ActionQueue.Count > 0)
                _ActionQueue.Dequeue()();

            _WasAuthenticated = true;
        }
    }
}