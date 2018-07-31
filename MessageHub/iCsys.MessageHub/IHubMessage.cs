using System;
using System.Collections.Generic;
using System.Text;

namespace iCsys.MessageHub
{
    public interface IHubMessage
    {
        HubMessageType MessageType { get; set; }
        String Topic { get; set; }
    }
}
