using System;
using System.Collections.Generic;
using System.Text;

namespace Caspara.DependencyInjection
{
    public enum DependencyInjectorEntryType
    {
        NONE,
        INSTANCE,
        RESOLVABLE,
        SINGLETON,
        LAZY,
        FUNCTION
    }
}
