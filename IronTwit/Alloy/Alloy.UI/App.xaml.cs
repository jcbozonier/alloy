using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using unite.ui.utilities;
using Unite.UI.ViewModels;
using StructureMap;
using System.Threading;
using System.Windows.Threading;
using StructureMap.Attributes;
using Unite.Messaging.Extras;
using Unite.Messaging.Messages;
using Unite.Messaging.Services;
using Unite.UI.Utilities;
using StructureMap;
using Unite.Messaging;
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

            var credentialManager = new CredentialAuthorizationController(unifiedMessenger, securityDialogService);

            var codePasteToUrlService = new CodePasteToUrlService();
            var automaticMessageFormatting = new AutoFormatCodePastesAsUrls(codePasteToUrlService);

            var messageRepository = new MessageRepository();
            var unifiedMessagingController = new UnifiedMessagingController(unifiedMessenger, messageRepository, automaticMessageFormatting, messagingFiber);

            var messagingViewModel = new MessagingViewModel(unifiedMessagingController);
            var messagingWindow = new MessagingWindow(messagingViewModel);
           
            messagingWindow.Show();
        }
    }
}
