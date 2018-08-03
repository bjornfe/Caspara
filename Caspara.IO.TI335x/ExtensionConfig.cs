using Caspara.Extensions;
using Caspara.IO.TI335x.GPIO;
using Caspara.IO.TI335x.Uart;
using System;
using System.Collections.Generic;
using System.Text;

namespace Caspara.IO.TI335x
{
    public class ExtensionConfig : Extension
    {
        public override string Name => "TI335x Extension";

        public override void Register(IApplication app)
        {
            app.InjectorService.Register<GPIOInput>().As<IDigitalInputPort>();
            app.InjectorService.Register<GPIOOutput>().As<IDigitalOutputPort>();
            app.InjectorService.Register<SerialDevice>().As<ISerialPort>();
        }
    }
}
