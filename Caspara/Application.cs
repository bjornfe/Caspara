using Caspara;
using Caspara.DependencyInjection;
using Caspara.Extensions;
using Caspara.Logging;
using Caspara.Persistance;
using Caspara.Repositories;
using Caspara.Serializing;
using Caspara.Services;
using System;
using System.Collections.Generic;
using System.IO;

namespace Caspara
{
    public class Application : IApplication
    {
        public IDependencyInjectorService InjectorService { get; set; } = new DependencyInjectorService();

        public string ApplicationPath { get; private set; }

        private List<IConfigurationClass> ConfigurationClasses;
        public Application(List<IConfigurationClass> ConfigurationClasses)
        {
            this.ConfigurationClasses = ConfigurationClasses;
        }

        public void Configure(string BasePath = null)
        {
            Console.WriteLine("#################################################################################################");
            var lines = File.ReadAllLines("Title.txt");
            foreach (var l in lines)
                Console.WriteLine(l);
            Console.WriteLine("#################################################################################################");
            Console.WriteLine("Device Name: " + Caspara.DeviceName);
            Console.WriteLine("Platform Type: " + Environment.OSVersion.Platform.ToString());
            Console.WriteLine("OS Version: " + Environment.OSVersion.VersionString);
            Console.WriteLine("64-Bit: " + Environment.Is64BitProcess);
            Console.WriteLine("#################################################################################################");

            foreach (var c in ConfigurationClasses)
                c.Configure(this);

            if(!InjectorService.IsRegistered<IConsoleLogger>())
                InjectorService.Register<ConsoleTextLogger>().As<IConsoleLogger>().AsSingleton();

            if(!InjectorService.IsRegistered<IExtensionManager>())
                InjectorService.Register<ExtensionManager>().As<IExtensionManager>().AsSingleton();

            if(!InjectorService.IsRegistered<IServiceManager>())
                InjectorService.Register<ServiceManager>().As<IServiceManager>().AsSingleton();

            if(!InjectorService.IsRegistered<ISerializer>())
                InjectorService.Register<Serializer>().As<ISerializer>();

            if (!InjectorService.IsRegistered<ISerializerService>())
                InjectorService.Register<LocalSerializerService>().As<ISerializerService>().AsSingleton();

            if(!InjectorService.IsRegistered<IPersistanceService>())
                InjectorService.Register<PersistanceService>().As<IPersistanceService>().AsSingleton();

            var extensionManager = InjectorService.Resolve<IExtensionManager>();

            extensionManager.LocateExtensions();
            if (BasePath == null)
                BasePath = extensionManager.GetApplicationPath();

            ApplicationPath = extensionManager.GetApplicationPath();

            var persistanceService = InjectorService.Resolve<IPersistanceService>();
            persistanceService.Load<PersistanceModel>(Path.Combine(BasePath, "PersistanceModel.xml"));

            extensionManager.LoadExtensions(this);
        }

        public void Start()
        {
            foreach (var c in ConfigurationClasses)
                c.Start(this);

            var serviceManager = InjectorService.Resolve<IServiceManager>();

            foreach (var s in InjectorService.ResolveAll<IService>())
                serviceManager.AddService(s);

            serviceManager.StartServices();
        }

        public void Stop()
        {
            foreach (var c in ConfigurationClasses)
                c.Stop(this);

            var serviceManager = InjectorService.Resolve<IServiceManager>();
            serviceManager.StopServices();

            var persistanceService = InjectorService.Resolve<IPersistanceService>();
            persistanceService.Save();


        }

        
    }
}
