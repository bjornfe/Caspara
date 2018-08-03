using Caspara.Extensions;
using Caspara.Messaging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ZeroMQ;

namespace Caspara.Messaging.ZeroMQ
{
    public class ZeroMQExtension : Extension
    {
        public override string Name => "ZeroMQ Extension";

        public override void Register(IApplication app)
        {
            //Copy the nescessary ZeroMQ files into application folder

            var folder = "i386";
            if (Environment.Is64BitProcess)
                folder = "amd64";

            foreach (var f in Directory.GetFiles(folder))
            {
                var fullName = Path.GetFullPath(f);
                var fileName = Path.GetFileName(fullName);

                var appFileName = Path.Combine(app.ApplicationPath, fileName);

                if (!File.Exists(appFileName))
                    File.Copy(fullName, appFileName, true);

            }

            app.InjectorService.Register<ZeroMQServer>().As<IMessagePublishServer>();
            app.InjectorService.Register<ZeroMQClient>().As<IMessagePublishClient>();
            app.InjectorService.Register<ZeroMQP2PClient>().As<IMessageP2PClient>();
            app.InjectorService.Register<ZeroMQP2PListener>().As<IMessageP2PListener>();
        }
    }
}
