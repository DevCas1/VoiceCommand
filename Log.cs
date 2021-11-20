using System;
using System.Collections.Generic;
using System.Text;

namespace VoiceCommand
{
    public static class Log
    {
        public static void LogToConsole(string message, LogType logType) => Console.WriteLine($"{DateTime.Now} ({logType.ToString()}) {message}");

        public enum LogType
        {
            Info,
            Warning,
            Error
        }
    }
}