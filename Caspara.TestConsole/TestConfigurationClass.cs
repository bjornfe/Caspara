using Caspara.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Caspara.TestConsole
{
    public class TestConfigurationClass : IConfigurationClass
    {
        public void Configure(IApplication app)
        {
            app.InjectorService.Register<TestThreadService>().AsIncludedInterfaces().AsSingleton();
            app.InjectorService.Register<TestConsoleLogger>().As<IConsoleLogger>().AsSingleton();
        }

        public void Start(IApplication app)
        {
            Console.WriteLine("Starting in TestConfigurationClass");
        }

        public void Stop(IApplication app)
        {
            Console.WriteLine("Stopping in TestConfigurationClass");
        }
    }
}
