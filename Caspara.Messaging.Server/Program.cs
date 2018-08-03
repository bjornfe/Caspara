using System;

namespace Caspara.Messaging.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            Caspara.DeviceName = "MessagingServer";
            Caspara.IncludeConfiguration<Configuration>();
            Caspara.LoadConfiguration();

            Caspara.Start();
            Console.WriteLine("Server Started");
            Console.ReadLine();
            Caspara.Stop();
        }
    }
}
