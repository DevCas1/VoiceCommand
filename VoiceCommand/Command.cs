using VoiceCommand.Input;

namespace VoiceCommand
{
    internal readonly struct Command(string commandPhrase, InputAction[] inputActions)
    {
        public readonly string CommandPhrase = commandPhrase;
        public readonly InputAction[] InputActions = inputActions;
    }
}