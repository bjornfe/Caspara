using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;

namespace Caspara.Serializing
{
    public class LocalSerializerService : ISerializerService
    {
        public ISerializer Serializer;

        public LocalSerializerService(ISerializer Serializer)
        {
            this.Serializer = Serializer;
        }

        public T Load<T>(SerializeType serType, string Path)
        {
            if (File.Exists(Path))
            {
                try
                {
                    String s = File.ReadAllText(Path);
                    T obj = Serializer.Deserialize<T>(s,serType);
                    Console.WriteLine(typeof(T).Name + " Deserialized");
                    return obj;
                }
                catch (Exception err)
                {
                    Console.WriteLine("Failed to deserialize " + typeof(T).Name + " from " + Path + " Exception: " + err.ToString());
                    return default(T);
                }
            }
            else
                return Activator.CreateInstance<T>();
        }

        public void Save<T>(SerializeType serType, T obj, string Path)
        {
            if (obj != null && Path != null)
            {
                new FileInfo(Path).Directory.Create();

                String s = Serializer.Serialize(obj, serType);

                try
                {
                    File.WriteAllText(Path, s);
                    Console.WriteLine("Saved " + obj.GetType().Name + " to " + Path);
                }
                catch (Exception err)
                {
                    Console.WriteLine("Failed to save " + obj.GetType().Name + " to " + Path + " Exception: " + err.ToString());
                }
            }
        }

        
    }
}
