using System;
using System.Collections.Generic;
using System.Text;

namespace Caspara.Commands
{
    public interface ICommandHandler
    {
        ICommandResult Execute(ICommand command);
    }
}
