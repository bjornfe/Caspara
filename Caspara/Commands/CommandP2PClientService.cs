using Caspara.Messaging;
using Caspara.Serializing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Caspara.Commands
{
    public class CommandP2PClientService : ICommandClientService
    {
        private IMessageP2PClient MessageClient;
        private ISerializer Serializer;
        private String LastHost = "";
        private int lastPort = 65008;
        public CommandP2PClientService(IMessageP2PClient MessageClient, ISerializer Serializer)
        {
            this.MessageClient = MessageClient;
            this.Serializer = Serializer;
        }

        private readonly object _lockObject = new object();
        public CommandResult ExecuteCommand(String Target, Command Command, int Port = 0)
        {
            lock (_lockObject)
            {
                if (Command != null)
                {
                    if (Target != null)
                        LastHost = Target;
                    else
                        Target = LastHost;

                    if (Port != 0)
                        lastPort = Port;
                    else
                        Port = lastPort;

                    string commandString = Serializer.Serialize(Command, SerializeType.JSON);
                    var responseString = MessageClient.GetResponse(Target, Port, commandString, Command.TimeOutSeconds);
                    
                    if (responseString != null)
                    {
                        var response = Serializer.Deserialize<CommandResult>(responseString, SerializeType.JSON);
                        return response;
                    }
                }
                return null;
            }
        }
    }
}
