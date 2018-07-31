using System;
using System.Collections.Generic;
using System.Text;

namespace Caspara.Logging
{
    public class ConsoleTextLogger : IConsoleLogger
    {
        public void LogText(string Text)
        {
            Console.WriteLine(DateTime.Now.ToString("HH:mm:ss") + " -> " + Text);
        }
    }
}
