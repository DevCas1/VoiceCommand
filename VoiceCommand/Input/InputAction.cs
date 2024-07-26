namespace VoiceCommand.Input;

/// <summary>A collection of Keyboard Scan Codes, whether the key was pressed down or released, and it's duration in miliseconds.</summary>
/// <param name="scanCode">Indicates which key to press or release.</param>
/// <param name="keyDown">True to press key, false to release.<br/>An identical keyDown value following the same <see cref="ScanCode"/> could have unintended effects and isn't tested!</param>
/// <param name="duration">The duration in miliseconds before this Input Action is considered done.</param>
public readonly struct InputAction(ScanCode scanCode, bool keyDown, float duration)
{
    public readonly ScanCode Scancode = scanCode;
    public readonly bool KeyDown = keyDown;
    public readonly float Duration = duration;

    public InputAction(ScanCode scanCode, bool keyDown) : this(scanCode, keyDown, 0) { }
}