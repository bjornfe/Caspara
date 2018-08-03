using Caspara.ConfigurationSets;
using Caspara.Messaging;
using Caspara.Persistance;
using Caspara.Serializing;
using Caspara.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Caspara.Commands
{
    public class CommandClientService : Service, ICommandClientService
    {
        public override string Name { get; set; } = "Command Client Service";

        IPersistanceModel PersistanceModel;
        IMessagePublishClient Client;
        ISerializer Serializer;
        ConfigurationSet Configuration;
        String ResponseTopic = Guid.NewGuid().ToString();
        bool MessageReceived = false;
        string ReceivedMessageText = "";

        public CommandClientService(IPersistanceModel PersistanceModel, IMessagePublishClient Client, ISerializer Serializer)
        {
            this.PersistanceModel = PersistanceModel;
            this.Client = Client;
            this.Serializer = Serializer;
            Configuration = PersistanceModel.Configurations.Get("ObservationMessageClient");
        }

        public override void Start()
        {
            Client.Hostname = Configuration.Get<string>("Hostname");
            Client.SubscribePort = Configuration.Get<int>("SubscribePort");
            Client.PublishPort = Configuration.Get<int>("PublishPort");
            Client.MessageReceived += Subscriber_MessageReceived;
            Client.Subscribe(ResponseTopic);
            Client.Start();

            base.Start();
        }

        private void Subscriber_MessageReceived(object sender, string e)
        {
            MessageReceived = true;
            ReceivedMessageText = e;
        }

        public override void Stop()
        {
            Configuration.Set("Hostname", Client.Hostname);
            Configuration.Set("SubscribePort", Client.SubscribePort);
            Configuration.Set("PublishPort", Client.PublishPort);
            Client.Stop();
            base.Stop();
        }

        public CommandResult ExecuteCommand(String Target, Command Command, int Port = 0)
        {
            var message = new CommandMessage()
            {
                Sender = Caspara.DeviceName,
                RespondTo = ResponseTopic,
                Command = Command
            };

            string commandString = Serializer.Serialize(message, SerializeType.JSON);
            MessageReceived = false;
            ReceivedMessageText = null;
            Client.Publish(Target + ".EXECUTE.COMMAND", commandString);

            bool timeout = false;

            while(!MessageReceived && !timeout)
            {
                timeout = (DateTime.Now - Command.TimeStamp).TotalSeconds < Command.TimeOutSeconds;
                Thread.Sleep(1);
            }

            if (!timeout)
            {
                var response = Serializer.Deserialize<CommandResult>(ReceivedMessageText, SerializeType.JSON);
                return response;
            }

            return null;
        }
    }
}
