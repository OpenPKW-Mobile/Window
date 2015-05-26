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
        /// Pobranie informacji o komisji wyborczej.
        /// </summary>
        /// <param name="commissionID">Identyfikator komisji</param>
        /// <returns>Dane adresowe komisji</returns>
        CommissionEntity GetCommission(UserEntity user, string commissionID);

        /// <summary>
        /// Pobranie listy kandydatów.
        /// </summary>
        /// <returns>Lista kandydatów, wartość "null" w przypadku błędu.</returns>
        CandidateEntity[] GetCandidates(UserEntity user, CommissionEntity commission);

        /// <summary>
        /// Wysłanie wyników głosowania.
        /// </summary>
        /// <param name="election">Wyniki głosowania.</param>
        /// <returns></returns>
        bool UploadResults(UserEntity user, CommissionEntity commission, ElectionEntity election, CandidateEntity[] candidates);
    }
}
