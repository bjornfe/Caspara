using System;
using System.Collections.Generic;
using System.Text;

namespace Caspara.IO
{
    public interface IDigitalOutputPort : IPort
    {
        void Write(bool state);
    }
}
