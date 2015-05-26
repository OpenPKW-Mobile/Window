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
using System.Threading.Tasks;

namespace OpenPKW_Mobile.Services
{
    partial class LoginService
    {
        /// <summary>
        /// Zestaw danych przekazywanych pomiędzy wątkami.
        /// </summary>
        struct LoginData
        {
            public string UserName;
            public string UserPassword;
            public IAuthenticationProvider Provider;
        }

        WorkerHandle Login;

        public event Action<UserEntity> LoginCompleted;
        public event Action<string> LoginRejected;

        void ILoginService.BeginLogin(string userName, string userPassword)
        {
            IAuthenticationProvider provider = this._provider;
            WorkerContext context = new WorkerContext()
            {
                DoWork = loginProcess,
                RunWorkerCompleted = loginCompleted,
                UserData = new LoginData()
                {
                    UserName = userName,
                    UserPassword = userPassword,
                    Provider = provider
                }
            };

            Login = Begin(context);

            // tutaj cały czas trwa procedura logowania
            // ...
        }

        /// <summary>
        /// Procedura logowania.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void loginProcess(object sender, DoWorkEventArgs e)
        {
            var provider = ((LoginData)e.Argument).Provider;
            var userName = ((LoginData)e.Argument).UserName;
            var userPassword = ((LoginData)e.Argument).UserPassword;

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
                if (provider.IsSessionValid(user) == false)
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
                    user = provider.UserLogin(userName, userPassword);
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


        /// <summary>
        /// Obsługa wyniku procedury logowania.
        /// </summary>
        /// <param name="sender">Nadawca zdarzenia.</param>
        /// <param name="e">Wyniki przetwarzania.</param>
        void loginCompleted(object sender, RunWorkerCompletedEventArgs e)
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
        /// Rodzaj wyjątku używany w procesie logowania użytkownika.
        /// </summary>
        public class LoginException : ApplicationException
        {
            public enum ErrorReason
            {
                AnonymousNotAllowed,
                PasswordRequired,
                IncorrectNameOrPassword,
                SessionExpired,
            }

            private ErrorReason _reason;
            private Dictionary<ErrorReason, string> _messages;

            public LoginException(ErrorReason reason)
            {
                this._reason = reason;
                this._messages = new Dictionary<ErrorReason, string>()
                {
                    { ErrorReason.AnonymousNotAllowed, "Aby zalogować się do systemu, musisz podać nazwę użytkownika oraz hasło." },
                    { ErrorReason.PasswordRequired, "Powinieneś podać hasło, które otrzymałeś od administratora systemu." },
                    { ErrorReason.IncorrectNameOrPassword, "Prawdopodobnie popełniłeś błąd wprowadzając nazwę użytkownika lub hasło." },
                    { ErrorReason.SessionExpired, "Od Twojej ostatniej aktywności upłynęło trochę czasu, więc zaloguj się ponownie." },
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
