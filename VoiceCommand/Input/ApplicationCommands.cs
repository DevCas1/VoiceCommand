namespace VoiceCommand.Input;

internal class ApplicationCommands(string applicationName, CommandSet[] commandSets)
{
    public readonly string ApplicationName = applicationName;
    public readonly CommandSet[] CommandSets = commandSets;
}