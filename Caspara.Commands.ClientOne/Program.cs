using System;

namespace Caspara.Commands.ClientOne
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
                    switch (key.KeyChar)
                    {
                        case '1':
                            break;
                            
                    }
                }

            }


            Caspara.Stop();
        }
    }
}
