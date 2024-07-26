namespace VoiceCommand.Input;

/// <summary>A collection of Keyboard Scancode and the duration for it to hold.</summary>
public readonly struct InputAction(ScanCode scancode, bool keyDown, float duration)
{
    public readonly bool KeyDown = keyDown;
    public readonly float Duration = duration;
    public readonly ScanCode Scancode = scancode;

    public InputAction(ScanCode scancode, bool keyDown) : this(scancode, keyDown, 0) { }
}