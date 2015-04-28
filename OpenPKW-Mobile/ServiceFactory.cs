using OpenPKW_Mobile.Providers;
using OpenPKW_Mobile.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenPKW_Mobile
{
    static class ServiceFactory
    {
        public static IAuthenticationProvider AuthenticationProvider;

        public static LoginService Login
        {
            get
            {
                return new LoginService(AuthenticationProvider);
            }
        }
    }
}
