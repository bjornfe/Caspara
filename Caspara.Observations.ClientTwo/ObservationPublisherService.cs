using Caspara.Observations;
using Caspara.Services;
using Caspara.Values;
using System;
using System.Collections.Generic;
using System.Text;

namespace Caspara.Observations.ClientTwo
{
    public class ObservationPublisherService : ThreadService
    {
        public override string Name { get; set; } = "Observation Publisher Service";

        IObservationMessageService MessageService;

        public ObservationPublisherService(IObservationMessageService MessageService)
        {
            this.MessageService = MessageService;
            SleepInterval = 20;
        }

        Random rnd = new Random();
        public override void PerformAction()
        {
            var value = (rnd.NextDouble() * 1000);
            var obs = new Observation()
            {
                ID = 101,
                Name = "Pressure",
                Value = new Value(value)
            };

            MessageService.Publish(obs);
        }
    }
}
