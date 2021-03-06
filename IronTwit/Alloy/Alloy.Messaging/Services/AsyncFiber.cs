﻿using System;
using System.Threading;
using System.Windows.Threading;

namespace Unite.Messaging.Services
{
    public class AsyncFiber : IFiber
    {
        private readonly Dispatcher _MainDispatcher;

        public AsyncFiber(Dispatcher mainDispatcher)
        {
            _MainDispatcher = mainDispatcher;
        }

        public void RunOnMainThread(Action job)
        {
            if (_MainDispatcher.Thread != Thread.CurrentThread)
                _MainDispatcher.Invoke(DispatcherPriority.Normal, job);
            else
                job();
        }

        public void Run(Action job)
        {
            new Thread(()=>job()).Start();
        }
    }
}
