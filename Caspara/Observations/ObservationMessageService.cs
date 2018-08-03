using Caspara;
using Caspara.ConfigurationSets;
using Caspara.Messaging;
using Caspara.Observations;
using Caspara.Persistance;
using Caspara.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Caspara.Observations
{
    public class ObservationMessageService : Service, IObservationMessageService
    {
        public override string Name { get; set; } = "Observation Message Client";

        IPersistanceModel PersistanceModel;
        IMessagePublishClient Client;
        ConfigurationSet ConfigurationSet;

        public event EventHandler<Observations.ObservationMessage> MessageReceived;

        public ObservationMessageService(IPersistanceModel PersistanceModel, IMessagePublishClient Client)
        {
            this.PersistanceModel = PersistanceModel;
            this.Client = Client;
            ConfigurationSet = PersistanceModel.Configurations.Get("ObservationMessageClient");
        }

        public override void Start()
        {
            Client.Hostname = ConfigurationSet.Get<string>("Hostname");
            Client.SubscribePort = ConfigurationSet.Get<int>("SubscribePort");
            Client.PublishPort = ConfigurationSet.Get<int>("PublishPort");
            Client.MessageReceived += Subscriber_MessageReceived;
            Client.Start();

            base.Start();
        }

        private void Subscriber_MessageReceived(object sender, string e)
        {
            Observations.ObservationMessage om = JsonConvert.DeserializeObject<Observations.ObservationMessage>(e);
            if (om != null)
                MessageReceived?.Invoke(this, om);
        }

        public override void Stop()
        {
            ConfigurationSet.Set("Hostname", Client.Hostname);
            ConfigurationSet.Set("SubscribePort", Client.SubscribePort);
            ConfigurationSet.Set("PublishPort", Client.PublishPort);
            Client.Stop();
            base.Stop();
        }

        public void Subscribe(string Topic)
        {
            Client.Subscribe(Topic);
        }

        public void Unsubscribe(string Topic)
        {
            Client.Unsubscribe(Topic);
        }

        public void Publish(Observations.ObservationMessage Message)
        {
            var msg = JsonConvert.SerializeObject(Message);
            Client.Publish(Message.Topic, msg);
        }

        public void Publish(Observation Observation)
        {
            var message = new Observations.ObservationMessage()
            {
                Sender = Caspara.DeviceName,
                Topic = Caspara.DeviceName + ".OBSERVATION." + Observation.ID,
                Observation = Observation
            };

            var msg = JsonConvert.SerializeObject(message);
            Client.Publish(message.Topic, msg);
        }
    }
}
