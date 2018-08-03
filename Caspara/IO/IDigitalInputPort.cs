using System;
using System.Collections.Generic;
using System.Text;

namespace Caspara.IO
{
    public interface IDigitalInputPort : IPort
    {
        bool Read();
    }
}
