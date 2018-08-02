using System;

namespace Caspara.TestClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Caspara.IncludeConfiguration<Configuration>();
            Caspara.LoadConfiguration();

            Caspara.Start();
            Console.WriteLine("TestApplication Started");
            Console.ReadLine();


            Caspara.Stop();
            Console.WriteLine("TestApplication Ended");
            Console.ReadLine();
        }
    }
}
