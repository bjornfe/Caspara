

namespace Caspara.Messaging.Server
{
    public class Configuration : IConfigurationClass
    {
        public void Configure(IApplication app)
        {
            app.InjectorService.Register<MessagePublishServer>()
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
