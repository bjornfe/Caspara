using System;
using System.Collections.Generic;
using System.Text;

namespace Caspara.Repositories
{
    public interface IRepository : IObject
    {
        List<IObject> GetAll();

        IObject Get(object ID);

        void Add(IObject item);

        void Remove(object ID);
    }
}
