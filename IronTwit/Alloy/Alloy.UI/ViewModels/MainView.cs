using System.Collections.Generic;
using Bound.Net;
using Unite.Messaging.Entities;
using Unite.Messaging.Messages;
using Unite.Messaging.Services;
using unite.ui.utilities;
using Unite.UI.Utilities;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Unite.Messaging;

namespace Unite.UI.ViewModels
{
    public class MainView : INotifyPropertyChanged
    {
        
        public MainView(
            IInteractionContext interactionContext, 
            CredentialManager credentialManager,
            ContactManager contactManager, 
            MessageManager messageManager)
        {
            if (interactionContext == null)
                throw new ArgumentNullException("interactionContext");
            if(credentialManager == null)
                throw new ArgumentNullException("credentialManager");
            if(contactManager == null)
                throw new ArgumentNullException("contactManager");
            if(messageManager == null)
                throw new ArgumentNullException("messageManager");

            _CredentialManager = credentialManager;
            _Interactions = interactionContext;
            _ContactManager = contactManager;
            _MessageManager = messageManager;

            _MessageManager.NewMessagesReceived += _MessageManager_NewMessagesReceived; 
            _CredentialManager.CredentialsRequested += messagingService_CredentialsRequested;
            _CredentialManager.AuthorizationFailed += _MessagingService_AuthorizationFailed;

            PropertyChanged += MainView_PropertyChanged;
            
            Messages = new ObservableCollection<IMessage>();

            SendMessage = new SendMessageCommand(
                () =>
                {
                    _MessageManager.MessageToSend(Recipient, MessageToSend);
                    MessageToSend = "";
                });

            ReceiveMessage = new ReceiveMessagesCommand(_MessageManager.RequestMessageUpdate);

            _GetMessages();

        }

        void _MessageManager_NewMessagesReceived(object sender, EventArgs e)
        {
            _GetMessages();
        }

        /// <summary>
        /// Any user input the view model needs can be requested through
        /// this object. Instantiation is handled in IoC container.
        /// </summary>
        private readonly IInteractionContext _Interactions;
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
        private CredentialManager _CredentialManager;

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
            var messages = _MessageManager.GetAllMessages();

            // There is some sort of a race condition here.
            // if you clear messages first this may cause duplicates.
            Messages.Clear();
            foreach (var message in messages)
            {
                Messages.Add(message);
            }
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
            _CredentialManager.SetCredentials(credentials);
        }

        void _MessagingService_AuthorizationFailed(object sender, CredentialEventArgs e)
        {
            // This needs to be refactored out of the view model for sure and placed
            // into the credential manager.
            _RetryOnAuthFailure = _RetryOnAuthFailure ?? new Dictionary<ServiceInformation, bool>();

            if (!_RetryOnAuthFailure.ContainsKey(e.ServiceInfo))
                _RetryOnAuthFailure[e.ServiceInfo] = _Interactions.AuthenticationFailedRetryQuery();

            if (!_RetryOnAuthFailure[e.ServiceInfo]) return;

            messagingService_CredentialsRequested(this, e);
        }

        public void Dispose()
        {
            PropertyChanged -= MainView_PropertyChanged;
            _MessageManager.NewMessagesReceived -= _MessageManager_NewMessagesReceived;
            _CredentialManager.CredentialsRequested -= messagingService_CredentialsRequested;
            _CredentialManager.AuthorizationFailed -= _MessagingService_AuthorizationFailed;
        }
    }
}
