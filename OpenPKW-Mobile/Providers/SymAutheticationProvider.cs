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
    class SymAuthenticationProvider : ProviderBase, IAuthenticationProvider
    {
        UserEntity IAuthenticationProvider.UserLogin(string userName, string userPassword)
        {
            var message = String.Format("Czy zalogować użytkownika z poświadczeniami '{0} {1}' ?", 
                userName, userPassword);            
            var result = ShowMessage(message);

            // symuluje opóżnienia w komunikacji z zewnętrzną usługą
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
        }

        bool IAuthenticationProvider.UserLogout(UserEntity user)
        {
            var message = String.Format("Czy użytkownik '{0} {1}' może się wylogować ?",
                user.FirstName, user.LastName);
            var result = ShowMessage(message);

            // symuluje opóżnienia w komunikacji z zewnętrzną usługą
            Thread.Sleep(3000);

            return (result == MessageBoxResult.OK);
        }

        bool IAuthenticationProvider.IsSessionValid(UserEntity user)
        {
            var message = String.Format("Czy token '{0}' użytkownika '{1} {2}' jest prawidłowy ?",
                user.AuthenticationToken, user.FirstName, user.LastName);
            var result = ShowMessage(message);

            // symuluje opóżnienia w komunikacji z zewnętrzną usługą
            Thread.Sleep(3000);

            return (result == MessageBoxResult.OK);
        }

        bool IAuthenticationProvider.PasswordRemind(string userName, string userEmail)
        {
            var message = String.Format("Czy wysłać na adres mailowy {1} linka umożliwiającego zresetowanie hasła użytkownika {0}?",
                userName, userEmail);
            var result = ShowMessage(message);

            // symuluje opóżnienia w komunikacji z zewnętrzną usługą
            Thread.Sleep(3000);

            if (result == MessageBoxResult.Cancel)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        bool IAuthenticationProvider.UserRegister(string firstName, string lastName, string email)
        {
            var message = String.Format("Czy zarejestrować użytkownika '{0} {1}' ?",
                firstName, lastName);
            var result = ShowMessage(message);

            // symuluje opóżnienia w komunikacji z zewnętrzną usługą
            Thread.Sleep(3000);

            return (result == MessageBoxResult.OK);
        }
    }
}
