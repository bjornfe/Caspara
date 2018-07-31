using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Runtime.Serialization;

namespace Caspara.Extensions
{
    public class ExtensionManager : IExtensionManager
    {
        private readonly List<Type> _objectTypes = new List<Type>();
        private readonly List<Type> _validTypes = new List<Type>();
        private readonly List<Type> _extensionTypes = new List<Type>();
        private readonly List<IExtension> _extensions = new List<IExtension>();
        private readonly object _lockObject = new object();
        private string _applicationPath;

        public string GetApplicationPath()
        {
            throw new NotImplementedException();
        }

        public IExtension GetExtension(string Name)
        {
            lock (_lockObject)
            {
                if (_extensions.Any(i => i.Name.Equals(Name)))
                    return _extensions.Where(i => i.Name.Equals(Name)).First();
                else
                    return null;
            }
        }

        public List<Type> GetObjectTypes()
        {
            return _objectTypes;
        }

        public List<Type> GetValidTypes()
        {
            return _validTypes;
        }

        public void LoadExtensions(IApplication app)
        {
            foreach (var t in _extensionTypes)
            {
                IExtension e = (IExtension)Activator.CreateInstance(t);
                e.Register(app);

                _extensions.Add(e);
                Console.WriteLine("Loaded Extension:" + e.Name);
            }
        }

        public void LocateExtensions()
        {
            Console.WriteLine("Locating Extensions...");

            _objectTypes.Clear();

            var parentAssembly = Assembly.GetExecutingAssembly();
            AppDomain.CurrentDomain.Load(parentAssembly.FullName);

            _applicationPath = Path.GetDirectoryName(parentAssembly.Location);
            AssemblyName assemblyName = parentAssembly.GetName();


            foreach (var file in Directory.GetFiles(_applicationPath, "*.dll", SearchOption.TopDirectoryOnly))
            {
                if (Path.GetFileName(file).Contains("Caspara"))
                {
                    Console.WriteLine("Evaluating File: " + file);
                    try
                    {
                        assemblyName = AssemblyName.GetAssemblyName(file);
                        var asm = Assembly.Load(assemblyName);
                        LocateAssemblyExtensions(asm);
                    }
                    catch (Exception err)
                    {
                        Console.WriteLine("Failed to load assembly " + assemblyName.Name + "\n+" + err.ToString());
                    }
                }
            }
        }

        public void LocateAssemblyExtensions(Assembly asm)
        {
            foreach (Type t in asm.GetTypes().Where(tp => !tp.IsInterface))
            {
                _validTypes.Add(t);
                try
                {
                    if(t.GetCustomAttributes().Any(a=> a is DataContractAttribute))
                    {
                        if (!_objectTypes.Contains(t))
                            _objectTypes.Add(t);
                    }

                    if (!t.IsAbstract && t.GetInterfaces().Any(i => i == typeof(IExtension)))
                    {
                        if (!_extensionTypes.Contains(t))
                            _extensionTypes.Add(t);
                    }
                }
                catch (Exception err)
                {
                    Console.WriteLine("Failed to add " + t.Name + " -> " + asm.FullName + " -> " + t.Name + " " + err.ToString());
                }
            }
        }

        public void SaveExtensions(IApplication app)
        {
            foreach (var m in _extensions)
                m.Save(app);
        }
    }
}
