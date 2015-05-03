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
        /// Rozpoczęcie procedury uwierzytelniania.
        /// Procedura jest wykonywana w tle, a jej wynik zgłaszany poprzez zdarzenia.
        /// </summary>
        /// <param name="userName">Nazwa użytkownika.</param>
        /// <param name="userPassword">Hasło użytkownika.</param>
        void BeginLogin(string userName, string userPassword);

    }
}
