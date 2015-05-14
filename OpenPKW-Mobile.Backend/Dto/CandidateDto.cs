using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OpenPKW_Mobile.Backend.Dto
{
    [DataContract]
    public class CandidateDto
    {
        [DataMember(Order = 1)]
        public string pkwId { get; set; }

        [DataMember(Order = 2)]
        public String firstname { get; set; }

        [DataMember(Order = 3)]
        public String lastname { get; set; }

        [DataMember(Order = 4)]
        public long votes { get; set; }
    }
}
