using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenPKW_Mobile.Entities
{
    public class CandidateEntity : EntityBase
    {
        /// <summary>
        /// Nazwisko.
        /// </summary>
        public string Surname { get; set; }

        /// <summary>
        /// Pierwsze imię.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Drugie imię.
        /// </summary>
        public string SecondName { get; set; }

        /// <summary>
        /// Liczba oddanych głosów.
        /// </summary>
        public int? Votes { get; set; }


        public override string Identifier
        {
            get 
            {
                if (SecondName != null)
                {
                    return String.Format("{0} {1}, {2}", Surname.ToUpper(), FirstName, Surname);
                }
                else
                {
                    return String.Format("{0} {1}", Surname.ToUpper(), FirstName);
                }
            }
        }

        public override string ToString()
        {
            return Identifier;
        }
    }
}
