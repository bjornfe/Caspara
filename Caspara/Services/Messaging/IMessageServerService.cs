using Caspara.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Caspara.Services.Messaging.Server
{
    public interface IMessageServerService<T> : IThreadService where T : IMessage
    {

    }
}
