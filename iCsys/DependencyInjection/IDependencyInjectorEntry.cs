using System;
using System.Collections.Generic;
using System.Text;

namespace Caspara.DependencyInjection
{
    public interface IDependencyInjectorEntry
    {
        List<Type> GetRegisteredTypes();
        bool IsRegisteredAs(Type t);
        List<object> GetKeys();
        bool HasKey(object Key);
        bool IsEntryType(DependencyInjectorEntryType type);
        IDependencyInjectorEntry AsSelf();
        IDependencyInjectorEntry AsIncludedInterfaces();
        IDependencyInjectorEntry AsInstance(object Instance);
        IDependencyInjectorEntry AsFunction(Func<object> Function);
        IDependencyInjectorEntry AsLazy(Lazy<object> Lazy);
        IDependencyInjectorEntry As<T>();
        IDependencyInjectorEntry As(Type t);
        IDependencyInjectorEntry AsSingleton();
        IDependencyInjectorEntry WithKey(object Key);
        T GetEntry<T>();
        object GetEntry(Type t);
    }
}
