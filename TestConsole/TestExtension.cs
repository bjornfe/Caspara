using Caspara.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Caspara.TestConsole
{
    public class TestExtension : Extension
    {
        public override string Name => "Test Extension";

        public override void Register(IApplication app)
        {
            app.InjectorService.Register<TestThreadService>().AsIncludedInterfaces().AsSingleton();
        }
    }
}
