using Caspara.DependencyInjection;
using Caspara.Extensions;
using Caspara.Repositories;
using Caspara.Serializing;
using Caspara.Services;
using System;

namespace Caspara
{
    public class Application : IApplication
    {
        public IDependencyInjectorService InjectorService { get; set; } = new DependencyInjectorService();

        public void Load(string Path = null)
        {
           
            InjectorService.Register<ExtensionManager>().As<IExtensionManager>().AsSingleton();
            InjectorService.Register<ServiceManager>().As<IServiceManager>().AsSingleton();
            InjectorService.Register<Serializer>().As<ISerializer>();
            InjectorService.Register<LocalSerializerService>().As<ISerializerService>().AsSingleton();
            InjectorService.Register<RepositoryPersistanceService>().As<IRepositoryPersistanceService>().AsSingleton();

            var extensionManager = InjectorService.Resolve<IExtensionManager>();
            extensionManager.LocateExtensions();

            var persistanceService = InjectorService.Resolve<IRepositoryPersistanceService>();
            persistanceService.Load<RepositoryCollection>(Path);
            extensionManager.LoadExtensions(this);
        }

        public void Start()
        {
            var serviceManager = InjectorService.Resolve<IServiceManager>();

            foreach (var s in InjectorService.ResolveAll<IService>())
                serviceManager.AddService(s);

            serviceManager.StartServices();
        }

        public void Stop()
        {
            var serviceManager = InjectorService.Resolve<IServiceManager>();
            serviceManager.StopServices();

            var persistanceService = InjectorService.Resolve<IRepositoryPersistanceService>();
            persistanceService.Save();

        }
    }
}
