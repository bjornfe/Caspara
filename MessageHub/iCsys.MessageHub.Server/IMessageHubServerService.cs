using iCsys.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace iCsys.MessageHub.Server
{
    public interface IMessageHubServerService<T> : IThreadService where T : IHubMessage
    {

    }
}
