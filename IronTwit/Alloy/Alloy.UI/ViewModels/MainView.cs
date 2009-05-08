using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Data;
using System.Windows.Threading;
using Bound.Net;
using Unite.Messaging.Entities;
using Unite.Messaging.Extras;
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
        private readonly IMessagingServiceManager _MessagingService;
        private readonly Thread _CurrentThread;
        private readonly Dispatcher _CurrentDispatcher;
        private Dictionary<ServiceInformation, bool> _RetryOnAuthFailure;

        protected IContactProvider _ContactRepo;

        /// <summary>
        /// Any user input the view model needs can be requested through
        /// this object. Instantiation is handled in IoC container.
        /// </summary>
        public IInteractionContext Interactions;

        /// <summary>
        /// A list of all of the tweets that should be displayed.
        /// </summary>
        public ObservableCollection<UiMessage> Messages { get; set; }

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

        UiMessage _SelectedMessage;
        public UiMessage SelectedMessage
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
        private IMessageFormatter _MessageFormatter;

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

        public MainView(
            IInteractionContext interactionContext,
            IMessagingServiceManager messagingService, 
            IContactProvider contactRepo,
            IMessageFormatter messageFormatter)
        {
            if(interactionContext == null) 
                throw new ArgumentNullException("interactionContext");
            if(messagingService == null)
                throw new ArgumentNullException("messagingService");

            _CurrentThread = Thread.CurrentThread;
            _CurrentDispatcher = Dispatcher.CurrentDispatcher;

            _ContactRepo = contactRepo;
            _MessagingService = messagingService;
            _MessageFormatter = messageFormatter;
            PropertyChanged += MainView_PropertyChanged;
            _MessagingService.CredentialsRequested += messagingService_CredentialsRequested;
            _MessagingService.AuthorizationFailed += _MessagingService_AuthorizationFailed;
            _MessagingService.MessagesReceived += _MessagingService_MessagesReceived;

            Messages = new ObservableCollection<UiMessage>();

            Interactions = interactionContext;

            SendMessage = new SendMessageCommand(
                () =>
                {
                    var messageToSend = MessageToSend;
                    _SendMessage(messageToSend, Recipient);
                    MessageToSend = "";
                });

            ReceiveMessage = new ReceiveMessagesCommand(_GetMessages);

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

        public void Dispose()
        {
            _MessagingService.StopReceiving();
            PropertyChanged -= MainView_PropertyChanged;
            _MessagingService.CredentialsRequested -= messagingService_CredentialsRequested;
            _MessagingService.AuthorizationFailed -= _MessagingService_AuthorizationFailed;
            _MessagingService.MessagesReceived -= _MessagingService_MessagesReceived;
        }

        void MainView_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {

            switch (e.PropertyName)
            {
                case "SelectedMessage":
                    Recipient = SelectedMessage.Address.UserName;
                    break;
                case "Recipient":
                    SuggestedRecipients = _SuggestRecipients(Recipient);
                    break;
            }
        }

        private IEnumerable<IIdentity> _SuggestRecipients(string recipient)
        {
            return from message in Messages
                    where message.Address.UserName.StartsWith(recipient)
                    select message.Address;
        }

        void messagingService_CredentialsRequested(object sender, CredentialEventArgs e)
        {
            var credentials = Interactions.GetCredentials(e.ServiceInfo);
            _MessagingService.SetCredentials(credentials);
        }

        void _MessagingService_AuthorizationFailed(object sender, CredentialEventArgs e)
        {
            _RetryOnAuthFailure = _RetryOnAuthFailure ?? new Dictionary<ServiceInformation, bool>();

            if (!_RetryOnAuthFailure.ContainsKey(e.ServiceInfo))
                _RetryOnAuthFailure[e.ServiceInfo] = Interactions.AuthenticationFailedRetryQuery();

            if (!_RetryOnAuthFailure[e.ServiceInfo]) return;

            messagingService_CredentialsRequested(this, e);
        }

        void _MessagingService_MessagesReceived(object sender, MessagesReceivedEventArgs e)
        {
            var currentMessages = Messages;
            var newMessages = e.Messages;

            _UpdateMessageRepo(newMessages, currentMessages);
        }

        private void _UpdateMessageRepoWithMessages(IEnumerable<IMessage> newMessages)
        {
            var messageRepo = Messages;
            var result = newMessages;
            var messageList = new List<UiMessage>(messageRepo);
            messageRepo.Clear();

            foreach (var message in result)
            {
                var uiMessage = new UiMessage(message, _ContactRepo.Get(message.Address));
                messageRepo.Add(uiMessage);
            }

            foreach (var message in messageList)
            {
                messageRepo.Add(message);
            }
        }
        

        private void _UpdateMessageRepo(IEnumerable<IMessage> newMessages, ICollection<UiMessage> messageRepo)
        {
            if (_CurrentThread != Thread.CurrentThread)
            {
                _CurrentDispatcher.Invoke(
                    DispatcherPriority.Normal,
                    (Action) (()=>_UpdateMessageRepoWithMessages(newMessages)));
            }
            else
            {
                _UpdateMessageRepoWithMessages(newMessages);
            }
        }

        private void _GetMessages()
        {
            _UpdateMessageRepoWithMessages(_MessagingService.GetMessages());
        }

        private void _SendMessage(string messageToSend, string recipient)
        {
            messageToSend = _MessageFormatter.ApplyFormatting(messageToSend);
            _MessagingService.SendMessage(recipient, messageToSend);
        }
    }
}
