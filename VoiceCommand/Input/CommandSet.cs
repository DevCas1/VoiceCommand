namespace VoiceCommand.Input;

/// <summary>A container for a set of <see cref="Command"/>s.</summary>
/// <param name="name">Name of the Command Set.</param>
/// <param name="commands">The <see cref="Command"/>s contained in this <see cref="CommandSet"/>.</param>
internal class CommandSet(string name, Command[] commands)
{
    public readonly string Name = name;
    public readonly Command[] Commands = commands;
}