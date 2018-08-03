using Caspara.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Caspara.Commands
{
    public class CommandExtension : Extension
    {

        public override string Name => "Commands";

        public override void Register(IApplication app)
        {
            app.InjectorService.Register<CommandReponseService>().As<ICommandResponseService>();
        }
    }
}
