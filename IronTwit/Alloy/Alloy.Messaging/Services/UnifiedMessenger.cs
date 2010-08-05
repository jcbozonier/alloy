using System;
using System.Collections.Generic;
using Unite.Messaging.Entities;
using Unite.Messaging.Messages;

namespace Unite.Messaging.Services
{
    public class UnifiedMessenger : IUnifiedMessagingService
    {
        private readonly IServiceProvider _Provider;

        /// <summary>
        /// Invoked whenever new messages are received from any one of the
        /// loaded services.
        /// </summary>
        public event EventHandler<MessagesReceivedEventArgs> MessagesReceived;

        public UnifiedMessenger(IServiceProvider provider)
        {
            _Provider = provider;
        }

        /// <summary>
        /// Gets an aggregated list of messages received from all plugins.
        /// </summary>
        /// <returns></returns>
        public void RequestMessages()
        {
            var messages = new List<IMessage>();

            _Provider.ForEachPlugIn(plugIn => messages.AddRange(plugIn.GetMessages()));

            if(MessagesReceived != null)
                MessagesReceived(this, new MessagesReceivedEventArgs(messages));
        }

        /// <summary>
        /// Call this to send the provided message using any and
        /// all services that know how to broadcast to the 
        /// provided address/username.
        /// </summary>
        /// <param name="recipient"></param>
        /// <param name="message"></param>
        public void SendMessage(string recipient, string message)
        {
            _Provider.ForEachPlugIn(messagingService =>
                                        {
                                            messagingService.SendMessage(
                                                new Identity(recipient, messagingService.GetInformation()),
                                                message);
                                        });
        }

        /// <summary>
        /// TODO: Need specs that cover the use of this method.
        /// </summary>
        /// <param name="credentials"></param>
        public void SetCredentials(Credentials credentials)
        {
            _Provider.ForEachPlugIn(plugIn=>plugIn.IfCanAcceptSet(credentials));
        }

        /// <summary>
        /// Call to start up each service.
        /// </summary>
        public void StartReceiving()
        {

            _Provider.ForEachPlugIn(plugIn=>
                                        {
                                            plugIn.MessagesReceived += service_MessagesReceived;
                                            plugIn.StartReceiving();
                                        });
        }

        private void service_MessagesReceived(object sender, MessagesReceivedEventArgs e)
        {
            if (MessagesReceived != null)
                MessagesReceived(sender, e);
        }
    }
}