using Caspara.Values;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Caspara.Observations
{
    [DataContract]
    public class Observation : ObjectBase, IObservation
    {
        [DataMember]
        public IValue Value { get; set; } = new Value();
    }
}
