using System;
using System.Collections.Generic;
using System.Text;

namespace Caspara.Commands
{
    public interface ICommandExecutor
    {
        CommandResult Execute();
    }
}
