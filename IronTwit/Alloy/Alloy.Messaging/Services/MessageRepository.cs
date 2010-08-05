using System;
using System.Collections.Generic;
using System.Linq;
using Unite.Messaging.Entities;
using Unite.Messaging.Messages;

namespace Unite.Messaging.Services
{
    public class MessageRepository
    {
        private readonly List<IMessage> _Messages;
        private IMessageChannel _MessageChannel;

        public MessageRepository()
        {
            _Messages = new List<IMessage>();
        }

        private bool _Contains(IMessage message)
        {
            return _Messages.Exists(x => x.Address.ServiceInfo == message.Address.ServiceInfo &&
                                         message.Text == x.Text &&
                                         message.TimeStamp == x.TimeStamp);
        }

        public void AddUniqueMessages(IEnumerable<IMessage> messages)
        {
            lock (_Messages)
            {
                foreach (var message in messages.Where(message => !_Contains(message)))
                {
                    _Messages.Add(message);
                    _Messages.Sort((x, y) => y.TimeStamp.CompareTo(x.TimeStamp));
                }

                var returnableMessages = new IMessage[_Messages.Count];
                _Messages.CopyTo(returnableMessages);

                _MessageChannel.ReceivedMessages(returnableMessages);
            }
        }

        public void OnAddedMessagesNotify(IMessageChannel messageChannel)
        {
            _MessageChannel = messageChannel;
        }
    }
}
