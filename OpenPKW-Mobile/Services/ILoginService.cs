using OpenPKW_Mobile.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenPKW_Mobile.Services
{
    interface ILoginService
    {
        /// <summary>
        /// Zdarzenie informujące o poprawnym uwierzytelnieniu użytkownika.
        /// </summary>
        event Action<UserEntity> LoginCompleted;

        /// <summary>
        /// Zdarzenie informujące o błedzie uwierzytelniania użytkownika.
        /// </summary>
        event Action<string> LoginRejected;

        /// <summary>
        /// Zdarzenie informujące o poprawnym zamknięciu sesji użytkownika.
        /// </summary>
        event Action LogoutCompleted;

        /// <summary>
        /// Zdarzenie informujące o błedzie podczas zamykania sesji użytkownika.
        /// </summary>
        event Action<string> LogoutRejected;

        /// <summary>
        /// Zdarzenie informujące o poprawej zmianie hasłą
        /// </summary>
        event Action<string> RemindCompleted;

        /// <summary>
        /// Zdarzenie informujące o błedzie podczas zmiany hasła
        /// </summary>
        event Action<string> RemindRejected;
       
        /// <summary>
        /// Rozpoczęcie procedury uwierzytelniania.
        /// Procedura jest wykonywana w tle, a jej wynik zgłaszany poprzez zdarzenia.
        /// </summary>
        /// <param name="userName">Nazwa użytkownika.</param>
        /// <param name="userPassword">Hasło użytkownika.</param>
        void BeginLogin(string userName, string userPassword);

        /// <summary>
        /// Rozpoczęcie procedury zamykania sesji użytkownika.
        /// Procedura jest wykonywana w tle, a jej wynik zgłaszany poprzez zdarzenia.
        /// </summary>
        /// <param name="user"></param>
        void BeginLogout(UserEntity user);

        /// <summary>
        /// Rozpoczęcie procedury odzyskiwanie hasła.
        /// Procedura jest wykonywana w tle, a jej wynik zgłaszany poprzez zdarzenia.
        /// </summary>
        /// <param name="userName">Nazwa użytkownika.</param>
        /// <param name="userEmail">E-mail użytkownika.</param>        
        void BeginRemind(string userName, string userEmail);
    }
}
