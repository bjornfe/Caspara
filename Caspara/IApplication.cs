using Caspara.DependencyInjection;
using Caspara.Extensions;
using Caspara.Serializing;
using Caspara.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Caspara
{
    public interface IApplication
    {
        IDependencyInjectorService InjectorService { get; set; }
        void Configure(String Path = null);
        void Start();
        void Stop();
        
    }
}
