namespace VoiceCommand.Input;

public class VoiceCommandConfig()
{
    public string Version { get; set; } = string.Empty;
    public string Language { get; set; } = string.Empty;
    public List<Application> Applications { get; set; } = [];
}

/// <summary>A container for a set of <see cref="CommandSet"/>s specific to an application.</summary>
/// <param name="name">Name of the Application.</param>
/// <param name="commandSets">The configured <see cref="CommandSet"/>s for a given <see cref="Application"/>.</param>
public struct Application()
{
    public string Name { get; set; } = string.Empty;
    public List<CommandSet> CommandSets { get; set; } = [];
}

/// <summary>A container for a set of <see cref="Command"/>s.</summary>
/// <param name="name">Name of the Command Set.</param>
/// <param name="commands">The <see cref="Command"/>s contained in this <see cref="CommandSet"/>.</param>
public class CommandSet()
{
    public string Name { get; set; } = string.Empty;
    public List<Command> Commands { get; set; } = [];
}

/// <summary>A collection of Command Phrase, and the associated Input Actions.</summary>
public struct Command()
{
    public string Name { get; set; } = string.Empty;
    public string CommandPhrase { get; set; } = string.Empty;
    public List<InputAction> InputActions { get; set; } = [];
}

/// <summary>A collection of Keyboard Scan Codes, whether the key was pressed down or released, and it's duration in miliseconds.</summary>
/// <param name="scanCode">Indicates which key to press or release.</param>
/// <param name="keyDown">True to press key, false to release.<br/>An identical keyDown value following the same <see cref="ScanCode"/> could have unintended effects and isn't tested!</param>
/// <param name="duration">The duration in miliseconds before this Input Action is considered done.</param>
public struct InputAction()
{
    public ScanCode Scancode { get; set; }
    public bool KeyDown { get; set; }
    public float Duration { get; set; }
}