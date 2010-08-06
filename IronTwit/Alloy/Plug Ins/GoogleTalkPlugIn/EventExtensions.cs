using System;

namespace GoogleTalkPlugIn
{
    public static class EventExtensions
    {
        public static void SafelyInvoke<T>(this EventHandler<T> eventHandler, object sender, T eventArgs)
            where T : EventArgs
        {
            var tempEvent = eventHandler;
            if (tempEvent != null)
                tempEvent(sender, eventArgs);
        }
    }
}