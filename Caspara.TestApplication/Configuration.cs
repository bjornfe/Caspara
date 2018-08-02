using Caspara.Configurations;
using Caspara.ObservationMessage.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace Caspara.TestClient
{
    public class Configuration : IConfigurationClass
    {
        public void Configure(IApplication app)
        {
            app.IncludeObservationMessageClient();
        }

        public void Start(IApplication app)
        {
            
        }

        public void Stop(IApplication app)
        {
            
        }
    }
}
