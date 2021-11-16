using System;
using System.Threading;
using System.Threading.Tasks;
using System.Speech.Recognition;
using System.Speech.Recognition.SrgsGrammar;

namespace VoiceCommand
{
    internal class Program
    {
        internal class Command
        {
            public Command(string commandPhrase, string[] actions)
            {
                CommandPhrase = commandPhrase;
                Actions = actions;
            }

            public readonly string CommandPhrase;
            public readonly string[] Actions;
        }

        private static ManualResetEvent Completed = null;
        private static string ShutdownWord = "Close Voice Command";

        private static void Main(string[] args) // Add (string[] args) to use command line arguments when starting the program
        {
            LogToConsole("Initializing...");
            Completed = new ManualResetEvent(false);

            SpeechRecognitionEngine recognitionEngine = new SpeechRecognitionEngine();

            recognitionEngine.LoadGrammarCompleted += (object o, LoadGrammarCompletedEventArgs a) => { Console.WriteLine("Grammars loaded."); };
            recognitionEngine.SpeechRecognized += OnSpeechRecognized;
            recognitionEngine.SpeechRecognitionRejected += OnSpeechRecognitionRejected;
            //grammarBuilder.Culture = new System.Globalization.CultureInfo("en-GB"); gr = new Grammar(grammarBuilder);

            AddGrammars(recognitionEngine, LoadCommands());

            //recognitionEngine.LoadGrammar(new Grammar(new GrammarBuilder("maximize shields"))); // Special setup
            //recognitionEngine.LoadGrammar(new Grammar(new GrammarBuilder("maximize shields"))); // Special setup
            //recognitionEngine.LoadGrammar(new Grammar(new GrammarBuilder("maximize shields"))); // Special setup

            recognitionEngine.SetInputToDefaultAudioDevice(); // set the input of the speech recognizer to the default audio device
            recognitionEngine.RecognizeAsync(RecognizeMode.Multiple); // recognize speech asynchronous
            LogToConsole("Ready!");

            Completed.WaitOne(); // wait until speech recognition is completed

            recognitionEngine.Dispose(); // dispose the speech recognition engine

            //Console.ReadLine();
        }

        private static Command[] LoadCommands() //TODO: Replace with JSON parser
        {
            Command[] commands = new Command[]
            {
                new Command(ShutdownWord, new string[]{""}),
                new Command("power the engines", new string[]{""}),
                new Command("power the weapons", new string[]{""}),
                new Command("power the shields", new string[]{""}),
                new Command("maximize engines", new string[]{""}),
                new Command("maximize weapons", new string[]{""}),
                new Command("maximize shields", new string[]{""})
            };

            return commands;
        }

        private static void AddGrammars(SpeechRecognitionEngine recognitionEngine, Command[] commands)
        {
            LogToConsole("Loading commands...");
            foreach (var command in commands)
                recognitionEngine.LoadGrammar(new Grammar(new GrammarBuilder(command.CommandPhrase)));

            LogToConsole($"{commands.Length} command{(commands.Length > 1 ? "s" : "")} loaded.");
        }

        private static void LogToConsole(string message) => Console.WriteLine(DateTime.Now + ": " + message);

        private static void OnSpeechRecognized(object sender, SpeechRecognizedEventArgs args)
        {            
            if (args.Result.Text == ShutdownWord)
            {
                LogToConsole("Shutdown word recognized, shutting down...");
                Thread.Sleep(1000);
                Completed.Set();
                return;
            }

            LogToConsole($"Command recognized \"{args.Result.Text}\"");
        }

        private static void OnSpeechRecognitionRejected(object sender, SpeechRecognitionRejectedEventArgs args) => LogToConsole("Still listening...");
    }
}