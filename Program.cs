using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Speech.Recognition;

namespace VoiceCommand
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var recognizer = new SpeechRecognizer();
            //var grammar = new Grammar(GrammarBuilder.Add(new Choices(), );
            //recognizer.LoadGrammarAsync(grammar);

            Console.ReadLine();
        }
    }
}
