using System;
using System.Collections.Generic;
using System.Text;
using Caspara.DependencyInjection;
using Caspara.Repositories;
using Caspara.Serializing;

namespace Caspara.Repositories
{
    public class RepositoryPersistanceService : IRepositoryPersistanceService
    {
        public IRepositoryCollection Repositories { get; private set; }

        ISerializerService SerializerService;
        IDependencyInjectorService InjectorService;
        String Path = null;
        public RepositoryPersistanceService(ISerializerService SerializerService, IDependencyInjectorService InjectorService)
        {
            this.SerializerService = SerializerService;
            this.InjectorService = InjectorService;
        }

        public void Load<T>(string Path) where T : IRepositoryCollection
        {
            this.Path = Path;
            Repositories = SerializerService.Load<T>(SerializeType.XML, Path);
            InjectorService.Register(Repositories).As<IRepositoryCollection>();

        }

        public void Save()
        {
            SerializerService.Save(SerializeType.XML, Repositories, Path);
        }
    }
}
