using Caspara.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Caspara.Repositories
{
    public interface IRepositoryPersistanceService
    {
        IRepositoryCollection Repositories { get; }
        void Load<T>(String Path) where T : IRepositoryCollection;
        void Save();
    }
}
