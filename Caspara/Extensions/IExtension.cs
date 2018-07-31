using Caspara.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Caspara.Extensions
{
    public interface IExtension
    {
        string Name { get; }
        void Register(IApplication app);
        void Load(IApplication app);
        void Save(IApplication app);
    }
}
