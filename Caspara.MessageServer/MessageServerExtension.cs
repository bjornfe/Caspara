using Caspara.Extensions;
using Caspara.Services;
using System;

namespace Caspara.MessageServer
{
    public static class MessageServerExtension
    {
        public static void IncludeMessageServer(this IApplication app)
        {
            app.InjectorService.Register<MessageServer>()
                .AsIncludedInterfaces()
                .AsSingleton();
        }
    }
}
