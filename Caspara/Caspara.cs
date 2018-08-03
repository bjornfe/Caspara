using Caspara;
using Caspara.DependencyInjection;
using Caspara.Extensions;
using System;
using System.Collections.Generic;
using System.Net;

namespace Caspara
{
    public static class Caspara
    {
        public static string DeviceName = Dns.GetHostName();

        private static List<IConfigurationClass> ConfigurationClasses = new List<IConfigurationClass>();
        public static void IncludeConfiguration<T>() where T : IConfigurationClass
        {
            ConfigurationClasses.Add(Activator.CreateInstance<T>());
        }

        private static IApplication Application;

        public static void LoadConfiguration(String Path = null)
        {
            Application = new Application(ConfigurationClasses);
            Application.Configure(Path);
        }

        public static void Start()
        {
            Application.Start();
        }

        public static void Stop()
        {
            Application.Stop();
        }


    }
}
