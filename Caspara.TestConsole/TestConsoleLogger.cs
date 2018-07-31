using Caspara.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Caspara.TestConsole
{
    public class TestConsoleLogger : IConsoleLogger
    {
        public void LogText(string Text)
        {
            Console.WriteLine("TestConsoleLogger -> " + Text);
        }
    }
}
