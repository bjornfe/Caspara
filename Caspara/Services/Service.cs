using Caspara.Configurations;
using Caspara.Persistance;
using System;
using System.Collections.Generic;
using System.Text;

namespace Caspara.Services
{
    public abstract class Service : IService
    {
        public virtual object ID { get; set; } = Guid.NewGuid();
        public abstract string Name { get; set; }
        public bool Enabled { get; set; } = true;
        public bool IsRunning { get; protected set; }

        //protected virtual object ConfigurationID => this.GetType().Name;
        //protected IPersistanceModel PersistanceModel;
        //protected ConfigurationSet ConfigurationSet;

        //public Service(IPersistanceModel PersistanceModel)
        //{
        //    this.PersistanceModel = PersistanceModel;
        //    ConfigurationSet = PersistanceModel.Configurations.Get(ConfigurationID);
        //}

        public virtual void Start()
        {
            IsRunning = true;
        }

        public virtual void Stop()
        {
            IsRunning = false;
        }
    }
}
