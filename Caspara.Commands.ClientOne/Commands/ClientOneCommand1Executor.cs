using System;
using System.Collections.Generic;
using System.Text;

namespace Caspara.Commands.ClientOne.Commands
{
    public class ClientOneCommand1Executor : CommandExecutor
    {

        public override object CommandID => 100;

        ICommandClientService Client;
        public ClientOneCommand1Executor(ICommandClientService Client) : base(Client)
        {
            this.Client = Client;
        }

        

        
    }
}
