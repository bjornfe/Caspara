using System;
using System.Collections.Generic;
using System.Text;

namespace Caspara.Serializing
{
    public interface ISerializerService
    {
        void Save<T>(SerializeType serType, T obj, String Path);
        T Load<T>(SerializeType serType, String Path);
    }
}
