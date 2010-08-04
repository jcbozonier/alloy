using System.Windows;
using unite.ui.utilities;
using Unite.UI.ViewModels;
using Unite.Messaging.Extras;
using Unite.Messaging.Messages;
using Unite.Messaging.Services;
using Unite.UI.Utilities;
using Unite.UI.Views;

namespace Unite.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            Startup += App_Startup;
        }

        void App_Startup(object sender, StartupEventArgs e)
        {
            var messagingPluginFinder = new MessagingPluginFinder();
            var messagingPlugInRepository = new MessagingPlugInRepository(messagingPluginFinder);
            var unifiedMessenger = new UnifiedMessenger(messagingPlugInRepository);

            var messagingFiber = new AsyncFiber(this.Dispatcher);
            var credentialRepository = new MessagingAccountCredentialRepository(messagingPlugInRepository);
            var securityDialogService = new SecurityDialogService(credentialRepository, messagingFiber);

            new CredentialAuthorizationController(unifiedMessenger, securityDialogService);

            var codePasteToUrlService = new CodePasteToUrlService();
            var automaticMessageFormatting = new AutoFormatCodePastesAsUrls(codePasteToUrlService);

            var messageRepository = new MessageRepository();
            var unifiedMessagingController = new UnifiedMessagingController(unifiedMessenger, messageRepository, automaticMessageFormatting, messagingFiber);
            messageRepository.SendAddedMessagesTo(unifiedMessagingController);

            var messagingViewModel = new MessagingViewModel(unifiedMessagingController);
            unifiedMessagingController.SendReceivedMessagesTo(messagingViewModel);

            var messagingWindow = new MessagingWindow(messagingViewModel);
           
            messagingViewModel.ReceiveMessage.Execute(null);
            messagingWindow.Show();
        }
    }
}
