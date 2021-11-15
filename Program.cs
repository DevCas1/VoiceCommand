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

        private static void Main(string[] args) // Add (string[] args) to use command line arguments when starting the program
        {
            Completed = new ManualResetEvent(false);

            SpeechRecognitionEngine recognitionEngine = new SpeechRecognitionEngine();
            recognitionEngine.LoadGrammarCompleted += (object o, LoadGrammarCompletedEventArgs a) => { Console.WriteLine("Grammars loaded."); };
            recognitionEngine.SpeechRecognized += OnSpeechRecognized;
            recognitionEngine.SpeechRecognitionRejected += OnSpeechRecognitionRejected;

            //SrgsRule moduleRule = new SrgsRule("moduleList");

            //SrgsOneOf modules = new SrgsOneOf(new string[] { "engine", "weapon", "shield"});
            //moduleRule.Add(modules);
            //moduleRule.Scope = SrgsRuleScope.Public;

            //SrgsDocument document = new SrgsDocument();
            //document.Rules.Add(moduleRule);
            //document.Root = moduleRule;

            //SrgsItem[] optionalItems = { new SrgsItem(0, 1, "the"), new SrgsItem(0, 1, "the") };
            //SrgsItem mandatoryItem = new SrgsItem(1, 1, "power");
            //SrgsItem alternativeMandatoryItem = new SrgsItem(1, 1, "convert");

            recognitionEngine.LoadGrammar(new Grammar(new GrammarBuilder("test")));
            recognitionEngine.LoadGrammar(new Grammar(new GrammarBuilder("exit")));

            recognitionEngine.SetInputToDefaultAudioDevice(); // set the input of the speech recognizer to the default audio device
            recognitionEngine.RecognizeAsync(RecognizeMode.Multiple); // recognize speech asynchronous

            Completed.WaitOne(); // wait until speech recognition is completed

            recognitionEngine.Dispose(); // dispose the speech recognition engine

            Console.ReadLine();
        }

        private static void OnSpeechRecognized(object sender, SpeechRecognizedEventArgs args)
        {
            Console.WriteLine($"Recognized: {args.Result.Text}");
            
            if (args.Result.Text == "exit")
                Completed.Set();
        }

        private static void OnSpeechRecognitionRejected(object sender, SpeechRecognitionRejectedEventArgs args) => Console.WriteLine("Still listening...");
    }
}