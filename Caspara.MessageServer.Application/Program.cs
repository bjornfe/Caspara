using System;

namespace Caspara.MessageServer.Application
{
    class Program
    {
        static void Main(string[] args)
        {
            Caspara.DeviceName = "MessageServer.Application";
            Caspara.IncludeConfiguration<Configuration>();
            Caspara.LoadConfiguration();

            Caspara.Start();
            Console.WriteLine("MessageServer Started");
            Console.ReadLine();
            Caspara.Stop();
        }
    }
}
