using System;

namespace Caspara.Commands.ClientTwo
{
    class Program
    {
        static void Main(string[] args)
        {
            Caspara.IncludeConfiguration<Configuration>();
            Caspara.LoadConfiguration();

            Caspara.Start();

            bool cont = true;
            while (cont)
            {
                var key = Console.ReadKey();

                if (key.Key == ConsoleKey.Escape)
                    cont = false;
                else
                {
                    object CommandID = null;
                    switch (key.KeyChar)
                    {
                        case '1':
                            CommandID = 100;
                            break;

                    }

                    if (CommandID != null)
                    {
                        var executor = Caspara.GetDependencyInjector().Resolve<ICommandExecutor>(CommandID);
                        var result = executor.Execute("192.168.1.106");
                        if (result != null)
                        {
                            Console.WriteLine("Command " + executor.CommandID + " Executed with " + (result.Success ? " Success" : " Failure") + " and Value: " + result.Result.ToString());
                        }
                        else
                            Console.WriteLine("Failed to get response");
                    }
                }

            }

            Caspara.Stop();
        }
    }
}
