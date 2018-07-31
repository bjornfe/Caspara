using System;
using System.Collections.Generic;
using System.Text;

namespace Caspara.Services.Messaging.Client
{
    public interface IMessageClientService<T> where T : IMessage
    {
        event EventHandler<T> MessageReceived;
        event EventHandler Disconnected;
        void SendMessage(T Message);
        void Subscribe(String Topic);
        void Unsubscribe(String Topic);
    }
}
