using System.Speech.Recognition;
using VoiceCommand.Input;

namespace VoiceCommand;

// using Application = Input.Application;

public class RecognitionHandler(VoiceCommandConfig config)
{
    public bool ShouldStopRecognizing { get; private set; } = false;

    private const string DEFAULT_QUIT_COMMAND = "Exit Voice Command";

    private List<Command>? _loadedCommands = null;

    public void Start()
    {
        Log.Info("Initializing");

        if (config.Commands.Count == 0)
        {
            Log.Error("Loaded VoiceCommandConfig contains no configured applications!\nExiting...");
            return;
        }

        _loadedCommands = config.Commands;

        Run();
    }

    private void Run()
    {
        if (_loadedCommands == null)
        {
            string message = "RecognitionHandler.Run() was called before LoadedCommands was assigned!";
            Log.Error(message);
            throw new NullReferenceException(message);
        }

        // For SAPI errors regarding phonetic alphabet selection
        // using SpeechRecognitionEngine recognitionEngine = new SpeechRecognitionEngine(new System.Globalization.CultureInfo("en-GB"));
        using SpeechRecognitionEngine recognitionEngine = new(new System.Globalization.CultureInfo(config.Language));

        AddGrammarToRecognitionEngine(recognitionEngine, _loadedCommands);
        SubscribeToRecognitionEvents(recognitionEngine);

        Log.Info("Ready!");

        recognitionEngine.SetInputToDefaultAudioDevice(); // set the input of the speech recognizer to the default audio device
        recognitionEngine.RecognizeAsync(RecognizeMode.Multiple); // recognize speech asynchronous

        // Wait for operation to complete
        while (ShouldStopRecognizing == false)
        {
            Thread.Sleep(333); // Some value from documentation
        }

        Console.WriteLine("Speech recognition completed.");
    }

    private void SubscribeToRecognitionEvents(SpeechRecognitionEngine recognitionEngine)
    {
        // recognitionEngine.LoadGrammarCompleted += (object? sender, LoadGrammarCompletedEventArgs args) => Console.WriteLine("Grammars loaded.");
        recognitionEngine.SpeechDetected += OnSpeechDetected;
        recognitionEngine.SpeechRecognized += OnSpeechRecognized;
        recognitionEngine.SpeechRecognitionRejected += OnSpeechRecognitionRejected;
    }

    private void AddGrammarToRecognitionEngine(SpeechRecognitionEngine recognitionEngine, List<Command> commands)
    {
        Log.Info("Loading commands...");

        List<GrammarBuilder> grammarsToLoad = commands.Select(command => new GrammarBuilder(command.CommandPhrase)).ToList();
        grammarsToLoad.Append(new GrammarBuilder(DEFAULT_QUIT_COMMAND));

        foreach(var grammarBuilder in grammarsToLoad)
            recognitionEngine.LoadGrammar(new Grammar(grammarBuilder));

        Log.Info(
            $"{grammarsToLoad.Count} Command{(grammarsToLoad.Count == 1 ? "" : "s")} loaded."
        );
    }

    private void OnSpeechDetected(object? sender, SpeechDetectedEventArgs args) => Log.Info("Speech detected.");

    private void OnSpeechRecognized(object? sender, SpeechRecognizedEventArgs args)
    {
        Log.Info("Speech recognized!");

        string result = args.Result.Text;

        // Switch statement for possible further expansion with built-in commands, like reloading commands from disk
        switch (result)
        {
            case DEFAULT_QUIT_COMMAND:
                ShutDown();
                return;
            default:
                break;
        }

        if (_loadedCommands == null)
        {
            Log.Error("Cannot execute commands when no commands are loaded!");
            return;
        }

        Command? recognizedCommand = _loadedCommands.FirstOrDefault(command => command.CommandPhrase == result);

        if (recognizedCommand == null)
        {
            Log.Error($"Recognized loaded and enabled text {result} but couldn't match it to a command!");
            return;
        }

        Log.Info($"Command recognized \"{recognizedCommand.Value.Name}\" ({recognizedCommand.Value.Name})");
        Keyboard.SendInputs(recognizedCommand.Value.InputActions);
    }

    private void ShutDown()
    {
        Log.Info($"Shutdown command recognized, shutting down...");
        ShouldStopRecognizing = true;
        // Thread.Sleep(1000);
        // Completed?.Set();
    }

    private void OnSpeechRecognitionRejected(object? sender, SpeechRecognitionRejectedEventArgs args) => Log.Info($"\"{args.Result.Text}\" does not match any loaded and enabled Grammar and was rejected. (Confidence: {args.Result.Confidence})");
}