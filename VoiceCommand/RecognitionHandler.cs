using System.Speech.Recognition;
using System.Threading;
using VoiceCommand.Input;

namespace VoiceCommand;

using Application = Input.Application;

public class RecognitionHandler(VoiceCommandConfig config)
{
    public bool ShouldStopRecognizing { get; private set; } = false;

    private const string DEFAULT_QUIT_COMMAND = "Close Voice Command";
    private const string DEFAULT_RELOAD_COMMAND = "Reload all Commands";

    private Application? _currentApplication = null;
    private CommandSet? _currentCommandSet = null;
    private List<Command>? _loadedCommands = null;

    public void Start()
    {
        Log.Info("Initializing...");

        if (config.Applications.Count == 0)
        {
            Log.Error("Loaded VoiceCommandConfig contains no configured applications!\nExiting...");
            return;
        }

        _currentApplication = config.Applications?[0];
        _currentCommandSet = _currentApplication?.CommandSets[0];
        _loadedCommands = _currentCommandSet?.Commands;

        Run();
    }

    // public bool LoadNewConfig(VoiceCommandConfig newConfig)
    // {
    //     if(newConfig.Applications.Count == 0)
    //     {
    //         Log.Warning("New VoiceCommandConfig contains no configured applications!\nNew config not loaded.");
    //         return false;
    //     }

    //     return true;
    // }

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

        GrammarBuilder grammarBuilder = new();

        grammarBuilder.Append(DEFAULT_QUIT_COMMAND);
        // recognitionEngine.LoadGrammar(new Grammar(new GrammarBuilder(_quitCommand)));

        Parallel.ForEach(commands, command => { recognitionEngine.LoadGrammar(new Grammar(new GrammarBuilder(command.CommandPhrase))); });
        // Parallel.ForEach(commands, command => { grammarBuilder.Append(command.CommandPhrase); }); // DOESN'T WORK

        // recognitionEngine.LoadGrammarAsync(new Grammar(grammarBuilder));
        // foreach (var command in commands)
        //     recognitionEngine.LoadGrammar(new Grammar(new GrammarBuilder(command.CommandPhrase)));

        Log.Info(
            $"{_loadedCommands?.Count ?? 0} Command{(_loadedCommands?.Count > 1 ? "s" : "")} loaded." +
            $" (Application: \"{_currentApplication?.Name}\" |" +
            $" Command Set: \"{_currentCommandSet?.Name}\")"
        );
    }

    private void OnSpeechDetected(object? sender, SpeechDetectedEventArgs args) => Log.Info("Possible speech detected, processing...");

    private void OnSpeechRecognized(object? sender, SpeechRecognizedEventArgs args)
    {
        Log.Info("Speech recognized! Processing...");

        string result = args.Result.Text;

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
        // Command? recognizedCommand = null;

        // foreach (Command command in LoadedCommands)
        // {
        //     if (command.CommandPhrase == result)
        //     {
        //         recognizedCommand = command;
        //         break;
        //     }
        // }

        if (recognizedCommand == null)
        {
            Log.Error($"Recognized loaded and enabled text {result} but couldn't match it to a command!");
            return;
        }

        Log.Info($"Command recognized \"{recognizedCommand.Value.CommandPhrase}\"");
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