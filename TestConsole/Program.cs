using System;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Caspara.Core.Application.Load(@"C:\Caspara\Repositories.xml");
            Caspara.Core.Application.Start();

            Console.WriteLine("Application Started");
            Console.ReadLine();

            Caspara.Core.Application.Stop();
            Console.WriteLine("Application Ended");
            Console.ReadLine();

        }
    }
}
