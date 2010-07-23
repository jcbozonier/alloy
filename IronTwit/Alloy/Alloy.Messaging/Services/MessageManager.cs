using System;
using Unite.Messaging.Extras;
using Unite.Messaging.Messages;

namespace Unite.Messaging.Services
{
    public class UnifiedMessagingController : IUnifiedMessagingController
    {
        private readonly IUnifiedMessagingService _MessagingService;
        private readonly MessageRepository _MessageRepository;
        private readonly IMessageFormatter _MessageFormatter;
        private readonly IFiber _JobRunner;
        private IMessageChannel _MessageChannel;
        private bool _MessagingServiceIsStarted;

        public UnifiedMessagingController(
            IUnifiedMessagingService messagingService, 
            MessageRepository messageRepository, 
            IMessageFormatter messageFormatter,
            IFiber jobRunner)
        {
            _MessagingService = messagingService;
            _MessageRepository = messageRepository;
            _MessageFormatter = messageFormatter;
            _JobRunner = jobRunner;
            _MessagingService.MessagesReceived += _MessagingService_MessagesReceived;
        }
        public void SetMessageChannel(IMessageChannel messageChannel)
        {
            _MessageChannel = messageChannel;
            if (_MessagingServiceIsStarted) return;

            _MessagingServiceIsStarted = true;
            _JobRunner.Run(_MessagingService.StartReceiving);
        }

        void _MessagingService_MessagesReceived(object sender, MessagesReceivedEventArgs e)
        {
            var messages = e.Messages;
            foreach(var message in messages)
            {
                _MessageRepository.UniqueAdd(message);   
            }

            _JobRunner.RunOnMainThread(()=>_MessageChannel.ReceivedMessages(_MessageRepository.GetAll()));
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
            _JobRunner.Run(()=>_MessagingService.RequestMessages());
        }

        public void GetAllMessages()
        {
            
        }

        public event EventHandler NewMessagesReceived;
    }
}
