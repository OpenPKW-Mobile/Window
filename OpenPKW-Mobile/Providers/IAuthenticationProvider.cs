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
        UserEntity UserLogin(string userName, string userPassword);

        /// <summary>
        /// Zakończenie sesji użytkownika.
        /// </summary>
        /// <param name="user">Dane użytkownika.</param>
        bool UserLogout(UserEntity user);

        /// <summary>
        /// Sprawdzenie ważności sesji.
        /// </summary>
        /// <param name="user">Dane użytkownika.</param>
        /// <returns>Potwierdzenie ważności sesji.</returns>
        bool IsSessionValid(UserEntity user);

        /// <summary>
        /// Odzyskiwanie hasła
        /// </summary>
        /// <param name="userName">Nazwa użytkownika.</param>
        /// <param name="userEmail">E-mail użytkownika.</param>
        /// <returns>Informacja, czy powiodło się wysłanie prośby o odzyskanie hasła</returns>
        bool PasswordRemind(string userName, string email);

        /// <summary>
        /// Rejestracja nowego użytkownika.
        /// </summary>
        /// <param name="firstName">Imię</param>
        /// <param name="lastName">Nazwisko</param>
        /// <param name="email">Konto e-mail</param>
        /// <returns>Informaca, czy rejestracja została wykonana prawidłowo</returns>
        bool UserRegister(string firstName, string lastName, string email);
    }
}
