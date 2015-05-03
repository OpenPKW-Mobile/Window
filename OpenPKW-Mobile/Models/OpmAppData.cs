using OpenPKW_Mobile.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenPKW_Mobile.Models
{
    /// <summary>
    /// Dane aplikacji Open PKW Mobile - wspólne dla wszystkich stron
    /// </summary>
    public class OpmAppData
    {
        /// <summary>
        /// Komisje wyborcze, wybrane przez użytkownika, dla których wysyła dane
        /// </summary>
        public IEnumerable<ElectoralCommission> SelectedCommisions { get; set; }

        /// <summary>
        /// Komisja wyborcza, aktualnie wybrana przez użytkownika
        /// </summary>
        public CommissionEntity CurrentCommision { get; set; }

        /// <summary>
        /// Lista kandydatów na urząd prezydenta
        /// </summary>
        public CandidateEntity[] Candidates { get; set; }

        /// <summary>
        /// Podsumowanie głosów z urny wyborczej
        /// </summary>
        public ElectionEntity Election { get; set; }

#if DEBUG
        public OpmAppData()
        {
            CommissionEntity commission = new CommissionEntity()
            {
                Type = "Obwodowa Komisja Wyborcza",
                Title = "Nr 5 w Łodzi",
                Id = "106101-5",
                Place = "Laboratorium Produkcji Ogrodniczej w Zespole Szkół Rzemiosła im. Jana Kilińskiego",
                Address = "ul. Liściasta 181, 91-220 Łódź"
            };

            CurrentCommision = commission;
        }
#endif
    }
}
