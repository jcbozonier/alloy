using System.Collections.Generic;
using Bound.Net;
using Unite.Messaging.Entities;
using Unite.Messaging.Messages;
using Unite.Messaging.Services;
using Unite.UI.Utilities;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Unite.UI.ViewModels
{
    public class MessagingViewModel : INotifyPropertyChanged, IMessageChannel
    {
        public MessagingViewModel(IUnifiedMessagingController messagingController)
        {
            if(messagingController == null)
                throw new ArgumentNullException("messagingController");

            _MessagingController = messagingController;

            PropertyChanged += MainView_PropertyChanged;
            
            Messages = new ObservableCollection<IMessage>();

            SendMessage = new SendMessageCommand(
                () =>
                {
                    _MessagingController.MessageToSend(Recipient, MessageToSend);
                    MessageToSend = "";
                });

            ReceiveMessage = new ReceiveMessagesCommand(_MessagingController.RequestMessageUpdate);
        }

        private readonly IUnifiedMessagingController _MessagingController;

        /// <summary>
        /// A list of all of the tweets that should be displayed.
        /// </summary>
        public IEnumerable<IMessage> Messages
        {
            get
            {
                return _Messages;
            }
            set
            {
                _Messages = value;
                PropertyChanged.Notify(() => Messages);
            }
        }

        private string _messageToSend;
        /// <summary>
        /// The message the user wants to send.
        /// </summary>
        public string MessageToSend
        {
            get
            {
                return _messageToSend;
            }
            set
            {
                _messageToSend = value;
                PropertyChanged.Notify(()=>MessageToSend);
            }
        }

        private string _Recipient;
        /// <summary>
        /// The recipient the user wants to send MessageToSend to.
        /// </summary>
        public string Recipient
        {
            get
            {
                return _Recipient;
            }
            set
            {
                _Recipient = value;
                PropertyChanged.Notify(()=>Recipient);
            }
        }

        public string Title { get { return "Alloy Messaging"; } }

        IMessage _SelectedMessage;
        public IMessage SelectedMessage
        {
            get
            {
                return _SelectedMessage;
            }
            set
            {
                _SelectedMessage = value;
                PropertyChanged.Notify(()=>SelectedMessage);
            }
        }


        private IEnumerable<IMessage> _Messages;

        /// <summary>
        /// Command object invoked by the InteractionContext (GUI) to send
        /// a message.
        /// </summary>
        public SendMessageCommand SendMessage { get; set; }

        /// <summary>
        /// Command object invoked by the InteractionContext (GUI) to
        /// receive a message.
        /// </summary>
        public ReceiveMessagesCommand ReceiveMessage { get; set; }

        public void ReceivedMessages(IEnumerable<IMessage> messages)
        {
            Messages = messages;
        }

        /// <summary>
        /// Gets called whenever a property is changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        void MainView_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {

            switch (e.PropertyName)
            {
                case "SelectedMessage":
                    Recipient = SelectedMessage.Address.UserName;
                    break;
            }
        }

        

        public void Dispose()
        {
            PropertyChanged -= MainView_PropertyChanged;
        }
    }
}
