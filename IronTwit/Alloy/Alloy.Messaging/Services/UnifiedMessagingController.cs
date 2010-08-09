using System;
using System.Collections.Generic;
using Unite.Messaging.Entities;
using Unite.Messaging.Extras;
using Unite.Messaging.Messages;

namespace Unite.Messaging.Services
{
    public class UnifiedMessagingController : IUnifiedMessagingController, IMessageChannel
    {
        private readonly IUnifiedMessagingService _MessagingService;
        private readonly MessageRepository _MessageRepository;
        private readonly IMessageFormatter _MessageFormatter;
        private readonly IFiber _Fiber;
        private IMessageChannel _MessageChannel;

        public UnifiedMessagingController(
            IUnifiedMessagingService messagingService, 
            MessageRepository messageRepository, 
            IMessageFormatter messageFormatter,
            IFiber fiber)
        {
            _MessagingService = messagingService;
            _MessageRepository = messageRepository;
            _MessageFormatter = messageFormatter;
            _Fiber = fiber;
            _MessagingService.MessagesReceived += _MessagingService_MessagesReceived;
        }
        public void OnReceivedMessagesNotify(IMessageChannel messageChannel)
        {
            _MessageChannel = messageChannel;
        }

        void _MessagingService_MessagesReceived(object sender, MessagesReceivedEventArgs e)
        {
            _MessageRepository.AddUniqueMessages(e.Messages);   
        }

        public void MessageToSend(string recipient, string message)
        {
            _Fiber.Run(() =>
                           {
                               var messageToSend = _MessageFormatter.ApplyFormatting(message);
                               _MessagingService.SendMessage(recipient, messageToSend);
                           });
        }

        public void RequestMessageUpdate()
        {
            _Fiber.Run(()=>_MessagingService.RequestMessages());
        }

        public void ReceivedMessages(IEnumerable<IMessage> messages)
        {
            _Fiber.RunOnMainThread(() => _MessageChannel.ReceivedMessages(messages));
        }

        public void StartReceiving()
        {
            _Fiber.Run(_MessagingService.StartReceiving);
        }
    }
}
