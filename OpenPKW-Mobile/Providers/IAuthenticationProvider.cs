using OpenPKW_Mobile.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenPKW_Mobile.Providers
{
    interface IAuthenticationProvider
    {
        UserEntity Authenticate(string userName, string userPassword);
        bool IsValid(UserEntity user);
    }
}
