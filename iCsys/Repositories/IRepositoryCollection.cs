using System;
using System.Collections.Generic;
using System.Text;

namespace Caspara.Repositories
{
    public interface IRepositoryCollection : IObject
    {
        void AddRepository(IRepository Repository);
        T GetRepository<T>() where T : IRepository;
        T GetRepository<T>(object Key) where T : IRepository;
        void RemoveRepository(IRepository repository);
        void RemoveRepository(object ID);
    }
}
