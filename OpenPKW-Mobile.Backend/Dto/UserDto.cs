using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OpenPKW_Mobile.Backend.Dto
{
    [DataContract]
    public class UserDto
    {
        [DataMember(Order = 1)]
        public int id { get; set; }

        [DataMember(Order = 2)]
        public String fullname { get; set; }

        [DataMember(Order = 3)]
        public String firstname { get; set; }

        [DataMember(Order = 4)]
        public String lastname { get; set; }

        [DataMember(Order = 5)]
        public String login { get; set; }

        [DataMember(Order = 6)]
        public String token { get; set; }

        [DataMember(Order = 7)]
        public bool sessionActive { get; set; }

        [DataMember(Order = 8)]
        public String sessionTimeout { get; set; }
    }
}
