using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OpenPKW_Mobile.Backend.Dto
{
    [DataContract]
    public class CommissionDto
    {
        [DataMember(Order = 1)]
        public String pkwId;

        [DataMember(Order = 2)]
        public String name;

        [DataMember(Order = 3)]
        public String address;

        [DataMember(Order = 4)]
        public DistrictDto district;

        [DataMember(Order = 5)]
        public List<CandidateDto> candidates;

        [DataMember(Order = 6)]
        public List<ResultDto> results;
    }
}
