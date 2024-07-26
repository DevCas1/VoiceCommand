using System;
using System.Speech.Recognition;
using System.Threading;
using VoiceCommand.Input;

namespace VoiceCommand;

internal class Program
{
    private RecognitionHandler? _recognitionHandler;

    private void Main() // Add (string[] args) to use command line arguments when starting the program
    {
        _recognitionHandler = new RecognitionHandler();
    }
}
