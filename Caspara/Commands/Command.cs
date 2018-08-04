using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Caspara.Values;

namespace Caspara.Commands
{
    [DataContract]
    public class Command : ObjectBase
    {
        [DataMember]
        public object CommandID { get; set; }
        [DataMember]
        public DateTime TimeStamp { get; set; } = DateTime.UtcNow;
        [DataMember]
        public Dictionary<int, Value> Arguments { get; set; }
        [DataMember]
        public int TimeOutSeconds { get; set; } = 10;

        public Command()
        {
            Arguments = new Dictionary<int, Value>();
        }

        public Command(object CommandID, int ValidSeconds = 10)
        {
            this.CommandID = CommandID;
            this.TimeStamp = DateTime.UtcNow;
            this.TimeOutSeconds = ValidSeconds;
            Arguments = new Dictionary<int, Value>();
        }

        public Command SetArgument(int ArgumentID, Value Argument)
        {
            Arguments[ArgumentID] = Argument;
            return this;
        }

        public Command SetID(object ID)
        {
            this.CommandID = ID;
            return this;
        }

        public Command SetTimeStamp(DateTime time)
        {
            this.TimeStamp = time;
            return this;
        }

        public Command SetTimeOutSeconds(int TimeOutSeconds)
        {
            this.TimeOutSeconds = TimeOutSeconds;
            return this;
        }
    }
}
