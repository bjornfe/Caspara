using System;
using System.Collections.Generic;
using System.Text;

namespace Caspara.Commands
{
    public interface ICommandClientService
    {
        CommandResult ExecuteCommand(String Target, Command Command, int Port = 0);
    }
}
