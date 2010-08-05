using System.Windows;
using Unite.Messaging.Entities;
using Unite.Messaging.Messages;
using Unite.Messaging.Prompts;
using Unite.Messaging.Services;
using unite.ui.utilities;
using Unite.UI.ViewModels;
using Unite.UI.Views;

namespace Unite.UI.Utilities
{
    public class SecurityDialogService : IUser
    {
        private readonly ICredentialCache _CredentialCache;
        private readonly IFiber _JobRunner;
        private ICredentialsObserver _CredentialsObserver;

        public SecurityDialogService(ICredentialCache credentialCache, IFiber jobRunner)
        {
            _CredentialCache = credentialCache;
            _JobRunner = jobRunner;
        }

        public void OnNewCredentials(ICredentialsObserver credentialsObserver)
        {
            _CredentialsObserver = credentialsObserver;
        }

        public void PromptForCredentials(IServiceInformation serviceInformation)
        {
            var credentialCache = _CredentialCache;
            Credentials cachedCredential = null;
            if (credentialCache.Contains(serviceInformation.ServiceID))
            {
                cachedCredential = credentialCache.Get(serviceInformation.ServiceID);
                if (cachedCredential.IsPasswordCachingAllowed) //else we will prompt for password
                {
                    _CredentialsObserver.SetCredentials(cachedCredential);
                    return;
                }
            }

            var username = "";
            var password = "";
            var savePassword = false;

            _JobRunner.RunOnMainThread(() =>
                                            {
                                                var userCredentialsViewModel = new UserCredentialsViewModel()
                                                                {
                                                                    Caption = serviceInformation.ServiceName + " Login",
                                                                    UserName = (cachedCredential != null) ? cachedCredential.UserName : string.Empty
                                                                };
                                                var dialog = new UserCredentialsWindow
                                                                 {
                                                                     DataContext = userCredentialsViewModel
                                                                 };
                                                var mainWindow = Application.Current.MainWindow;
                                                dialog.Owner = mainWindow;
                                                dialog.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                                                dialog.ShowDialog();
                                                username = userCredentialsViewModel.UserName;
                                                password = userCredentialsViewModel.Password;
                                                savePassword = userCredentialsViewModel.SavePassword;
                                            });

            var credentials = new Credentials()
                       {
                           UserName = username,
                           Password = password,
                           ServiceInformation = serviceInformation,
                           IsPasswordCachingAllowed = savePassword
                       };

            credentialCache.Add(credentials);

            _CredentialsObserver.SetCredentials(credentials);
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
