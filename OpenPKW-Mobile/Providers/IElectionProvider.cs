using OpenPKW_Mobile.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenPKW_Mobile.Providers
{
    interface IElectionProvider
    {
        /// <summary>
        /// Pobranie listy kandydatów.
        /// </summary>
        /// <returns>Lista kandydatów, wartość "null" w przypadku błędu.</returns>
        CandidateEntity[] GetCandidates();

        /// <summary>
        /// Wysłanie wyników głosowania.
        /// </summary>
        /// <param name="election">Wyniki głosowania.</param>
        /// <returns></returns>
        bool SendResults(ElectionEntity election);
    }
}
