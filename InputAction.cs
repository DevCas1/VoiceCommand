namespace VoiceCommand.Input
{
    /// <summary>A collection of Keyboard Scancode and the duration for it to hold.</summary>
    public struct InputAction
    {
        public InputAction(Scancode scancode, bool keyDown, float duration)
        {
            Scancode = scancode;
            KeyDown = keyDown;
            Duration = duration;
        }

        public InputAction(Scancode scancode, bool keyDown)
        {
            Scancode = scancode;
            KeyDown = keyDown;
            Duration = 0;
        }

        public readonly Scancode Scancode;
        public readonly bool KeyDown;
        public readonly float Duration;
    }
}