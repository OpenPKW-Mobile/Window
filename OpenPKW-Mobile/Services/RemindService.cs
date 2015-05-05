using OpenPKW_Mobile.Providers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenPKW_Mobile.Services
{
    /// <summary>
    /// usługa zmiany hasła użytkownika
    /// </summary>
    class RemindService : IRemindService
    {
        /// <summary>
        /// Zestaw danych przekazywanych pomiędzy wątkami.
        /// </summary>
        struct WorkerData
        {
            public string UserName;
            public string UserEmail;
            public IAuthenticationProvider AuthenticationProvider;
        }

        private IAuthenticationProvider _provider;
        private BackgroundWorker _worker;

        public event Action<string> RemindCompleted;
        public event Action<string> RemindRejected;

        /// <summary>
        /// Inicjalizacja serwisu odzyskiwania hasła.
        /// </summary>
        /// <param name="provider"></param>
        public RemindService(IAuthenticationProvider provider)
        {
            // przygotowanie usługi do pracy w tle
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += remindProcess;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;

            this._provider = provider;
            this._worker = worker;
        }

        void IRemindService.BeginRemind(string userName, string userEmail)
        {
            BackgroundWorker worker = this._worker;
            IAuthenticationProvider provider = this._provider;

            WorkerData data = new WorkerData()
            {
                UserName = userName,
                UserEmail = userEmail,
                AuthenticationProvider = provider
            };

            // uruchomienie procedury wysyłania prośby o odzyskanie hasła w osobnym wątku
            worker.RunWorkerAsync(data);

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
            var provider = ((WorkerData)e.Argument).AuthenticationProvider;
            var userName = ((WorkerData)e.Argument).UserName;
            var userEmail = ((WorkerData)e.Argument).UserEmail;


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


    }
}
