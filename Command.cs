namespace VoiceCommand
{
    internal struct Command
    {
        public Command(string commandPhrase, string[] actions)
        {
            CommandPhrase = commandPhrase;
            Actions = actions;
        }

        public readonly string CommandPhrase;
        public readonly string[] Actions;
    }
}