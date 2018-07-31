using System;
using System.Collections.Generic;
using System.Text;

namespace Caspara.Serializing
{
    public interface ISerializer
    {
        String Serialize<T>(T value, SerializeType SerType);
        T Deserialize<T>(String value, SerializeType SerType);

    }
}
