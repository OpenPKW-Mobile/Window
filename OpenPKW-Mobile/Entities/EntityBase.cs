using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace OpenPKW_Mobile.Entities
{
    [DataContract]
    public abstract class EntityBase
    {
        public abstract string Identifier { get; }
    }
}
