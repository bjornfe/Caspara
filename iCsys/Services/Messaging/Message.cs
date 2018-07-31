using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Caspara.Services.Messaging
{
    public class Message<T> : IMessage
    {
        [DataMember]
        public MessageType MessageType { get; set; }

        [DataMember]
        public String Topic { get; set; }

        [DataMember]
        public T Content { get; set; }
    }
}
