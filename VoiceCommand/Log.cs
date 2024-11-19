namespace VoiceCommand;

public static class Log
{
    public enum LogType {
#if DEBUG
        Debug,
#endif
        Info, 
        Warning, 
        Error 
    }

#if DEBUG
    public static void Debug(string message) => LogToConsole(message, LogType.Debug);
#endif

    public static void Info(string message) => LogToConsole(message, LogType.Info);

    public static void Warning(string message) => LogToConsole(message, LogType.Warning);

    public static void Error(string message) => LogToConsole(message, LogType.Error);

    private static void LogToConsole(string message, LogType logType) => Console.WriteLine($"[{DateTime.Now}] ({logType}) {message}");
}