using Newtonsoft.Json;
using OpenPKW_Mobile.Entities;
using OpenPKW_Mobile.Providers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace OpenPKW_Mobile.Services
{
    /// <summary>
    /// Usługa logowania użytkownika do systemu.
    /// </summary>
    class LoginService : ILoginService
    {
        /// <summary>
        /// Zestaw danych przekazywanych pomiędzy wątkami.
        /// </summary>
        struct WorkerData
        {
            public string UserName;
            public string UserPassword;
            public IAuthenticationProvider AuthenticationProvider;
        }

        private IAuthenticationProvider _provider;
        private BackgroundWorker _worker;

        public event Action<UserEntity> LoginCompleted;
        public event Action<string> LoginRejected;

        /// <summary>
        /// Inicjalizacja serwisu logowania.
        /// </summary>
        /// <param name="provider"></param>
        public LoginService(IAuthenticationProvider provider)
        {
            // przygotowanie usługi do pracy w tle
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += loginProcess;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;

            this._provider = provider;
            this._worker = worker;
        }
       
        void ILoginService.BeginLogin(string userName, string userPassword)
        {
            BackgroundWorker worker = this._worker;
            IAuthenticationProvider provider = this._provider;

            WorkerData data = new WorkerData()
            {
                UserName = userName,
                UserPassword = userPassword,
                AuthenticationProvider = provider
            };

            // uruchomienie procedury logowania w osobnym wątku
            worker.RunWorkerAsync(data);

            // tutaj cały czas trwa procedura logowania
            // ...
        }

        /// <summary>
        /// Obsługa wyniku procedury logowania.
        /// </summary>
        /// <param name="sender">Nadawca zdarzenia.</param>
        /// <param name="e">Wyniki przetwarzania.</param>
        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                // poinformowanie słuchaczy o problemie
                if (LoginRejected != null)
                    LoginRejected(e.Error.Message);
            }
            else 
            {
                // poinformowanie słuchaczy o poprawnym wyniku logowania
                // od tego momentu użytkownik jest identyfikowany poprzez obiekt typu [UserEntity]
                UserEntity user = (UserEntity)e.Result;
                if (LoginCompleted != null)
                    LoginCompleted(user);
            }
        }

        /// <summary>
        /// Procedura logowania.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void loginProcess(object sender, DoWorkEventArgs e)
        {
            var provider = ((WorkerData)e.Argument).AuthenticationProvider;
            var userName = ((WorkerData)e.Argument).UserName;
            var userPassword = ((WorkerData)e.Argument).UserPassword;

            byte[] entropy = { 8, 3, 9, 5, 2 };
            string settingKey = "user";
            UserEntity user = null;
            
            // informacja o użykowniku jest przechowywana w izolowanym miejscu
            // jest używana do automatycznego logowania użytkownika
            IsolatedStorageSettings settings = 
                IsolatedStorageSettings.ApplicationSettings;

            // sprawdzenie obecności danych użytkownika z poprzedniego logowania
            if (settings.Contains(settingKey))
            {
                // informacja o użytkowniku przechowywana jest w postaci szyfrowanej
                // dane użytkownika przed użyciem należy odszyfrować
                var encryptedUser = settings[settingKey] as byte[];
                var userBytes = ProtectedData.Unprotect(encryptedUser, entropy);
                var userJson = Encoding.Unicode.GetString(userBytes, 0, userBytes.Length);
                user = JsonConvert.DeserializeObject<UserEntity>(userJson);
            }
            
            // dwa tryby logowania:
            // - automatyczne, gdy istnieją dane użytkownika z poprzedniego logowania
            // - ręczne, gdy użytkownik podaje poświadczenia
            if (user != null)
            {
                // tryb automatyczny
                // należy sprawdzić, czy odczytane dane użytkownika są nadal prawidłowe
                // dane użytkownik zawierają token, który powinien być ważny tylko przez pewien czas
                if (provider.IsValid(user) == false)
                {
                    settings.Remove(settingKey);
                    user = null;

                    throw new LoginException(
                        LoginException.ErrorReason.SessionExpired);
                }
            }
            else
            {
                // tryb ręczny
                // niedozwolone jest logowanie przy użyciu niepełnych danych
                if (String.IsNullOrWhiteSpace(userName))
                {
                    throw new LoginException(
                        LoginException.ErrorReason.AnonymousNotAllowed);
                }
                else if (String.IsNullOrWhiteSpace(userPassword))
                {
                    throw new LoginException(
                        LoginException.ErrorReason.PasswordRequired);
                }
                else
                {
                    // dane są poprawnie przygotowane
                    // uwierzytelnianie następuje poprzez wybranego dostawcę usługi
                    user = provider.Authenticate(userName, userPassword);
                    if (user != null)
                    {
                        // zapisanie danych użytkownika do izolowanego repozytorium
                        // dane są przechowywane w postaci zaszyfrowanej
                        var userJson = JsonConvert.SerializeObject(user);
                        var userBytes = Encoding.Unicode.GetBytes(userJson);
                        var encryptedUser = ProtectedData.Protect(userBytes, entropy);
                        settings.Add(settingKey, encryptedUser);
                        settings.Save();
                    }
                    else
                    {
                        throw new LoginException(
                            LoginException.ErrorReason.IncorrectNameOrPassword);
                    }
                }
            }

            e.Result = user;
        }

    }
}
