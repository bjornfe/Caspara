using System;
using System.Collections.Generic;
using System.Text;

namespace Caspara.Services
{
    public interface IService : IObject
    {
        bool IsRunning { get; }
        bool Enabled { get; set; }
        void Start();
        void Stop();
    }
}
