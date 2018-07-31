using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Caspara.DependencyInjection
{
    public class DependencyInjectorService : IDependencyInjectorService
    {
        private List<IDependencyInjectorEntry> _services = new List<IDependencyInjectorEntry>();
        private readonly object _lockObject = new object();

        public DependencyInjectorService()
        {
        }

        public IDependencyInjectorEntry Register<T>()
        {
            return Register(typeof(T));
        }

        public IDependencyInjectorEntry Register(Type t)
        {
            lock (_lockObject)
            {
                var entry = new DependencyInjectionEntry(this, t);
                _services.Add(entry);
                return entry;
            }
        }

        public IDependencyInjectorEntry Register(object obj)
        {
            lock (_lockObject)
            {
                var entry = new DependencyInjectionEntry(this, obj.GetType()).AsInstance(obj);
                _services.Add(entry);
                return entry;
            }
        }

        public IDependencyInjectorEntry RegisterOrReplace<T>(object Key = null)
        {
            lock (_lockObject)
            {
                if (_services.Any(s => s.IsRegisteredAs(typeof(T)) && (Key == null || s.HasKey(Key))))
                {
                    return _services.Where(s => s.IsRegisteredAs(typeof(T)) && (Key == null || s.HasKey(Key))).First();
                }
                else
                    return Register<T>();
            }
        }

        public T Resolve<T>(object Key = null)
        {
            var service = Resolve(typeof(T), Key);
            if (service != null && service is T obj)
                return obj;
            else
                return default(T);
        }

        public List<T> ResolveAll<T>(DependencyInjectorEntryType type = DependencyInjectorEntryType.NONE)
        {
            lock (_lockObject)
            {
                List<T> Entries = new List<T>();

                foreach (var s in _services.Where(s => s.IsRegisteredAs(typeof(T)) && (type == DependencyInjectorEntryType.NONE || s.IsEntryType(type))))
                {
                    var service = s.GetEntry<T>();
                    if (!service.Equals(default(T)))
                        Entries.Add(service);
                }

                return Entries;
            }
        }

        public object Resolve(Type t, object Key = null)
        {
            
            lock (_lockObject)
            {
                if (_services.Any(s => s.IsRegisteredAs(t) && (Key == null || s.HasKey(Key))))
                {
                    var service = _services.Where(s => s.IsRegisteredAs(t) && (Key == null || s.HasKey(Key))).First();
                    return service.GetEntry(t);
                }
                else
                {
                    foreach(var entry in _services)
                    {

                    }
                        
                }
                    return null;

            }
        }

        public void RegisterAssemblyTypes<T>(Assembly asm, bool IncludeInterfaces = true)
        {
            foreach (var t in asm.GetTypes().Where(t => !t.IsAbstract && !t.IsInterface))
            {
                if (t.GetInterfaces().Any(i => (i == typeof(T))))
                {
                    var entry = Register(t).As<T>().AsSingleton();
                    if(IncludeInterfaces)
                        entry.AsIncludedInterfaces();
                }
            }
        }

        public void RegisterAssemblyTypesWithStaticFieldAsKey<T>(Assembly asm, string IDFieldName, Type IDFieldType)
        {
            foreach (var t in asm.GetTypes().Where(t => !t.IsAbstract && !t.IsInterface))
            {

                if (t.GetInterfaces().Any(i => (i == typeof(T) && !t.IsInterface)))
                {
                    var fieldInfos = t.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                            .Where(fi => fi.IsLiteral && !fi.IsInitOnly && fi.FieldType == IDFieldType && fi.Name != null && fi.Name.Equals(IDFieldName)).ToList();

                    if (fieldInfos != null && fieldInfos.Count == 1)
                    {
                        Register(t).As<T>().AsSingleton().WithKey(fieldInfos[0].GetRawConstantValue());
                    }
                }

            }

        }

        public int GetRegistrationCount()
        {
            lock (_lockObject)
            {
                return _services.Count();
            }
        }
    }
}
