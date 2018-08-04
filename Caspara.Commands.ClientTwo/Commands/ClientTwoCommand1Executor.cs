using System;
using System.Collections.Generic;
using System.Text;

namespace Caspara.Commands.ClientTwo.Commands
{
    public class ClientTwoCommand1Executor : CommandExecutor
    {
        public override object CommandID => 100;

        public ClientTwoCommand1Executor(ICommandClientService Client) : base(Client)
        {

        }

        
    }
}
