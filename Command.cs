using VoiceCommand.Input;

namespace VoiceCommand
{
    internal struct Command
    {
        public Command(string commandPhrase, InputAction[] inputs)
        {
            CommandPhrase = commandPhrase;
            Inputs = inputs;
        }

        public readonly string CommandPhrase;
        public readonly InputAction[] Inputs;
    }
}