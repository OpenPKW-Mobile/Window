using OpenPKW_Mobile.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenPKW_Mobile.Services
{
    interface IVotingService
    {
        /// <summary>
        /// Zdarzenie informujące o poprawnym odebraniu listy kandydatów.
        /// </summary>
        event Action<CandidateEntity[]> FetchCompleted;

        /// <summary>
        /// Zdarzenie informujące o błedzie przy odbiorze listy kandydatów.
        /// </summary>
        event Action<string> FetchRejected;

        /// <summary>
        /// Zdarzenie informujące o poprawnym wysłaniu wyników wyyborów.
        /// </summary>
        event Action SendCompleted;

        /// <summary>
        /// Zdarzenie informujące o nieudanych wysłaniu wyników wyborów.
        /// </summary>
        event Action<string> SendRejected;

        /// <summary>
        /// Rozpoczęcie procedury pobierania danych kandydatów.
        /// Procedura jest wykonywana w tle, a jej wynik zgłaszany poprzez zdarzenia.
        /// </summary>
        void BeginFetchCandidates();

        /// <summary>
        /// Rozpoczęcie procedury wysyłania wyników wyborów.
        /// Procedura jest wykonywana w tle, a jej wynik zgłaszany poprzez zdarzenia.
        /// </summary>
        /// <param name="election">Wyniki wyborów.</param>
        void BeginSendResults(ElectionEntity election);
    }
}
