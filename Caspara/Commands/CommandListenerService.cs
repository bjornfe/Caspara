using Caspara.ConfigurationSets;
using Caspara.Messaging;
using Caspara.Persistance;
using Caspara.Serializing;
using Caspara.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Caspara.Commands
{
    public class CommandListenerService : Service, ICommandListenerService
    {
        public override string Name { get; set; } = "Command Listener Service";

        IPersistanceModel PersistanceModel;
        IMessagePublishClient Client;
        ICommandResponseService ResponseService;
        ConfigurationSet Configuration;

        public CommandListenerService(IPersistanceModel PersistanceModel, IMessagePublishClient Client, ICommandResponseService ResponseService)
        {
            this.Client = Client;
            this.ResponseService = ResponseService;
            this.PersistanceModel = PersistanceModel;
            Configuration = PersistanceModel.Configurations.Get("CommandListenerService");
        }

        public override void Start()
        {
            Client.Hostname = Configuration.Get<string>("Hostname");
            Client.SubscribePort = Configuration.Get<int>("SubscribePort");
            Client.PublishPort = Configuration.Get<int>("PublishPort");
            Client.MessageReceived += Client_MessageReceived;
            Client.Subscribe(Caspara.DeviceName + ".EXECUTE.COMMAND");

            base.Start();
        }

        private void Client_MessageReceived(object sender, string e)
        {
            var response = ResponseService.GetResponse(e);
            Client.Publish(response.RespondTo, response.Message);
        }

        public override void Stop()
        {
            Configuration.Set("Hostname", Client.Hostname);
            Configuration.Set("SubscribePort", Client.SubscribePort);
            Configuration.Set("PublishPort", Client.PublishPort);
            Client.Stop();
            base.Stop();
        }
    }
}
