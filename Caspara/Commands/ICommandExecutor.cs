using System;
using System.Collections.Generic;
using System.Text;

namespace Caspara.Commands
{
    public interface ICommandExecutor
    {
        object CommandID { get; }
        CommandResult Execute(String OverrideTarget = null, int OverridePort = 0);
    }
}
