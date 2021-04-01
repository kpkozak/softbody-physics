using System;

namespace Logging
{
    public class ConsoleLogger : ILogger
    {
        public void Error(string message)
        {
            Console.Out.WriteLine($"{DateTime.Now} - ERROR - {message}");
        }

        public void Debug(string message)
        {
            Console.Out.WriteLine($"{DateTime.Now} - DEBUG - {message}");
        }
    }
}