using Caspara.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Caspara.Services.Messaging.Client
{
    public class MessageClientService<T> : ThreadService, IMessageClientService<T> where T : IMessage
    {
        public override string Name { get; set; } 

        public event EventHandler<T> MessageReceived;
        public event EventHandler Disconnected;

        public override void PerformAction()
        {
            throw new NotImplementedException();
        }

        public void SendMessage(T Message)
        {
            throw new NotImplementedException();
        }

        public void Subscribe(string Topic)
        {
            throw new NotImplementedException();
        }

        public void Unsubscribe(string Topic)
        {
            throw new NotImplementedException();
        }
    }
}
