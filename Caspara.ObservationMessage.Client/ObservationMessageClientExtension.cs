using Caspara.Extensions;
using Caspara.Services;
using System;

namespace Caspara.ObservationMessage.Client
{
    public static class ObservationMessageClientExtension
    {

        public static void IncludeObservationMessageClient(this IApplication app)
        {
            app.InjectorService.Register<ObservationMessageClient>()
                .AsIncludedInterfaces()
                .AsSingleton();
        }
    }

    //public class ObservationMessageClientExtension : Extension
    //{
    //    public override string Name => "ObservationMessage Client";

    //    public override void Register(IApplication app)
    //    {
    //        app.InjectorService.Register<ObservationMessageClient>()
    //            .AsIncludedInterfaces()
    //            .AsSingleton();
    //    }
    //}
}
