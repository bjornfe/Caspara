using iCsys.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace iCsys.MessageHub.Client
{
    public class MessageHubClientService<T> : ThreadService, IMessageHubClientService<T> where T : IHubMessage
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
