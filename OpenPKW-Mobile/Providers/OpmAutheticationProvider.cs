using OpenPKW_Mobile.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace OpenPKW_Mobile.Providers
{
    class OpmAuthenticationProvider : IAuthenticationProvider
    {
#if DEBUG
        private MessageBoxResult showMessage(string message)
        {
            AutoResetEvent @event = new AutoResetEvent(false);
            MessageBoxResult result = MessageBoxResult.None;

            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                result = MessageBox.Show(message, GetType().Name, MessageBoxButton.OKCancel);
                @event.Set();
            });

            @event.WaitOne();

            return result;
        }
#endif

        UserEntity IAuthenticationProvider.Authenticate(string userName, string userPassword)
        {
#if DEBUG
            var message = String.Format("Czy zalogować użytkownika z poświadczeniami '{0} {1}' ?", userName, userPassword);            
            var result = showMessage(message);

            Thread.Sleep(3000);

            if (result == MessageBoxResult.Cancel)
                return null;
            else
            {
                return new UserEntity() 
                { 
                    FirstName = "firstName_1", 
                    LastName = "lastName_1", 
                    AuthenticationToken = "token_1", 
                    UserID = "userID_1" 
                };                
            }
#else
            Thread.Sleep(5000);
            return null;
#endif
        }

        bool IAuthenticationProvider.IsValid(UserEntity user)
        {
            var message = String.Format("Czy token '{0}' użytkownika '{1} {2}' jest prawidłowy ?", user.AuthenticationToken, user.FirstName, user.LastName);
            var result = showMessage(message);

            Thread.Sleep(3000);

            return (result == MessageBoxResult.OK);
        }
    }
}
