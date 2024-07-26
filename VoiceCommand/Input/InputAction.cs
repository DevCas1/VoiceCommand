namespace VoiceCommand.Input;

/// <summary>A collection of Keyboard Scancode and the duration for it to hold.</summary>
public readonly struct InputAction
{
    public InputAction(ScanCode scancode, bool keyDown, float duration)
    {
        Scancode = scancode;
        KeyDown = keyDown;
        Duration = duration;
    }

    public InputAction(ScanCode scancode, bool keyDown)
    {
        Scancode = scancode;
        KeyDown = keyDown;
        Duration = 0;
    }

    public readonly ScanCode Scancode;
    public readonly bool KeyDown;
    public readonly float Duration;
}