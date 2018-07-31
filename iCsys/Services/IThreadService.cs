using System;
using System.Collections.Generic;
using System.Text;

namespace Caspara.Services
{
    public interface IThreadService : IService
    {
        
        int SleepInterval { get; set; }
        Double ExecutionTime { get; set; }
    }
}
