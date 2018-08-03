using Caspara.ConfigurationSets;
using Caspara.DependencyInjection;
using Caspara.Extensions;
using Caspara.Messaging;
using Caspara.Persistance;
using Caspara.Serializing;
using Caspara.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Caspara.Commands
{
    public class CommandP2PListenerService : Service, ICommandListenerService
    {
        public override string Name { get; set; } = "Command Listener";

        IPersistanceModel PersistanceModel;
        IMessageP2PListener CommandListener;
        ICommandResponseService ResponseService;
        ConfigurationSet Configuration;

        private List<ICommandHandler> CommandHandlers = new List<ICommandHandler>();


        public CommandP2PListenerService(IPersistanceModel PersistanceModel, IMessageP2PListener CommandListener, ICommandResponseService ResponseService)
        {
            this.CommandListener = CommandListener;
            this.PersistanceModel = PersistanceModel;
            this.ResponseService = ResponseService;
            Configuration = PersistanceModel.Configurations.Get("CommandP2PListenerService");

        }

        public override void Start()
        {
            var port = Configuration.Get<int>("ListenPort");
            if (port == 0)
                port = 65008;

            CommandListener.ListenPort = port;
            CommandListener.HandleMessage += CommandListener_HandleMessage;
            CommandListener.Start();

            base.Start();
        }

        private string CommandListener_HandleMessage(string arg)
        {
            return ResponseService.GetResponse(arg).Message;
        }

        public override void Stop()
        {
            Configuration.Set("ListenPort", CommandListener.ListenPort);
            CommandListener.Stop();
            base.Stop();
        }
    }
}
