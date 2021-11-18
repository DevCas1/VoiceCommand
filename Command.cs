namespace VoiceCommand
{
    internal struct Command
    {
        public Command(string commandPhrase, Keyboard.Scancode[] inputs)
        {
            CommandPhrase = commandPhrase;
            Inputs = inputs;
        }

        public readonly string CommandPhrase;
        public readonly Keyboard.Scancode[] Inputs;
    }
}