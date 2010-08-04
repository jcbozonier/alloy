using System;
using Unite.Messaging.Services;

namespace Unite.Specs.TestObjects
{
    public class TestFiber : IFiber
    {
        public void Run(Action job)
        {
            job();
        }

        public void RunOnMainThread(Action job)
        {
            job();
        }
    }
}