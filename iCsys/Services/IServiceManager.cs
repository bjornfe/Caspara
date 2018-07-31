using Caspara.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Caspara.Services
{
    public interface IServiceManager
    {
        void AddService(IService service);
        IService GetService(object ID);
        void StartServices();
        void StopServices();
    }
}
