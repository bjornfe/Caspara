using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Caspara.DependencyInjection
{
    public class DependencyInjectionEntry : IDependencyInjectorEntry
    {
        private List<Type> _registeredAs = new List<Type>();
        private object Instance;
        private DependencyInjectorEntryType resultType = DependencyInjectorEntryType.RESOLVABLE;
        private Lazy<object> Lazy;
        private Func<object> Function;
        private List<object> Keys = new List<object>();
        private IDependencyInjectorService InjectorService;

        public Type ResolveAsType { get; private set; }

        public void ReplaceResolveType(Type t)
        {
            bool replaceInRegister = false;
            if (_registeredAs.Contains(ResolveAsType))
            {
                _registeredAs.Remove(ResolveAsType);
                replaceInRegister = true;
            }
            ResolveAsType = t;
            if (replaceInRegister)
                As(ResolveAsType);
            
        }

        public DependencyInjectionEntry(IDependencyInjectorService InjectorService, Type ResolveAs)
        {
            this.InjectorService = InjectorService;
            this.ResolveAsType = ResolveAs;
            As(ResolveAs);
        }

        public IDependencyInjectorEntry AsIncludedInterfaces()
        {
            foreach (var i in ResolveAsType.GetInterfaces())
                As(i);

            return this;
        }

        public IDependencyInjectorEntry AsSelf()
        {
            _registeredAs.Add(ResolveAsType);
            return this;
        }

        public IDependencyInjectorEntry AsInstance(object Instance)
        {
            resultType = DependencyInjectorEntryType.INSTANCE;
            this.Instance = Instance;
            return this;
        }

        public IDependencyInjectorEntry AsFunction(Func<object> Function)
        {
            resultType = DependencyInjectorEntryType.FUNCTION;
            this.Function = Function;
            return this;
        }

        public IDependencyInjectorEntry AsLazy(Lazy<object> Lazy)
        {
            resultType = DependencyInjectorEntryType.LAZY;
            this.Lazy = Lazy;
            return this;
        }

        public IDependencyInjectorEntry As<T>()
        {
            return As(typeof(T));
        }

        public IDependencyInjectorEntry As(Type t)
        {
            if (!_registeredAs.Contains(t))
                _registeredAs.Add(t);
            return this;
        }

        public IDependencyInjectorEntry AsSingleton()
        {
            resultType = DependencyInjectorEntryType.SINGLETON;
            return this;
        }

        public IDependencyInjectorEntry WithKey(object Key)
        {
            Keys.Add(Key);
            return this;
        }

        public T GetEntry<T>()
        {
            try
            {
                object obj = GetEntry(typeof(T));
                if (obj != null && obj is T o)
                {
                    return o;
                }
                else
                    return default(T);
            }
            catch (Exception err)
            {
                Console.WriteLine("Failed to Get Service -> " + err.ToString());
                return default(T);
            }

        }

        public bool IsRegisteredAs(Type t)
        {
            return _registeredAs.Contains(t);
        }

        public List<Type> GetRegisteredTypes()
        {
            return _registeredAs;
        }

        public List<object> GetKeys()
        {
            return Keys;
        }

        public bool HasKey(object Key)
        {
            if (Keys.Count == 0)
                return false;
            else
                return Keys.Contains(Key);
        }

        public bool IsEntryType(DependencyInjectorEntryType type)
        {
            return resultType == type;
        }
        private object _lockObject = new object();

        

        public object GetEntry(Type t)
        {
            try
            {
                
                object obj = null;

                switch (resultType)
                {
                    case DependencyInjectorEntryType.FUNCTION:
                        obj = Function.Invoke();
                        break;
                    case DependencyInjectorEntryType.INSTANCE:
                        obj = Instance;
                        break;
                    case DependencyInjectorEntryType.RESOLVABLE:
                        obj = CreateInstance();
                        break;
                    case DependencyInjectorEntryType.SINGLETON:
                        lock (_lockObject)
                        {
                            if (Instance != null)
                                return Instance;
                            else
                            {
                                Instance = CreateInstance();
                                obj = Instance;
                            }
                        }
                        break;
                    case DependencyInjectorEntryType.LAZY:
                        obj = Lazy.Value;
                        break;
                }

                return obj;

            }
            catch (Exception err)
            {
                Console.WriteLine("Failed to Get Service -> " + err.ToString());
                return null;
            }
        }

        private object CreateInstance()
        {
            var constructors = ResolveAsType.GetConstructors();
            var constructor = constructors[0];

            var parameters = constructor.GetParameters();
            if(parameters.Length == 0)
            {
                return Activator.CreateInstance(ResolveAsType);
            }
            else
            {
                object[] arguments = new object[parameters.Length];
                for(int i = 0; i< parameters.Length; i++)
                {
                    if (parameters[i].ParameterType == typeof(IDependencyInjectorService))
                        arguments[i] = InjectorService;
                    else
                        arguments[i] = InjectorService.Resolve(parameters[i].ParameterType);
                }
                return Activator.CreateInstance(ResolveAsType, arguments);
            }


        }
    }
}
