namespace VoiceCommand;

internal class Program
{
    private static RecognitionHandler? _recognitionHandler;

    private static void Main() // Add (string[] args) to use command line arguments when starting the program
    {
        _recognitionHandler = new RecognitionHandler("Close Voice Command");
        _recognitionHandler.Start();
    }
}
