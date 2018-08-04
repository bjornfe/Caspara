using System;
using System.Collections.Generic;
using System.Text;

namespace Caspara.Commands.ClientOne.CommandHandlers
{
    public class ClientTwoCommand1Handler : ICommandHandler
    {
        public object CommandID => throw new NotImplementedException();

        public CommandResult Execute(Command command)
        {
            Console.WriteLine("ClientTwoCommand Executed");
            return new CommandResult(true, "OK");
        }
    }
}
