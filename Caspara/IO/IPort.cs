using System;
using System.Collections.Generic;
using System.Text;

namespace Caspara.IO
{
    public interface IPort
    {
        string Name { get; }
        int Nr { get; set; }
        void SetPortNr(int Nr);
        void ClosePort();
    }
}
