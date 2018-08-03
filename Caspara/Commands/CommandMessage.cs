using Caspara.Messaging;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Caspara.Commands
{
    [DataContract]
    public class CommandMessage : IMessage
    {
        public string Sender { get; set; } = Caspara.DeviceName;
        public string Topic { get; set; }
        public string RespondTo { get; set; }
        public Command Command { get; set; }
    }
}
