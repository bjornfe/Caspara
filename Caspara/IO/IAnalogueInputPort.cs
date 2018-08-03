using System;
using System.Collections.Generic;
using System.Text;

namespace Caspara.IO
{
    public interface IAnalogueInputPort : IPort
    {
        double ReadValue();
    }
}
