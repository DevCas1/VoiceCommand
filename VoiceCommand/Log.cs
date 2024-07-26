namespace VoiceCommand;

public static class Log
{
    public enum LogType { Info, Warning, Error }
    
    public static void LogToConsole(string message, LogType logType = LogType.Info) => Console.WriteLine($"[{DateTime.Now}] ({logType}) {message}");
}