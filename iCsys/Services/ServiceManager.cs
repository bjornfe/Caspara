using System;
using System.Collections.Generic;
using System.Text;
using Caspara.DependencyInjection;

namespace Caspara.Services
{
    public class ServiceManager : IServiceManager
    {
        private readonly Dictionary<object, IService> _services = new Dictionary<object, IService>();
        private readonly object _lockObject = new object();
        public void AddService(IService service)
        {
            lock (_lockObject)
            {
                if (!_services.ContainsKey(service.ID))
                    _services[service.ID] = service;
            }
        }

        public IService GetService(object ID)
        {
            lock (_lockObject)
            {
                if (_services.ContainsKey(ID))
                    return _services[ID];
                else
                    return null;
            }
        }

        public void StartServices()
        {
            foreach (var s in _services.Values)
            {
                try
                {
                    s.Start();
                }
                catch(Exception err)
                {
                    Console.WriteLine("Failed to start service "+s.Name+" -> " + err.ToString());
                }
            }
        }

        public void StopServices()
        {
            foreach (var s in _services.Values)
            {
                try
                {
                    s.Stop();
                }
                catch (Exception err)
                {
                    Console.WriteLine("Failed to stop service " + s.Name + " -> " + err.ToString());
                }
            }
        }
    }
}
