using System;
using System.Collections.Generic;
using System.Text;
using Caspara.DependencyInjection;
using Caspara.Repositories;
using Caspara.Serializing;

namespace Caspara.Persistance
{
    public class PersistanceService : IPersistanceService
    {
        public IPersistanceModel Model { get; private set; }

        ISerializerService SerializerService;
        IDependencyInjectorService InjectorService;
        String Path = null;
        public PersistanceService(ISerializerService SerializerService, IDependencyInjectorService InjectorService)
        {
            this.SerializerService = SerializerService;
            this.InjectorService = InjectorService;
        }

        public void Load<T>(string Path) where T : IPersistanceModel
        {
            this.Path = Path;
            Model = SerializerService.Load<T>(SerializeType.XML, Path);
            InjectorService.Register(Model).As<IPersistanceModel>();
        }

        public void Save()
        {
            SerializerService.Save(SerializeType.XML, Model, Path);
        }
    }
}
