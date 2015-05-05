using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenPKW_Mobile.Services
{
    interface IRemindService
    {
        /// <summary>
        /// Zdarzenie informujące o poprawej zmianie hasłą
        /// </summary>
        event Action<string> RemindCompleted;

        /// <summary>
        /// Zdarzenie informujące o błedzie podczas zmiany hasła
        /// </summary>
        event Action<string> RemindRejected;

        /// <summary>
        /// Rozpoczęcie procedury odzyskiwanie hasła.
        /// Procedura jest wykonywana w tle, a jej wynik zgłaszany poprzez zdarzenia.
        /// </summary>
        /// <param name="userName">Nazwa użytkownika.</param>
        /// <param name="userEmail">E-mail użytkownika.</param>        
        void BeginRemind(string userName, string userEmail);
    }
}
