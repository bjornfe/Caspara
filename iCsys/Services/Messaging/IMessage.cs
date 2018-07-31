using System;
using System.Collections.Generic;
using System.Text;

namespace Caspara.Services.Messaging
{
    public interface IMessage
    {
        MessageType MessageType { get; set; }
        String Topic { get; set; }
    }
}
