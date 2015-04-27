using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using OpenPKW_Mobile.Services;
using OpenPKW_Mobile.Entities;

namespace OpenPKW_Mobile
{
    public partial class LoginPage : PhoneApplicationPage
    {
        protected string Message 
        { 
            get
            {
                return labelMessage.Text;
            }
            set
            {
                if (value == null)
                {
                    panelWarning.Visibility = Visibility.Collapsed;
                }
                else
                {
                    labelMessage.Text = value;
                    panelWarning.Visibility = Visibility.Visible;
                }
            }
        }

        private bool _locked = false;
        protected bool Locked 
        {
            get
            {
                return this._locked;
            }
            set
            {
                layoutWait.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
                this._locked = value;
            }
        }

        public LoginPage()
        {
            InitializeComponent();

            // ukrycie elementów, które nie powinny być początkowo widoczne,
            // a zostały dodane do formatki w celach projektowych
            panelWarning.Visibility = Visibility.Collapsed;

#if !DEBUG
            // ukrycie funkcjonalności nie posiadających implementacji
            panelCreate.Visibility = Visibility.Collapsed;
#endif

            ILoginService service = ServiceManager.Instance.Login;
            service.LoginCompleted += service_LoginCompleted;
            service.LoginRejected += service_LoginRejected;
        }

        #region Obsługa zdarzeń z serwisu logowania
        void service_LoginRejected(string message)
        {
            Locked = false;
            Message = message;
        }

        void service_LoginCompleted(UserEntity user)
        {
            Locked = false;

            ServiceManager.Instance.CurrentUser = user;

            NavigationService.Navigate(new Uri("/SelectCommisionsPage.xaml", UriKind.Relative));
        }
        #endregion

        #region Obsługa poleceń użytkownika
        /// <summary>
        /// Obsługa przycisku "Zaloguj"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonLogin_Click(object sender, RoutedEventArgs e)
        {
            ILoginService service = ServiceManager.Instance.Login;
            string userName = textUserName.Text;
            string userPassword = textUserPassword.Password;
            
            try
            {
                Locked = true;
                service.BeginLogin(userName, userPassword);
            }
            catch(LoginException ex)
            {
                Locked = false;
                Message = ex.Message;                
            }
        }

        /// <summary>
        /// Obsługa przycisku "Utwórz"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonCreate_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Rejestracja użytkownika");
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            ILoginService service = ServiceManager.Instance.Login;
            string userName = null;
            string userPassword = null;

            try
            {
                Locked = true;
                service.BeginLogin(userName, userPassword);
            }
            catch (LoginException ex)
            {
                Locked = false;
                Message = ex.Message;
            }
        }
        #endregion
    }
}