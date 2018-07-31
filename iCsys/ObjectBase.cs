using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Caspara
{
    [DataContract]
    public abstract class ObjectBase : IObject
    {
        [DataMember]
        public object ID { get; set; }

        [DataMember]
        public string Name { get; set; }
    }
}
