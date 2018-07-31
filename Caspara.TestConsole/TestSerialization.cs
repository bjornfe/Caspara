using Caspara.Logging;
using Caspara.Observations;
using Caspara.Serializing;
using Caspara.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Caspara.TestConsole
{
    public class TestSerialization : Service
    {
        public override string Name { get; set; } = "Test Serialization";

        ISerializer Serializer;
        IConsoleLogger ConsoleLogger;
        public TestSerialization(ISerializer Serializer, IConsoleLogger ConsoleLogger)
        {
            this.Serializer = Serializer;
            this.ConsoleLogger = ConsoleLogger;
        }

        public override void Start()
        {
            base.Start();

            var observation = new Observation();
            observation.Value.SetValue("Test");

            var text = Serializer.Serialize(observation, SerializeType.JSON);

            var newObservation = Serializer.Deserialize<Observation>(text, SerializeType.JSON);

            ConsoleLogger.LogText(newObservation.Value.GetValue().ToString());

            


        }
    }
}
