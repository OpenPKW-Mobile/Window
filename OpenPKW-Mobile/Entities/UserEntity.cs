using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OpenPKW_Mobile.Entities
{
    [DataContract]
    public class UserEntity : EntityBase
    {
        /// <summary>
        /// Identyfikator użytkownika.
        /// </summary>
        public string UserID { get; set; }

        /// <summary>
        /// Imię.
        /// </summary>
        [DataMember(Name = "firstname")]
        public string FirstName { get; set; }

        /// <summary>
        /// Nazwisko.
        /// </summary>
        [DataMember(Name = "lastname")]
        public string LastName { get; set; }

        /// <summary>
        /// Token.
        /// </summary>
        [DataMember(Name = "token")]
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
