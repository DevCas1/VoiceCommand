namespace VoiceCommand.Input;

internal readonly struct Command(string name, string commandPhrase, InputAction[] inputActions)
{
    public readonly string Name = name;
    public readonly string CommandPhrase = commandPhrase;
    public readonly InputAction[] InputActions = inputActions;
}