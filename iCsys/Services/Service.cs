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
