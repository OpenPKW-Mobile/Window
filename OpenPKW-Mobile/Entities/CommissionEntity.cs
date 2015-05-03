using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenPKW_Mobile.Entities
{
    public class CommissionEntity : EntityBase
    {
        /// <summary>
        /// Identyfikator komisji
        /// </summary>
        public string Id { get; set; }
        
        /// <summary>
        /// Typ komisji.
        /// </summary>
        /// <example>Obwodowa Komisja wyborcza</example>
        public string Type { get; set; }

        /// <summary>
        /// Nazwa komisji.
        /// </summary>
        /// <example>Nr 5 w Łodzi</example>
        public string Title { get; set; }

        /// <summary>
        /// Miejsce wykonywania czynności.
        /// </summary>
        /// <example>Laboratorium Produkcji Ogrodniczej w Zespole Szkół Rzemiosła im. Jana Kilińskiego</example>
        public string Place { get; set; }

        /// <summary>
        /// Adres miejsca.
        /// </summary>
        /// <example>ul. Liściasta 181, 91-220 Łódź</example>
        public string Address { get; set; }

        public override string Identifier
        {
            get 
            { 
                return Id.ToString(); 
            }
        }
    }
}
