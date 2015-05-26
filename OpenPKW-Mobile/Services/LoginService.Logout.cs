using OpenPKW_Mobile.Entities;
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
        struct LogoutData
        {
            public UserEntity User;
            public IAuthenticationProvider Provider;
        }

        WorkerHandle Logout;

        public event Action LogoutCompleted;
        public event Action<string> LogoutRejected;

        void ILoginService.BeginLogout(UserEntity user)
        {
            IAuthenticationProvider provider = this._provider;
            WorkerContext context = new WorkerContext()
            {
                DoWork = logoutProcess,
                RunWorkerCompleted = logoutCompleted,
                UserData = new LogoutData()
                {
                    User = user,
                    Provider = provider
                }
            };

            Logout = Begin(context);

            // tutaj cały czas trwa procedura logowania
            // ...        
        }

        private void logoutProcess(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            var worker = (BackgroundWorker)sender;
            var provider = ((LogoutData)e.Argument).Provider;
            var user = ((LogoutData)e.Argument).User;

            bool result = provider.UserLogout(user);
            if (result == false)
            {
                throw new LogoutException(
                     LogoutException.ErrorReason.RequestRejected);
            }
             
        }

        private void logoutCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                // poinformowanie słuchaczy o problemie
                if (LogoutRejected != null)
                    LogoutRejected(e.Error.Message);
            }
            else
            {
                // poinformowanie słuchaczy o poprawnym wyniku logowania
                // od tego momentu użytkownik jest identyfikowany poprzez obiekt typu [UserEntity]
                if (LogoutCompleted != null)
                    LogoutCompleted();
            }
        }

        /// <summary>
        /// Rodzaj wyjątku używany w procesie logowania użytkownika.
        /// </summary>
        public class LogoutException : ApplicationException
        {
            public enum ErrorReason
            {
                RequestRejected,
            }

            private ErrorReason _reason;
            private Dictionary<ErrorReason, string> _messages;

            public LogoutException(ErrorReason reason)
            {
                this._reason = reason;
                this._messages = new Dictionary<ErrorReason, string>()
                {
                    { ErrorReason.RequestRejected, "Operacja wylogowania użytkownika zakończyła się niepowodzeniem." },
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
