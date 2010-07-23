using System.Collections.Generic;
using Unite.Messaging.Entities;

namespace Unite.Messaging.Services
{
    public class MessageRepository
    {
        private readonly List<IMessage> _Messages;

        public MessageRepository()
        {
            _Messages = new List<IMessage>();
        }

        public void UniqueAdd(IMessage message)
        {
            lock(_Messages)
            {
                if (!_Contains(message))
                {
                    _Messages.Add(message);
                    _Messages.Sort((x, y) => y.TimeStamp.CompareTo(x.TimeStamp));
                }
            }
        }

        public IEnumerable<IMessage> GetAll()
        {
            var returnableMessages = new IMessage[_Messages.Count];
            _Messages.CopyTo(returnableMessages);
            return returnableMessages;
        }

        private bool _Contains(IMessage message)
        {
            return _Messages.Exists(x => x.Address.ServiceInfo == message.Address.ServiceInfo &&
                                         message.Text == x.Text &&
                                         message.TimeStamp == x.TimeStamp);
        }
    }
}
