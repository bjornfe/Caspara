using Caspara;
using Caspara.ConfigurationSets;
using Caspara.Messaging;
using Caspara.Persistance;
using Caspara.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Caspara.Messaging
{
    public class MessagePublishServer : Service
    {
        public override string Name { get; set; } = "Test Publisher";

        IMessagePublishServer Server;
        IPersistanceModel PersistanceModel;
        ConfigurationSet Configuration;
        public MessagePublishServer(IPersistanceModel PersistanceModel, IMessagePublishServer Server)
        {
            this.PersistanceModel = PersistanceModel;
            this.Server = Server;
            Configuration = PersistanceModel.Configurations.Get("TestPublisher");
        }

        public override void Start()
        {
            Server.PublishPort = Configuration.Get<int>("PublishPort");
            Server.SubscribePort = Configuration.Get<int>("SubscribePort");
            Server.Start();

            base.Start();
        }

        public override void Stop()
        {
            Configuration.Set("PublishPort", Server.PublishPort);
            Configuration.Set("SubscribePort", Server.SubscribePort);

            base.Stop();
            Server.Stop();
        }
        
    }
}
