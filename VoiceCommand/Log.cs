namespace VoiceCommand;

public static class Log
{
    public enum LogType { Info, Warning, Error }

    public static void Info(string message) => LogToConsole(message, LogType.Info);

    public static void Warning(string message) => LogToConsole(message, LogType.Warning);

    public static void Error(string message) => LogToConsole(message, LogType.Error);

    private static void LogToConsole(string message, LogType logType) => Console.WriteLine($"[{DateTime.Now}] ({logType}) {message}");
}