using System;
using System.Collections.Generic;
using System.Text;

namespace Caspara.ConfigurationSets
{
    public interface IConfigurationSet
    {
        void Set(object Key, object value);
        object Get(object Key);
        T Get<T>(object Key);
    }
}
