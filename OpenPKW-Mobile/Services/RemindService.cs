using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenPKW_Mobile.Services
{
    /// <summary>
    /// usługa zmiany hasła użytkownika
    /// </summary>
    class RemindService : IRemindService
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
    }
}
