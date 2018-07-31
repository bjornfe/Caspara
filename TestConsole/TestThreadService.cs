using Caspara.DependencyInjection;
using Caspara.Logging;
using Caspara.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Caspara.TestConsole
{
    public class TestThreadService : ThreadService
    {
        public override string Name { get; set; } = "Test Thread Service";

        IConsoleLogger TextLogger;
        IDependencyInjectorService DI;

        public TestThreadService(IConsoleLogger TextLogger, IDependencyInjectorService DI)
        {
            this.TextLogger = TextLogger;
            this.DI = DI;
            this.SleepInterval = 1000;
        }
        int messagnr = 0;
        public override void PerformAction()
        {
            TextLogger.LogText("Testing "+(++messagnr)+" with DICount: "+DI.GetRegistrationCount());
        }
    }
}
