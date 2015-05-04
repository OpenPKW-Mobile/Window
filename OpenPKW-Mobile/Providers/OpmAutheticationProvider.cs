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
    class OpmAuthenticationProvider : ProviderBase, IAuthenticationProvider
    {
        UserEntity IAuthenticationProvider.Authenticate(string userName, string userPassword)
        {
#if DEBUG
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
#else
            // TODO
            return null;
#endif
        }

        bool IAuthenticationProvider.IsValid(UserEntity user)
        {
#if DEBUG
            var message = String.Format("Czy token '{0}' użytkownika '{1} {2}' jest prawidłowy ?", 
                user.AuthenticationToken, user.FirstName, user.LastName);
            var result = ShowMessage(message);

            // symuluje opóżnienia w komunikacji z zewnętrzną usługą
            Thread.Sleep(3000);

            return (result == MessageBoxResult.OK);
#else
            // TODO
            return false;
#endif
        }
    }
}
