﻿using System;

namespace Caspara.TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Caspara.UseConfiguration<TestConfigurationClass>();
            Caspara.LoadConfiguration(@"C:\Caspara\Repositories.xml");

            Caspara.Start();
            Console.WriteLine("Application Started");
            Console.ReadLine();

            Caspara.Stop();
            Console.WriteLine("Application Ended");
            Console.ReadLine();

        }
    }
}