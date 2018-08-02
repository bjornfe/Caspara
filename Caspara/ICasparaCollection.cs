using System;
using System.Collections.Generic;
using System.Text;

namespace Caspara
{
    public interface ICasparaCollection<IType>
    {
        void Set(object Key, IType Item);
        T Get<T>() where T : IType;
        T Get<T>(object Key) where T : IType;
        IType Get(object Key);
        void Remove(IType Item);
        void Remove(object Key);
    }
}
