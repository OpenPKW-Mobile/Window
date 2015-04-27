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
        /// <summary>
        /// Informacja dla użytkownika o błędzie logowania.
        /// Wyświetla się na dole strony.
        /// </summary>
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

        /// <summary>
        /// Określa, czy strona jest zablokowana do edycji.
        /// Blokowanie edycji następuje w momencie komunikacji z serwerem.
        /// </summary>
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

        /// <summary>
        /// Inicjalizacja strony i wszystkich kontrolek.
        /// </summary>
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

            // podpięcie się pod zdarzenia z serwisu logowania
            // aplikacja będzie mogła reagować w przypadku błędu lub poprawnego uwierzytelniania
            ILoginService service = ServiceManager.Instance.Login;
            service.LoginCompleted += service_LoginCompleted;
            service.LoginRejected += service_LoginRejected;
        }

        #region Obsługa zdarzeń z serwisu logowania
        
        /// <summary>
        /// Obsługa niepoprawnego wyniku procedury uwierzytelniania.
        /// </summary>
        /// <param name="message"></param>
        void service_LoginRejected(string message)
        {
            Locked = false;
            Message = message;
        }

        /// <summary>
        /// Obsługa poprawnego wyniku procedury uwierzytelniania.
        /// </summary>
        /// <param name="user"></param>
        void service_LoginCompleted(UserEntity user)
        {
            Locked = false;

            // należy zapamiętać bieżącego użytkownika
            // będzie on używany przy komunikacji z pozostałymi serwisami (token)
            ServiceManager.Instance.CurrentUser = user;

            // przejście do strony z wyborem komisji
            NavigationService.Navigate(new Uri("/SelectCommisionsPage.xaml", UriKind.Relative));
        }

        #endregion Obsługa zdarzeń z serwisu logowania

        #region Obsługa poleceń użytkownika

        /// <summary>
        /// Obsługa przycisku "Zaloguj".
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

                // rozpoczęcie procedury logowania użytkownika w tle
                // koniec procedury jest sygnalizowany poprzez zdarzenia
                service.BeginLogin(userName, userPassword);
            }
            catch(LoginException ex)
            {
                Locked = false;
                Message = ex.Message;                
            }

            // tutaj w tle trwa procedura logowania
            // ...
        }

        /// <summary>
        /// Obsługa przycisku "Utwórz"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonCreate_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Rejestracja użytkownika");

            // TODO
        }

        /// <summary>
        /// Obsługa przeniesienia nawigacji do bieżącej strony.
        /// Próba automatycznego zalogowania użytkownika.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            // aplikacja spróbuje zalogować się przy użyciu zapamiętanego tokena
            // nazwa użytkownik i hasło powinny być puste
            ILoginService service = ServiceManager.Instance.Login;
            string userName = null;
            string userPassword = null;

            try
            {
                Locked = true;

                // rozpoczęcie procedury logowania użytkownika w tle
                // koniec procedury jest sygnalizowany poprzez zdarzenia
                service.BeginLogin(userName, userPassword);
            }
            catch (LoginException ex)
            {
                Locked = false;
                Message = ex.Message;
            }

            // tutaj w tle trwa procedura logowania
            // ...
        }
        #endregion
    }
}