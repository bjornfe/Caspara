using System;
using System.Collections.Generic;
using System.Text;

namespace Caspara.Settings
{
    public interface ISetting : IObject
    {
        object Value { get; set; }
        DateTime LastUpdated { get; set; }
    }
}
