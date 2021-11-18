using VoiceCommand.Input;

namespace VoiceCommand
{
    internal struct Command
    {
        public Command(string commandPhrase, Scancode[] inputs)
        {
            CommandPhrase = commandPhrase;
            Inputs = inputs;
        }

        public readonly string CommandPhrase;
        public readonly Scancode[] Inputs;
    }
}