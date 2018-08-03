using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Caspara;
using Caspara.ConfigurationSets;
using Caspara.Repositories;

namespace Caspara.Persistance
{
    [DataContract]
    public class PersistanceModel : IPersistanceModel
    {
        [DataMember]
        public CasparaCollection<Repository> Repositories { get; private set; }

        [DataMember]
        public CasparaCollection<ConfigurationSet> Configurations { get; private set; }

        public PersistanceModel()
        {
            Repositories = new CasparaCollection<Repository>();
            Configurations = new CasparaCollection<ConfigurationSet>();
        }
    }
}
