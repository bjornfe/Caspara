using Caspara.Messaging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Caspara.Observations
{
    public interface IObservationMessageService
    {
        event EventHandler<ObservationMessage> MessageReceived;
        void Subscribe(String Topic);
        void Unsubscribe(String Topic);
        void Publish(ObservationMessage Message);
        void Publish(Observation Observation);
    }
}
