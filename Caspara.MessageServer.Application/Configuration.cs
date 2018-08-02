using Caspara.Configurations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Caspara.MessageServer.Application
{
    public class Configuration : IConfigurationClass
    {
        public void Configure(IApplication app)
        {
            app.UseMessageServer();
        }

        public void Start(IApplication app)
        {
            
        }

        public void Stop(IApplication app)
        {
            
        }
    }
}
