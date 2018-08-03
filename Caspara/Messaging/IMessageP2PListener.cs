using System;
using System.Collections.Generic;
using System.Text;

namespace Caspara.Messaging
{
    public interface IMessageP2PListener
    {
        event Func<string, string> HandleMessage;
        int ListenPort { get; set; }
        void Start();
        void Stop();
    }
}
