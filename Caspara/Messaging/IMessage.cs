using System;
using System.Collections.Generic;
using System.Text;

namespace Caspara.Messaging
{
    public interface IMessage
    {
        string Sender { get; set; }
        string Topic { get; set; }
    }
}
