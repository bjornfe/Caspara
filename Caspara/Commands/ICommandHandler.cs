using System;
using System.Collections.Generic;
using System.Text;

namespace Caspara.Commands
{
    public interface ICommandHandler
    {
        object CommandID { get; }
        CommandResult Execute(Command command);
    }
}
