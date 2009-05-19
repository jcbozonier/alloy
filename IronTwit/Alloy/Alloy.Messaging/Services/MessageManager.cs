using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Threading;
using Unite.Messaging.Entities;
using Unite.Messaging.Extras;

namespace Unite.Messaging.Services
{
    public class MessageManager
    {
        private readonly IMessagingServiceManager _MessagingService;
        private readonly MessageRepository _MessageRepository;
        private readonly IMessageFormatter _MessageFormatter;
        private readonly IJobRunner _JobRunner;
        private bool _MessagingServiceIsStarted;

        public MessageManager(
            IMessagingServiceManager messagingService, 
            MessageRepository messageRepository, 
            IMessageFormatter messageFormatter,
            IJobRunner jobRunner)
        {
            _MessagingService = messagingService;
            _MessageRepository = messageRepository;
            _MessageFormatter = messageFormatter;
            _JobRunner = jobRunner;
            _MessagingService.MessagesReceived += _MessagingService_MessagesReceived;
        }

        void _MessagingService_MessagesReceived(object sender, MessagesReceivedEventArgs e)
        {
            var messages = e.Messages;
            foreach(var message in messages)
            {
                if(!_MessageRepository.Contains(message))
                {
                    _MessageRepository.Add(message);
                }
            }

            _JobRunner.RunOnMainThread(_NotifyMessagesReceived);
        }

        private void _NotifyMessagesReceived()
        {
            var messagesReceivedHandler = NewMessagesReceived;
            if (messagesReceivedHandler != null)
                messagesReceivedHandler(this, EventArgs.Empty);
        }

        public void MessageToSend(string recipient, string message)
        {
            _JobRunner.Run(() =>
                           {
                               var messageToSend = _MessageFormatter.ApplyFormatting(message);
                               _MessagingService.SendMessage(recipient, messageToSend);
                           });
        }

        public void RequestMessageUpdate()
        {
            _JobRunner.Run(()=>_MessagingService.GetMessages());
        }

        public IEnumerable<IMessage> GetAllMessages()
        {
            if(!_MessagingServiceIsStarted)
            {
                _MessagingServiceIsStarted = true;
                _JobRunner.Run(_MessagingService.StartReceiving);
            }

            return _MessageRepository.GetAll();
        }

        public event EventHandler NewMessagesReceived;
    }
}
