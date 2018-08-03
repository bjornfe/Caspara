using System;

namespace Caspara.Observations.ClientTwo
{
    class Program
    {
        static void Main(string[] args)
        {
            Caspara.DeviceName = "ObservationClient2";
            Caspara.IncludeConfiguration<Configuration>();
            Caspara.LoadConfiguration();

            Caspara.Start();
            Console.ReadLine();
            Caspara.Stop();
        }
    }
}
