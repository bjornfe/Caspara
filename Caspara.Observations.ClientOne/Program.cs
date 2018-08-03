using System;

namespace Caspara.Observations.ClientOne
{
    class Program
    {
        static void Main(string[] args)
        {
            Caspara.IncludeConfiguration<Configuration>();
            Caspara.LoadConfiguration();

            Caspara.Start();
            Console.ReadLine();
            Caspara.Stop();

        }
    }
}
