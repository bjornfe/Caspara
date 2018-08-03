using Caspara;
using Caspara.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Caspara.Persistance
{
    public interface IPersistanceService
    {
        IPersistanceModel Model { get; }

        void Load<T>(String Path) where T : IPersistanceModel;
        void Save();
    }
}
