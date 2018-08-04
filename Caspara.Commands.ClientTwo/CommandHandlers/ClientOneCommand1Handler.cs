using System;
using System.Collections.Generic;
using System.Text;

namespace Caspara.Commands.ClientTwo.CommandHandlers
{
    public class ClientOneCommand1Handler : ICommandHandler
    {
        public object CommandID => 100;

        public CommandResult Execute(Command command)
        {
            Console.WriteLine("ClientOneCommand1 Executed");
            return new CommandResult(true, "OK");
        }
    }
}
