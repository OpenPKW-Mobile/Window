using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenPKW_Mobile.Entities
{
    public class ElectionEntity : EntityBase
    {
        /// <summary>
        /// Liczba uprawnionych do głosowania.
        /// </summary>
        public int Voters { get; set; }

        /// <summary>
        /// Liczba wydanych kart do głosowania.
        /// </summary>
        public int Cards { get; set; }

        /// <summary>
        /// Liczna ważnych kart do głosowania.
        /// </summary>
        public int ValidCards { get; set; }

        /// <summary>
        /// Liczba ważnych głosów.
        /// </summary>
        public int ValidVotes { get; set; }

        /// <summary>
        /// Liczba nieważnych głosów.
        /// </summary>
        public int InvalidVotes { get; set; }

        public override string Identifier
        {
            get
            {
                throw new NotImplementedException();
            }
        }
    }
}
