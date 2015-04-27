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
    class LoginService : ILoginService
    {
        private IAuthenticationProvider _provider;
        private BackgroundWorker _worker;

        public LoginService(IAuthenticationProvider provider)
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += loginProcess;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;

            this._provider = provider;
            this._worker = worker;

        }

        struct WorkerData
        {
            public string UserName;
            public string UserPassword;
            public IAuthenticationProvider AuthenticationProvider;
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

            worker.RunWorkerAsync(data);
        }

        public event Action<UserEntity> LoginCompleted;
        public event Action<string> LoginRejected;

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                if (LoginRejected != null)
                    LoginRejected(e.Error.Message);
            }
            else 
            {
                UserEntity user = (UserEntity)e.Result;
                if (LoginCompleted != null)
                    LoginCompleted(user);
            }
        }

        private void loginProcess(object sender, DoWorkEventArgs e)
        {
            var provider = ((WorkerData)e.Argument).AuthenticationProvider;
            var userName = ((WorkerData)e.Argument).UserName;
            var userPassword = ((WorkerData)e.Argument).UserPassword;

            byte[] entropy = { 8, 3, 9, 5, 2 };
            string settingKey = "user";
            UserEntity user = null;
            
            IsolatedStorageSettings settings = 
                IsolatedStorageSettings.ApplicationSettings;

            if (settings.Contains(settingKey))
            {
                var encryptedUser = settings[settingKey] as byte[];
                var userBytes = ProtectedData.Unprotect(encryptedUser, entropy);
                var userJson = Encoding.Unicode.GetString(userBytes, 0, userBytes.Length);
                user = JsonConvert.DeserializeObject<UserEntity>(userJson);
            }
            
            if (user != null)
            {
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
                    user = provider.Authenticate(userName, userPassword);
                    if (user != null)
                    {
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
