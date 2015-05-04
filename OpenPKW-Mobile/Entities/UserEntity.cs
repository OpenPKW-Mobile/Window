using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenPKW_Mobile.Entities
{
    class UserEntity : EntityBase
    {
        /// <summary>
        /// Identyfikator użytkownika.
        /// </summary>
        public string UserID { get; set; }

        /// <summary>
        /// Imię.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Nazwisko.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Token.
        /// </summary>
        public string AuthenticationToken { get; set; }

        public override string Identifier
        {
            get 
            {
                return UserID;
            }
        }
    }
}
