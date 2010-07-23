using System;

namespace Unite.Messaging.Services
{
    public interface IFiber 
    {
        void Run(Action job);
        void RunOnMainThread(Action job);
    }
}