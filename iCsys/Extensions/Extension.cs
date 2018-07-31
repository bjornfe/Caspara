using System;
using System.Collections.Generic;
using System.Text;
using Caspara.DependencyInjection;

namespace Caspara.Extensions
{
    public abstract class Extension : IExtension
    {
        public abstract string Name { get; }

        public virtual void Load(IApplication app)
        {
            
        }

        public abstract void Register(IApplication app);
       
        public virtual void Save(IApplication app)
        {
            
        }
    }
}
