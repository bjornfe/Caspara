using System;
using System.Collections.Generic;
using System.Text;

namespace Caspara.Messaging
{
    public interface IMessageP2PClient
    {
        string GetResponse(String IP, int Port, string Message, int TimeOutSeconds = 10);
    }
}
