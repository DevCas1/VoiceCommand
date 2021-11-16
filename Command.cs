namespace VoiceCommand
{
    internal class Command
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