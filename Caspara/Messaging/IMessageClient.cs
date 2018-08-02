using System;
using System.Collections.Generic;
using System.Text;

namespace Caspara.Messaging
{
    public interface IMessageClient
    {
        event EventHandler<String> MessageReceived;
        string Hostname { get; set; }
        int SubscribePort { get; set; }
        int PublishPort { get; set; }
        IMessageClient Publish(String Topic, String Message);
        IMessageClient Subscribe(String Topic);
        IMessageClient Unsubscribe(String Topic);
        IMessageClient Start();
        IMessageClient Stop();
    }
}
