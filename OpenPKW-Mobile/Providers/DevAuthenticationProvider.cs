using OpenPKW_Mobile.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenPKW_Mobile.Providers
{
    class DevAuthenticationProvider : IAuthenticationProvider
    {
        UserEntity IAuthenticationProvider.Authenticate(string userName, string userPassword)
        {
            throw new NotImplementedException();
        }

        bool IAuthenticationProvider.PasswordRemind(string userName, string userEmail)
        {
            throw new NotImplementedException();
        }

        bool IAuthenticationProvider.IsValid(UserEntity user)
        {
            throw new NotImplementedException();
        }
    }
}
