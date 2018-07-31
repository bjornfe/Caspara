using System;
using System.Collections.Generic;
using System.Text;

namespace Caspara.Values
{
    public interface IValue
    {
        String ValueType { get; set; }

        object GetValue();
        IValue SetValue(object Value);
    }
}
