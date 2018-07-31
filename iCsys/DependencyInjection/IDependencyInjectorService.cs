using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Caspara.DependencyInjection
{
    public interface IDependencyInjectorService
    {
        int GetRegistrationCount();
        IDependencyInjectorEntry Register<T>();
        IDependencyInjectorEntry Register(Type t);
        IDependencyInjectorEntry Register(object obj);
        IDependencyInjectorEntry RegisterOrReplace<T>(object Key = null);
        T Resolve<T>(object Key = null);
        object Resolve(Type t, object Key = null);
        List<T> ResolveAll<T>(DependencyInjectorEntryType type = DependencyInjectorEntryType.NONE);
        void RegisterAssemblyTypes<T>(Assembly asm, bool IncludeInterfaces = true);
        void RegisterAssemblyTypesWithStaticFieldAsKey<T>(Assembly asm, String IDFieldName, Type IDFieldType);

    }
}
