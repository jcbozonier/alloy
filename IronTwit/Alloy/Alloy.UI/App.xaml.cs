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
            var messagingPlugInRepository = new MessagingPlugInRepository();
            var messagingPluginFinder = new MessagingPluginFinder(messagingPlugInRepository);
            var unifiedMessenger = new UnifiedMessenger(messagingPlugInRepository);

            var messagingFiber = new AsyncFiber(Dispatcher);
            var credentialRepository = new MessagingAccountCredentialRepository(messagingPlugInRepository);
            var securityDialogService = new SecurityDialogService(credentialRepository, messagingFiber);

            var credentialAuthorizationController = new CredentialAuthorizationController(unifiedMessenger, securityDialogService);
            securityDialogService.OnNewCredentials(credentialAuthorizationController);

            var codePasteToUrlService = new CodePasteToUrlService();
            var automaticMessageFormatting = new AutoFormatCodePastesAsUrls(codePasteToUrlService);

            var messageRepository = new MessageRepository();
            var unifiedMessagingController = new UnifiedMessagingController(unifiedMessenger, messageRepository, automaticMessageFormatting, messagingFiber);

            var messagingViewModel = new MessagingViewModel(unifiedMessagingController);
            var messagingWindow = new MessagingWindow(messagingViewModel);

            messageRepository.SendAddedMessagesTo(unifiedMessagingController);
            unifiedMessagingController.SendReceivedMessagesTo(messagingViewModel);
            messagingPluginFinder.GetAllPlugins();

            messagingViewModel.ReceiveMessage.Execute(null);
            messagingWindow.Show();
        }
    }
}
