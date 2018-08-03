using Caspara.Observations;
using Caspara.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Caspara.Observations.ClientOne
{
    public class ObservationSubscriberService : Service
    {
        public override string Name { get; set; } = "Observation Subscriber Service";

        IObservationMessageService MessageService;
        public ObservationSubscriberService(IObservationMessageService MessageService)
        {
            this.MessageService = MessageService;
            MessageService.MessageReceived += MessageService_MessageReceived;
            MessageService.Subscribe("ObservationClient2.OBSERVATION.101");
        }

        private void MessageService_MessageReceived(object sender, Observations.ObservationMessage e)
        {
            Console.WriteLine(Caspara.DeviceName + " Received -> " + e.Observation.ID + "-" + e.Observation.Name + "-" + e.Observation.Value.GetValue());
        }
    }
}
