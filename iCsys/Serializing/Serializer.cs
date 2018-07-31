using Caspara.Extensions;
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
    public class Serializer : ISerializer
    {
        IExtensionManager ExtensionManager;

        public Serializer(IExtensionManager ExtensionManager)
        {
            this.ExtensionManager = ExtensionManager;
        }

        public String Serialize<T>(T value, SerializeType SerType)
        {
            List<Type> currentTypes = ExtensionManager.GetObjectTypes();
            if (!currentTypes.Contains(typeof(T)))
                currentTypes.Add(typeof(T));
           

            switch (SerType)
            {
                case SerializeType.JSON:
                    return JsonConvert.SerializeObject(value, Newtonsoft.Json.Formatting.Indented);
                case SerializeType.XML:
                    using (var stringWriter = new StringWriter(CultureInfo.InvariantCulture))
                    {
                        var writer = XmlWriter.Create(stringWriter, new XmlWriterSettings { Indent = true, IndentChars = "\t" });

                        var dataContractSerializer = new DataContractSerializer(typeof(T), currentTypes);
                        dataContractSerializer.WriteObject(writer, value);
                        writer.Flush();
                        return stringWriter.ToString();
                    }
                default:
                    return value.ToString();

            }


        }

        public T Deserialize<T>(String value, SerializeType SerType)
        {

            List<Type> currentTypes = ExtensionManager.GetObjectTypes();
            if (!currentTypes.Contains(typeof(T)))
                currentTypes.Add(typeof(T));


            switch (SerType)
            {
                case SerializeType.JSON:
                    return JsonConvert.DeserializeObject<T>(value);
                case SerializeType.XML:
                    using (var stringReader = new StringReader(value))
                    {
                        var reader = XmlReader.Create(stringReader);
                        var dataContractSerializer = new DataContractSerializer(typeof(T), currentTypes);
                        return (T)dataContractSerializer.ReadObject(reader);

                    }
                default:
                    return default(T);

            }


        }
    }
}
