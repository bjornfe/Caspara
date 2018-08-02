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
        public object ID { get; set; } = Guid.NewGuid();

        [DataMember]
        public string Name { get; set; } = "";
    }
}
