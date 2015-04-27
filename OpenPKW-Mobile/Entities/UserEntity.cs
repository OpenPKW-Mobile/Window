using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenPKW_Mobile.Entities
{
    class UserEntity : EntityBase
    {
        public string UserID { get; set; }
        public string AuthenticationToken { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public override string Identifier
        {
            get 
            {
                return UserID;
            }
        }
    }
}
