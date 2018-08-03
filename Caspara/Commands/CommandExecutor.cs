using System;
using System.Collections.Generic;
using System.Text;

namespace Caspara.Commands
{
    public abstract class CommandExecutor : ICommandExecutor
    {
        public abstract object CommandID { get; }
        public abstract String Target { get; }
        public virtual int Port { get; } = 0;

        ICommandClientService Client;

        public CommandExecutor(ICommandClientService Client)
        {
            this.Client = Client;
        }

        public virtual CommandResult Execute()
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
