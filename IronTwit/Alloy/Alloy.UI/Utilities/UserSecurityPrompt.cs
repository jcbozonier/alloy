using System.Windows;
using Unite.Messaging.Entities;
using Unite.Messaging.Services;
using Unite.UI.ViewModels;
using Unite.UI.Views;

namespace Unite.UI.Utilities
{
    public class UserSecurityPrompt : ICredentialsRequestedObserver, ICredentialsProvidedObserver
    {
        private readonly IFiber _Fiber;
        private ICredentialsProvidedObserver _CredentialsProvidedObserver;

        private ICredentialRetryObserver _CredentialsRetryObserver;

        public UserSecurityPrompt(IFiber fiber)
        {
            _Fiber = fiber;
        }

        public void RequestCredentialsFor(string messagingServiceName, string suggestedUserName, IServiceInformation serviceInformation)
        {
            var username = "";
            var password = "";
            var savePassword = false;

            _Fiber.RunOnMainThread(() =>
                                       {
                                           var userCredentialsViewModel = new UserCredentialsViewModel()
                                                                              {
                                                                                  Caption = messagingServiceName + " Login",
                                                                                  UserName = suggestedUserName
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

            _CredentialsProvidedObserver.CredentialsProvided(new Credentials()
                                                         {
                                                             UserName = username,
                                                             Password = password,
                                                             ServiceInformation = serviceInformation,
                                                             IsPasswordCachingAllowed = savePassword
                                                         });
        }

        public void AuthenticationFailedRetryQuery(ServiceInformation serviceInfo)
        {
            var result = MessageBox.Show("Username and/or password are not correct. Retry?",
                            "Alloy by Justin Bozonier",
                            MessageBoxButton.YesNo);

            if (result == MessageBoxResult.No)
                _CredentialsRetryObserver.StopRetrying(serviceInfo);
        }

        public void OnCredentialsRetryNotify(ICredentialRetryObserver credentialRetryObserver)
        {
            _CredentialsRetryObserver = credentialRetryObserver;
        }

        public void CredentialsNeeded(IServiceInformation serviceInformation)
        {
            RequestCredentialsFor(serviceInformation.ServiceName, "", serviceInformation);
        }

        public void CredentialsProvided(Credentials credentials)
        {
            _CredentialsProvidedObserver.CredentialsProvided(credentials);
        }

        public void OnCredentialsProvidedNotify(ICredentialsProvidedObserver credentialsProvidedObserver)
        {
            _CredentialsProvidedObserver = credentialsProvidedObserver;
        }
    }
}