
using Caspara.Commands.ClientOne.CommandHandlers;
using Caspara.Commands.ClientOne.Commands;

namespace Caspara.Commands.ClientOne
{
    public class Configuration : IConfigurationClass
    {
        public void Configure(IApplication app)
        {
            app.InjectorService.Register<CommandP2PClientService>().As<ICommandClientService>().AsSingleton();
            app.InjectorService.Register<CommandP2PListenerService>().As<ICommandListenerService>().AsIncludedInterfaces().AsSingleton();

            app.InjectorService.Register<ClientOneCommand1Executor>().As<ICommandExecutor>().WithKey(100);
            app.InjectorService.Register<ClientTwoCommand1Handler>().As<ICommandHandler>().WithKey(100);

        }

        public void Start(IApplication app)
        {
            
        }

        public void Stop(IApplication app)
        {
            
        }
    }
}
