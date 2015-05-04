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
        /// <summary>
        /// Uwierzytelnianie użytkownika.
        /// </summary>
        /// <param name="userName">Nazwa użytkownika.</param>
        /// <param name="userPassword">Hasło użytkownika.</param>
        /// <returns>Dane użytkownika, wartość "null" w przypadku błedu.</returns>
        UserEntity Authenticate(string userName, string userPassword);

        /// <summary>
        /// Sprawdzenie ważności sesji.
        /// </summary>
        /// <param name="user">Dane użytkownika.</param>
        /// <returns>Potwierdzenie ważności sesji.</returns>
        bool IsValid(UserEntity user);
    }
}
