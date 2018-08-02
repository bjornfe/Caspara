using Caspara.Messaging;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Caspara.Observations
{
    [DataContract]
    public class ObservationMessage : IMessage
    {
        [DataMember]
        public string Sender { get; set; }
        [DataMember]
        public string Topic { get; set; }
        [DataMember]
        public Observation Observation { get; set; }
    }
}
