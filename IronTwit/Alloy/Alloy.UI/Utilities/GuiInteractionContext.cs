using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using StructureMap;
using Unite.Messaging.Entities;
using Unite.Messaging.Messages;
using Unite.UI.ViewModels;
using Unite.UI.Views;
using Unite.Messaging;

namespace Unite.UI.Utilities
{
    public class GuiInteractionContext : IInteractionContext
    {
        private Thread _mainThread;

        public GuiInteractionContext(Thread mainThread)
        {
            _mainThread = mainThread;
        }

        public Credentials GetCredentials(IServiceInformation serviceInformation)
        {
            var credentialCache = ObjectFactory.GetInstance<ICredentialCache>();
            Credentials cachedCredential = null;
            if (credentialCache.Contains(serviceInformation.ServiceID))
            {
                cachedCredential = credentialCache.Get(serviceInformation.ServiceID);
                if (cachedCredential.IsPasswordCachingAllowed) //else we will prompt for password
                    return cachedCredential;
            }

            var username = "";
            var password = "";
            var savePassword = false;

            var dispatcher = Dispatcher.FromThread(_mainThread);
            dispatcher.Invoke(DispatcherPriority.Normal,
                              (Action) (() =>
                                            {
                                                var model = new UserCredentialsViewModel()
                                                                {
                                                                    Caption = serviceInformation.ServiceName + " Login",
                                                                    UserName = (cachedCredential != null) ? cachedCredential.UserName : string.Empty
                                                                };
                                                var dialog = new UserCredentialsWindow
                                                                 {
                                                                     DataContext = model
                                                                 };
                                                var mainWindow = Application.Current.MainWindow;
                                                dialog.Owner = mainWindow;
                                                dialog.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                                                dialog.ShowDialog();
                                                username = model.UserName;
                                                password = model.Password;
                                                savePassword = model.SavePassword;
                                            }));

            var credentials = new Credentials()
                       {
                           UserName = username,
                           Password = password,
                           ServiceInformation = serviceInformation,
                           IsPasswordCachingAllowed = savePassword
                       };
            credentialCache.Add(credentials);
            return credentials;
        }

        public bool AuthenticationFailedRetryQuery()
        {
            var result = MessageBox.Show("Username and/or password are not correct. Retry?",
                            "Alloy by Justin Bozonier",
                            MessageBoxButton.YesNo);
            return result == MessageBoxResult.Yes;
        }
    }
}
