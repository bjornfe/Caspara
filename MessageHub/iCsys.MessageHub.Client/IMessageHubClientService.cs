using System;
using System.Collections.Generic;
using System.Text;

namespace iCsys.MessageHub.Client
{
    public interface IMessageHubClientService<T> where T : IHubMessage
    {
        event EventHandler<T> MessageReceived;
        event EventHandler Disconnected;
        void SendMessage(T Message);
        void Subscribe(String Topic);
        void Unsubscribe(String Topic);
    }
}
