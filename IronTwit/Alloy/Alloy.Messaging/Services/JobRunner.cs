using System;
using System.Threading;
using System.Windows.Threading;

namespace Unite.Messaging.Services
{
    public interface IJobRunner 
    {
        void Run(Action job);
        void RunOnMainThread(Action job);
    }

    public class AsyncJobRunner : IJobRunner
    {
        private Dispatcher _MainDispatcher;

        public AsyncJobRunner(Dispatcher mainDispatcher)
        {
            _MainDispatcher = mainDispatcher;
        }

        public void RunOnMainThread(Action job)
        {
            if (_MainDispatcher.Thread != Thread.CurrentThread)
                _MainDispatcher.Invoke(job, DispatcherPriority.Normal, null);
            else
                job();
        }

        public void Run(Action job)
        {
            new Thread(()=>job()).Start();
        }
    }

    public class SynchronousJobRunner : IJobRunner
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
