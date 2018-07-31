using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Caspara.Extensions
{
    public interface IExtensionManager
    {
        void LocateExtensions();
        void LocateAssemblyExtensions(Assembly asm);
        void LoadExtensions(IApplication app);
        void SaveExtensions(IApplication app);
        IExtension GetExtension(String Name);
        List<Type> GetObjectTypes();
        List<Type> GetValidTypes();
        String GetApplicationPath();
    }
}
