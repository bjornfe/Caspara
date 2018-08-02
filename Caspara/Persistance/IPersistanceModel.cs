using Caspara.Configurations;
using Caspara.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Caspara.Persistance
{
    public interface IPersistanceModel
    {
        CasparaCollection<Repository> Repositories { get; }
        CasparaCollection<ConfigurationSet> Configurations { get; }
    }
}
