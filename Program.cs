using System;
using System.Threading;
using System.Threading.Tasks;
using System.Speech.Recognition;
using System.Speech.Recognition.SrgsGrammar;

namespace VoiceCommand
{
    internal class Program
    {
        private static ManualResetEvent Completed = null;
        private static string ShutdownWord = "Close Voice Command";

        private static void Main(string[] args) // Add (string[] args) to use command line arguments when starting the program
        {
            Completed = new ManualResetEvent(false);

            SpeechRecognitionEngine recognitionEngine = new SpeechRecognitionEngine();

            recognitionEngine.LoadGrammarCompleted += (object o, LoadGrammarCompletedEventArgs a) => { Console.WriteLine("Grammars loaded."); };
            recognitionEngine.SpeechRecognized += OnSpeechRecognized;
            recognitionEngine.SpeechRecognitionRejected += OnSpeechRecognitionRejected;

            recognitionEngine.LoadGrammar(new Grammar(new GrammarBuilder(ShutdownWord)));
            recognitionEngine.LoadGrammar(new Grammar(new GrammarBuilder("test")));
            recognitionEngine.LoadGrammar(new Grammar(new GrammarBuilder("power the engines")));
            recognitionEngine.LoadGrammar(new Grammar(new GrammarBuilder("power the weapons")));
            recognitionEngine.LoadGrammar(new Grammar(new GrammarBuilder("power the shields")));
            recognitionEngine.LoadGrammar(new Grammar(new GrammarBuilder("maximize engines")));
            recognitionEngine.LoadGrammar(new Grammar(new GrammarBuilder("maximize weapons")));
            recognitionEngine.LoadGrammar(new Grammar(new GrammarBuilder("maximize shields")));
            //recognitionEngine.LoadGrammar(new Grammar(new GrammarBuilder("maximize shields"))); // Special setup
            //recognitionEngine.LoadGrammar(new Grammar(new GrammarBuilder("maximize shields"))); // Special setup
            //recognitionEngine.LoadGrammar(new Grammar(new GrammarBuilder("maximize shields"))); // Special setup

            recognitionEngine.SetInputToDefaultAudioDevice(); // set the input of the speech recognizer to the default audio device
            recognitionEngine.RecognizeAsync(RecognizeMode.Multiple); // recognize speech asynchronous

            Completed.WaitOne(); // wait until speech recognition is completed

            recognitionEngine.Dispose(); // dispose the speech recognition engine

            //Console.ReadLine();
        }

        private static void OnSpeechRecognized(object sender, SpeechRecognizedEventArgs args)
        {            
            if (args.Result.Text == ShutdownWord)
            {
                Console.WriteLine("Shutdown word recognized, shutting down...");
                Thread.Sleep(1000);
                Completed.Set();
                return;
            }

            Console.WriteLine($"Recognized: {args.Result.Text}");
        }

        private static void OnSpeechRecognitionRejected(object sender, SpeechRecognitionRejectedEventArgs args) => Console.WriteLine("Still listening...");
    }
}