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
    /// <summary>
    /// Usługa logowania użytkownika do systemu.
    /// </summary>
    partial class LoginService : ServiceBase, ILoginService
    {

        /// <summary>
        /// Dostawca usługi zarządzania użytkownikami.
        /// </summary>
        private IAuthenticationProvider _provider;

        /// <summary>
        /// Inicjalizacja serwisu logowania.
        /// </summary>
        /// <param name="provider"></param>
        public LoginService(IAuthenticationProvider provider)
        {
            this._provider = provider;
        }
    }
}
