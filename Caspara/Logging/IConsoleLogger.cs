using System;
using System.Collections.Generic;
using System.Text;

namespace Caspara.Logging
{
    public interface IConsoleLogger : ILogger
    {
        void LogText(String Text);
    }
}
