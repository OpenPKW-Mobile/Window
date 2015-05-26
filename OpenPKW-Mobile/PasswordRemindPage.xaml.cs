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

namespace OpenPKW_Mobile
{
    public partial class PasswordReminderPage : PhoneApplicationPage
    {
        /// <summary>
        /// Obsługa niepoprawnego wyniku procedury odzyskiwania hasła.
        /// </summary>
        /// <param name="message"></param>
        void service_RemindRejected(string message)
        {
            Locked = false;
            Message = message;
        }

        /// <summary>
        /// Obsługa poprawnego wyniku procedury odzyskiwania hasła.
        /// </summary>
        /// <param name="user"></param>
        void service_RemindCompleted(string message)
        {
            Locked = false;
            textUserName.Text = "";
            textUserEmail.Text = "";

            //wyswietlenie komunikatu o zakończeniu działania
            Message= message;
            // TODO
        }

        public PasswordReminderPage()
        {
            InitializeComponent();

            // ukrycie elementów, które nie powinny być początkowo widoczne,
            // a zostały dodane do formatki w celach projektowych
            panelWarning.Visibility = Visibility.Collapsed;

#if !DEBUG
            // ukrycie funkcjonalności nie posiadających implementacji
            panelCreate.Visibility = Visibility.Collapsed;
#endif

            // podpięcie się pod zdarzenia z serwisu odzyskiwania hasła
            // aplikacja będzie mogła reagować w przypadku błędu lub poprawnego wykonania operacji
            ILoginService service = ServiceManager.Instance.Login;
            service.RemindCompleted += service_RemindCompleted;
            service.RemindRejected += service_RemindRejected;
        }

        /// <summary>
        /// Informacja dla użytkownika o statusie operacji.
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

        private void buttonRemind_Click(object sender, RoutedEventArgs e)
        {
            ILoginService service = ServiceManager.Instance.Login;
            string userName = textUserName.Text;
            string userEmail = textUserEmail.Text;

            try
            {
                Locked = true;

                // rozpoczęcie procedury odzyskiwania hasła użytkownika w tle
                // koniec procedury jest sygnalizowany poprzez zdarzenia
                service.BeginRemind(userName, userEmail);
            }
            catch (LoginService.RemindException ex)
            {
                Locked = false;
                Message = ex.Message;
            }

            // tutaj w tle trwa procedura odzyskiwania hasła
            // ...
        }
    }
}