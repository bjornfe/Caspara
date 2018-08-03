
namespace Caspara.Observations.ClientTwo
{
    public class Configuration : IConfigurationClass
    {
        public void Configure(IApplication app)
        {
            app.InjectorService.Register<ObservationMessageService>()
                .AsIncludedInterfaces()
                .AsSingleton();

            app.InjectorService.Register<ObservationPublisherService>()
                .AsIncludedInterfaces()
                .AsSingleton();

            app.InjectorService.Register<ObservationSubscriberService>()
                .AsIncludedInterfaces()
                .AsSingleton();

        }

        public void Start(IApplication app)
        {
            
        }

        public void Stop(IApplication app)
        {
            
        }
    }
}
