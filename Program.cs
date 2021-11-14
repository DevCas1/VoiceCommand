using System;
using System.Threading.Tasks;
using System.Speech.Recognition;
using System.Speech.Recognition.SrgsGrammar ;

namespace VoiceCommand
{
    internal class Program
    {
        private static void Main(string[] args) // Add (string[] args) to use command line arguments when starting the program
        {
            SpeechRecognitionEngine speechRecognitionEngine = new SpeechRecognitionEngine();
            SpeechRecognizer recognizer = new SpeechRecognizer();
            //recognizer.SpeechRecognized += OnSpeechRecognized;

            SrgsRule moduleRule = new SrgsRule("moduleList");

            SrgsOneOf modules = new SrgsOneOf(new string[] { "engine", "weapon", "shield"});
            moduleRule.Add(modules);
            moduleRule.Scope = SrgsRuleScope.Public;

            SrgsDocument document = new SrgsDocument();
            document.Rules.Add(moduleRule);
            document.Root = moduleRule;

            SrgsItem[] optionalItems = { new SrgsItem(0, 1, "the"), new SrgsItem(0, 1, "the") };
            SrgsItem mandatoryItem = new SrgsItem(1, 1, "power");
            SrgsItem alternativeMandatoryItem = new SrgsItem(1, 1, "convert");

            Console.ReadLine();
        }

        private static void OnSpeechRecognized(SpeechRecognizedEventArgs args)
        {

        }
    }
}