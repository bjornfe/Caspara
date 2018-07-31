using Caspara.DependencyInjection;
using Caspara.Extensions;
using Caspara.Logging;
using Caspara.Repositories;
using Caspara.Serializing;
using Caspara.Services;
using System;
using System.Collections.Generic;

namespace Caspara
{
    public class Application : IApplication
    {
        public IDependencyInjectorService InjectorService { get; set; } = new DependencyInjectorService();
        private List<IConfigurationClass> ConfigurationClasses;
        public Application(List<IConfigurationClass> ConfigurationClasses)
        {
            this.ConfigurationClasses = ConfigurationClasses;
        }

        public void Configure(string Path = null)
        {
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

            if(!InjectorService.IsRegistered<IRepositoryPersistanceService>())
                InjectorService.Register<RepositoryPersistanceService>().As<IRepositoryPersistanceService>().AsSingleton();

            var extensionManager = InjectorService.Resolve<IExtensionManager>();
            extensionManager.LocateExtensions();

            var persistanceService = InjectorService.Resolve<IRepositoryPersistanceService>();
            persistanceService.Load<RepositoryCollection>(Path);

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

            var persistanceService = InjectorService.Resolve<IRepositoryPersistanceService>();
            persistanceService.Save();


        }

        
    }
}
