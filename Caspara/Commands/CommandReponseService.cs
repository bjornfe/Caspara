using Caspara.DependencyInjection;
using Caspara.Serializing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Caspara.Commands
{
    public class CommandReponseService : ICommandResponseService
    {
        ISerializer Serializer;
        IDependencyInjectorService InjectorService;

        public CommandReponseService(ISerializer Serializer, IDependencyInjectorService InjectorService)
        {
            this.Serializer = Serializer;
            this.InjectorService = InjectorService;
        }

        public CommandResponse GetResponse(string CommandString)
        {
            var result = new CommandResult(false, "Failed to execute");
            string respondTo = null;
            var cmd = Serializer.Deserialize<CommandMessage>(CommandString, SerializeType.JSON);
            if (cmd != null && cmd.Command != null)
            {
                respondTo = cmd.RespondTo;
                var handler = InjectorService.Resolve<ICommandHandler>(cmd.Command.CommandID);
                if (handler != null)
                {
                    result = handler.Execute(cmd.Command);
                }
                else
                {
                    result.Result.SetValue("Invalid Command");
                }

            }
            var resultString = Serializer.Serialize(result, SerializeType.JSON);
            var response = new CommandResponse()
            {
                RespondTo = respondTo,
                Message = resultString
            };

            return response;
        }
    }
}
