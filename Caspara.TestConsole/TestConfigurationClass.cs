using Caspara.Logging;
using Caspara.Services;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Caspara.TestConsole
{
    public class TestConfigurationClass : IConfigurationClass
    {
        public void Configure(IApplication app)
        {
            //app.InjectorService.Register<TestThreadService>().AsIncludedInterfaces().AsSingleton();
            app.InjectorService.Register<TestConsoleLogger>().As<IConsoleLogger>().AsSingleton();
            app.InjectorService.RegisterAssemblyTypes<IService>(Assembly.GetAssembly(GetType()));
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
