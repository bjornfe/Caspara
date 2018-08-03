using System;
using System.Collections.Generic;
using System.Text;

namespace Caspara.Commands.ClientOne.Commands
{
    public class ClientOneCommand1 : CommandExecutor
    {

        public override object CommandID => 100;

        public override string Target => "UbuntuTest";

        ICommandClientService Client;
        public ClientOneCommand1(ICommandClientService Client) : base(Client)
        {
            this.Client = Client;
        }

        

        
    }
}
