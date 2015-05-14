using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OpenPKW_Mobile.Backend.Dto
{
    [DataContract]
    public class DistrictDto
    {
        [DataMember(Order = 1)]
        public String pkwId { get; set; }

        [DataMember(Order = 2)]
        public String name { get; set; }

        [DataMember(Order = 3)]
        public String address { get; set; }
    }
}
