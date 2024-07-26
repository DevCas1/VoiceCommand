using VoiceCommand.Input;

namespace VoiceCommand
{
    internal readonly struct Command
    {
        public Command(string commandPhrase, InputAction[] inputActions)
        {
            CommandPhrase = commandPhrase;
            InputActions = inputActions;
        }

        public readonly string CommandPhrase;
        public readonly InputAction[] InputActions;
    }
}