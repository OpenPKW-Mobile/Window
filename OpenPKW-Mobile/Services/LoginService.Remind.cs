using OpenPKW_Mobile.Providers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenPKW_Mobile.Services
{
    partial class LoginService
    {
        /// <summary>
        /// Zestaw danych przekazywanych pomiędzy wątkami.
        /// </summary>
        struct RemindData
        {
            public string UserName;
            public string UserEmail;
            public IAuthenticationProvider Provider;
        }

        WorkerHandle Remind;

        public event Action<string> RemindCompleted;
        public event Action<string> RemindRejected;

        void ILoginService.BeginRemind(string userName, string userEmail)
        {
            IAuthenticationProvider provider = this._provider;
            WorkerContext context = new WorkerContext()
            {
                DoWork = remindProcess,
                RunWorkerCompleted = worker_RunWorkerCompleted,
                UserData = new RemindData()
                {
                    UserName = userName,
                    UserEmail = userEmail,
                    Provider = provider
                }
            };

            Remind = Begin(context);

            // tutaj cały czas trwa procedura wysyłania prośby o odzyskanie hasła
            // ...
        }

        /// <summary>
        /// Obsługa wyniku procedury odzyskiwania hasła.
        /// </summary>
        /// <param name="sender">Nadawca zdarzenia.</param>
        /// <param name="e">Wyniki przetwarzania.</param>
        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                // poinformowanie słuchaczy o problemie
                if (RemindRejected != null)
                    RemindRejected(e.Error.Message);
            }
            else
            {
                // poinformowanie słuchaczy o poprawnym wyniku procedury prośby o zresetowanie hasła                
                bool isSuccess = (bool)e.Result;
                if (RemindCompleted != null)
                {
                    RemindCompleted("Na twojego maila wysłaliśmy linka do zresetowania hasła");
                }
            }
        }

        /// <summary>
        /// Procedura odzyskiwania hasła.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void remindProcess(object sender, DoWorkEventArgs e)
        {
            var provider = ((RemindData)e.Argument).Provider;
            var userName = ((RemindData)e.Argument).UserName;
            var userEmail = ((RemindData)e.Argument).UserEmail;

            // niedozwolone jest wysyłanie niepełnych danych
            if (String.IsNullOrWhiteSpace(userName))
            {
                throw new RemindException(
                    RemindException.ErrorReason.AnonymousNotAllowed);
            }
            else if (String.IsNullOrWhiteSpace(userEmail))
            {
                throw new RemindException(
                    RemindException.ErrorReason.EmailRequired);
            }
            else
            {
                // dane są poprawnie przygotowane
                // następuje wysłanie prośby o odzyskanie hasła

                if (provider.PasswordRemind(userName, userEmail))
                {
                    e.Result = true;
                }
                else
                {
                    throw new RemindException(
                        RemindException.ErrorReason.IncorrectNameOrEmail);
                }
            }
        }


        /// <summary>
        /// Rodzaj wyjątku używany w procesie odzyskiwania hasła przez użytkownika.
        /// </summary>
        public class RemindException : ApplicationException
        {
            public enum ErrorReason
            {
                AnonymousNotAllowed,
                EmailRequired,
                IncorrectNameOrEmail
            }

            private ErrorReason _reason;
            private Dictionary<ErrorReason, string> _messages;

            public RemindException(ErrorReason reason)
            {
                this._reason = reason;
                this._messages = new Dictionary<ErrorReason, string>()
                {
                    { ErrorReason.AnonymousNotAllowed, "Aby odzyskać hasło do systemu, musisz podać nazwę użytkownika oraz adres mailowy." },
                    { ErrorReason.EmailRequired, "Powinieneś podać adres e-mail, które otrzymałeś od administratora systemu." },
                    { ErrorReason.IncorrectNameOrEmail, "Prawdopodobnie popełniłeś błąd wprowadzając nazwę użytkownika lub adres mailowy." }                
                };
            }

            public override string Message
            {
                get
                {
                    return this._messages[_reason];
                }
            }
        }

    }
}
