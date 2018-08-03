using System;
using System.Collections.Generic;
using System.Text;

namespace Caspara.Commands
{
    public interface ICommandResponseService
    {
        CommandResponse GetResponse(String CommandString);
    }
}
