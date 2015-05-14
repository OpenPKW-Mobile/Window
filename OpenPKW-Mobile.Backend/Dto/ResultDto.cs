using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OpenPKW_Mobile.Backend.Dto
{
    [DataContract]
    public class ResultDto
    {
        [DataMember(Order = 1)]
        public int uprawnionych { get; set; }

        [DataMember(Order = 2)]
        public int glosujacych { get; set; }

        [DataMember(Order = 3)]
        public int kartWaznych { get; set; }

        [DataMember(Order = 4)]
        public int glosowNieWaznych { get; set; }

        [DataMember(Order = 5)]
        public int glosowWaznych { get; set; }

        [DataMember(Order = 6)]
        public int k1 { get; set; }

        [DataMember(Order = 7)]
        public int k2 { get; set; }

        [DataMember(Order = 8)]
        public int k3 { get; set; }

        [DataMember(Order = 9)]
        public int k4 { get; set; }

        [DataMember(Order = 10)]
        public int k5 { get; set; }

        [DataMember(Order = 11)]
        public int k6 { get; set; }

        [DataMember(Order = 12)]
        public int k7 { get; set; }

        [DataMember(Order = 13)]
        public int k8 { get; set; }

        [DataMember(Order = 14)]
        public int k9 { get; set; }

        [DataMember(Order = 15)]
        public int k10 { get; set; }

        [DataMember(Order = 16)]
        public int k11 { get; set; }

    }
}
