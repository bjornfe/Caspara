using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Caspara.Values;

namespace Caspara.Commands
{
    [DataContract]
    public class CommandResult
    {
        [DataMember]
        public bool Success { get; private set; }

        [DataMember]
        public Value Result { get; set; }

        public CommandResult()
        {

        }

        public CommandResult(bool Success, object ResultValue)
        {
            this.Success = Success;
            Result = new Value(ResultValue);
        }
    }
}
