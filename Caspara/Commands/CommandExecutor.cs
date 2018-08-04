using System;
using System.Collections.Generic;
using System.Text;

namespace Caspara.Commands
{
    public abstract class CommandExecutor : ICommandExecutor
    {
        public abstract object CommandID { get; }

        ICommandClientService Client;

        public CommandExecutor(ICommandClientService Client)
        {
            this.Client = Client;
        }

        public virtual CommandResult Execute(String Target = "127.0.0.1", int Port = 65008)
        {

            var Command = new Command().SetID(CommandID);
            AddArguments(Command);
            return Client.ExecuteCommand(Target, Command, Port);
        }

        protected virtual void AddArguments(Command cmd)
        {

        }
    }
}
