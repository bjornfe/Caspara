using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace iCsys.MessageHub
{
    public class HubMessage<T> : IHubMessage
    {
        [DataMember]
        public HubMessageType MessageType { get; set; }

        [DataMember]
        public String Topic { get; set; }

        [DataMember]
        public T Content { get; set; }
    }
}
