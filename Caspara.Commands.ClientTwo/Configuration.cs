
using Caspara.Commands.ClientTwo.CommandHandlers;
using Caspara.Commands.ClientTwo.Commands;

namespace Caspara.Commands.ClientTwo
{
    public class Configuration : IConfigurationClass
    {
        public void Configure(IApplication app)
        {
            app.InjectorService.Register<CommandP2PClientService>().As<ICommandClientService>();
            app.InjectorService.Register<CommandP2PListenerService>().As<ICommandListenerService>();
            app.InjectorService.Register<ClientOneCommand1Handler>().As<ICommandHandler>().WithKey(100);
            app.InjectorService.Register<ClientTwoCommand1Executor>().As<ICommandExecutor>().WithKey(100);

        }

        public void Start(IApplication app)
        {
            
        }

        public void Stop(IApplication app)
        {
            
        }
    }
}
