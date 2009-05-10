using System.Collections.Generic;
using Bound.Net;
using Unite.Messaging.Entities;
using Unite.Messaging.Messages;
using Unite.Messaging.Services;
using Unite.UI.Utilities;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Unite.Messaging;

namespace Unite.UI.ViewModels
{
    public interface IInitializeView : IDisposable
    {
        void Init();
    }

    public class MainView : IInitializeView, INotifyPropertyChanged
    {
        
        public MainView(
            IInteractionContext interactionContext, 
            IMessagingServiceManager messagingService, 
            ContactManager contactManager, 
            MessageManager messageManager)
        {
            if (interactionContext == null)
                throw new ArgumentNullException("interactionContext");
            if (messagingService == null)
                throw new ArgumentNullException("messagingService");

            _Interactions = interactionContext;
            _ContactManager = contactManager;
            _MessageManager = messageManager;
            _MessageManager.NewMessagesReceived += (sndr,e)=>_GetMessages();

            _MessagingService = messagingService;
            PropertyChanged += MainView_PropertyChanged;
            _MessagingService.CredentialsRequested += messagingService_CredentialsRequested;
            _MessagingService.AuthorizationFailed += _MessagingService_AuthorizationFailed;

            Messages = new ObservableCollection<IMessage>();

            SendMessage = new SendMessageCommand(
                () =>
                {
                    _MessageManager.MessageToSend(Recipient, MessageToSend);
                    MessageToSend = "";
                });

            ReceiveMessage = new ReceiveMessagesCommand(_MessageManager.RequestMessageUpdate);

        }

        /// <summary>
        /// Any user input the view model needs can be requested through
        /// this object. Instantiation is handled in IoC container.
        /// </summary>
        private readonly IInteractionContext _Interactions;
        private readonly IMessagingServiceManager _MessagingService;
        private readonly ContactManager _ContactManager;
        private readonly MessageManager _MessageManager;

        private Dictionary<ServiceInformation, bool> _RetryOnAuthFailure;
        
        /// <summary>
        /// A list of all of the tweets that should be displayed.
        /// </summary>
        public ObservableCollection<IMessage> Messages { get; set; }

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


        private IEnumerable<IIdentity> _suggestedRecipients;
        public IEnumerable<IIdentity> SuggestedRecipients
        {
            get
            {
                return _suggestedRecipients;
            }
            set
            {
                _suggestedRecipients = value;
                PropertyChanged.Notify(() => SuggestedRecipients);
            }
        }

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

        private void _GetMessages()
        {
            Messages.Clear();
            var messages = _MessageManager.GetAllMessages();

            foreach (var message in messages)
            {
                Messages.Add(message);
            }
        }

        /// <summary>
        /// This must be called when the application first starts so
        /// that the model can go through the appropriate workflow
        /// to set up the UI for the user.
        /// </summary>
        public void Init()
        {
            _MessagingService.StartReceiving();
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
                case "Recipient":
                    SuggestedRecipients = _ContactManager.SearchFor(Recipient);
                    break;
            }
        }

        void messagingService_CredentialsRequested(object sender, CredentialEventArgs e)
        {
            var credentials = _Interactions.GetCredentials(e.ServiceInfo);
            _MessagingService.SetCredentials(credentials);
        }

        void _MessagingService_AuthorizationFailed(object sender, CredentialEventArgs e)
        {
            _RetryOnAuthFailure = _RetryOnAuthFailure ?? new Dictionary<ServiceInformation, bool>();

            if (!_RetryOnAuthFailure.ContainsKey(e.ServiceInfo))
                _RetryOnAuthFailure[e.ServiceInfo] = _Interactions.AuthenticationFailedRetryQuery();

            if (!_RetryOnAuthFailure[e.ServiceInfo]) return;

            messagingService_CredentialsRequested(this, e);
        }

        public void Dispose()
        {
            _MessagingService.StopReceiving();
            PropertyChanged -= MainView_PropertyChanged;
            _MessagingService.CredentialsRequested -= messagingService_CredentialsRequested;
            _MessagingService.AuthorizationFailed -= _MessagingService_AuthorizationFailed;
        }
    }
}
