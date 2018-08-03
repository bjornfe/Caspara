using System;
using System.Collections.Generic;
using System.Text;

namespace Caspara.Messaging
{
    public interface IMessagePublishClient
    {
        event EventHandler<String> MessageReceived;
        string Hostname { get; set; }
        int SubscribePort { get; set; }
        int PublishPort { get; set; }
        IMessagePublishClient Publish(String Topic, String Message);
        IMessagePublishClient Subscribe(String Topic);
        IMessagePublishClient Unsubscribe(String Topic);
        IMessagePublishClient Start();
        IMessagePublishClient Stop();
    }
}
