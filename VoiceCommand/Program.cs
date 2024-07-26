using System;
using System.Speech.Recognition;
using System.Threading;
using VoiceCommand;
using VoiceCommand.Input;
using Scancode = VoiceCommand.Input.Scancode;

namespace VoiceCommandHagoeie
{
    internal class Program
    {
        private static ManualResetEvent? Completed = null;
        private static string ShutdownWord = "Close Voice Command";
        private static Command[]? LoadedCommands = null;

        private static void Main() // Add (string[] args) to use command line arguments when starting the program
        {
            Log.LogToConsole("Initializing...");

            Completed = new ManualResetEvent(false);

            SpeechRecognitionEngine recognitionEngine = new SpeechRecognitionEngine();

            recognitionEngine.LoadGrammarCompleted += static (object? sender, LoadGrammarCompletedEventArgs args) => Console.WriteLine("Grammars loaded.");
            recognitionEngine.SpeechRecognized += OnSpeechRecognized;
            recognitionEngine.SpeechRecognitionRejected += OnSpeechRecognitionRejected;

            AddGrammars(recognitionEngine, LoadedCommands = LoadCommands());

            recognitionEngine.SetInputToDefaultAudioDevice(); // set the input of the speech recognizer to the default audio device
            recognitionEngine.RecognizeAsync(RecognizeMode.Multiple); // recognize speech asynchronous
            Log.LogToConsole("Ready!");

            Completed.WaitOne(); // wait until speech recognition is completed
            recognitionEngine.Dispose(); // dispose the speech recognition engine
        }

        private static Command[] LoadCommands() //TODO: Replace with JSON parser
        {
            Command[] commands = new Command[]
            {
                new Command(
                    "test", 
                    [
                        new InputAction(Scancode.sc_shiftLeft, true),
                        new InputAction(Scancode.sc_h, true), new InputAction(Scancode.sc_h, false),
                        new InputAction(Scancode.sc_shiftLeft, false),
                        new InputAction(Scancode.sc_a, true), new InputAction(Scancode.sc_o, false),
                        new InputAction(Scancode.sc_g, true), new InputAction(Scancode.sc_i, false),
                        new InputAction(Scancode.sc_o, true), new InputAction(Scancode.sc_i, false),
                        new InputAction(Scancode.sc_e, true), new InputAction(Scancode.sc_i, false),
                        new InputAction(Scancode.sc_i, true), new InputAction(Scancode.sc_i, false),
                        new InputAction(Scancode.sc_e, true), new InputAction(Scancode.sc_i, false),
                    ]
                ),       
                
                new Command(
                    "power engines", 
                    [new InputAction(Scancode.sc_1, true), new InputAction(Scancode.sc_1, false),]
                ),        
                
                new Command(
                    "power weapons", 
                    [new InputAction(Scancode.sc_2, true), new InputAction(Scancode.sc_2, false),]
                ),        
                
                new Command(
                    "power shields", 
                    [new InputAction(Scancode.sc_3, true), new InputAction(Scancode.sc_3, false),]
                ),

                new Command(
                    "maximize engines", 
                    [
                        new InputAction(Scancode.sc_controlLeft, true),
                        new InputAction(Scancode.sc_1, true), new InputAction(Scancode.sc_1, false),
                        new InputAction(Scancode.sc_controlLeft, false)
                    ]
                ),

                new Command(
                    "maximize weapons", 
                    [
                        new InputAction(Scancode.sc_controlLeft, true),
                        new InputAction(Scancode.sc_2, true), new InputAction(Scancode.sc_2, false),
                        new InputAction(Scancode.sc_controlLeft, false)
                    ]
                ),

                new Command(
                    "maximize shields", 
                    [
                        new InputAction(Scancode.sc_controlLeft, true),
                        new InputAction(Scancode.sc_3, true), new InputAction(Scancode.sc_3, false),
                        new InputAction(Scancode.sc_controlLeft, false)
                    ]
                ),
            };

            return commands;
        }

        private static void AddGrammars(SpeechRecognitionEngine recognitionEngine, Command[] commands)
        {
            //new System.Globalization.CultureInfo("en-GB"); gr = new Grammar(grammarBuilder); // For SAPI errors regarding phonetic alphabet selection
            Log.LogToConsole("Loading commands...");
            recognitionEngine.LoadGrammar(new Grammar(new GrammarBuilder(ShutdownWord)));

            foreach (var command in commands)
                recognitionEngine.LoadGrammar(new Grammar(new GrammarBuilder(command.CommandPhrase)));

            Log.LogToConsole($"{commands.Length} command{(commands.Length > 1 ? "s" : "")} loaded.");
        }

        private static void OnSpeechRecognized(object? sender, SpeechRecognizedEventArgs args)
        {
            string result = args.Result.Text;

            if (result == ShutdownWord)
            {
                ShutDown();
                return;
            }

            if (LoadedCommands == null)
            {
                Log.LogToConsole("Cannot execute commands when no commands are loaded!", Log.LogType.Warning);
                return;
            }

            Command? recognizedCommand = null;

            foreach (var command in LoadedCommands)
            {
                if (command.CommandPhrase == result)
                {
                    recognizedCommand = command;
                    break;
                }
            }

            if (recognizedCommand != null) // Only use recognizedCommand when a command has actually been recognized. Can't check for null on recognizedCommand because it's a (non-nullable) struct
            {
                Log.LogToConsole($"Command recognized \"{recognizedCommand.Value.CommandPhrase}\"");
                Keyboard.SendInputs(recognizedCommand.Value.Inputs);
            }
        }

        private static void ShutDown()
        {
            Log.LogToConsole("Shutdown phrase recognized, shutting down...");
            Thread.Sleep(1000);
            Completed?.Set();
        }

        private static void OnSpeechRecognitionRejected(object? sender, SpeechRecognitionRejectedEventArgs args) => Log.LogToConsole("Still listening...");
    }
}
