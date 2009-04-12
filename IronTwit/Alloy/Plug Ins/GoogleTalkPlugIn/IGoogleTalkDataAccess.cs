using System;

namespace GoogleTalkPlugIn
{
    public interface IGoogleTalkDataAccess
    {
        event EventHandler OnAuthError;
        event EventHandler<GTalkMessageEventArgs> OnMessage;
        void Message(string name, string message);
        void Login(string name, string password);
        void Logoff();
        void SetAvailableMessage(string message);
        bool IsConnected { get; set; }
    }
}