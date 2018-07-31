using System;
using System.Collections.Generic;
using System.Text;

namespace Caspara
{
    public interface IConfigurationClass
    {
        void Configure(IApplication app);
        void Start(IApplication app);
        void Stop(IApplication app);
    }
}
