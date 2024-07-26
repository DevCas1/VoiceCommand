using System;
using System.Runtime.InteropServices;

namespace VoiceCommand.Input;

internal class Keyboard
{
    public static void SendInputs(InputAction[] actions)
    {
        Input[] inputs = new Input[actions.Length];

        for (int index = 0; index < actions.Length; index++)
        {
            var action = actions[index];
            inputs[index] = GetKBInputDownFromScancode(action.Scancode, action.KeyDown);
        }

        SendInput((uint)inputs.Length, inputs, Marshal.SizeOf(typeof(Input)));
    }

    public static ushort ConvertToUInt(ScanCode code) => (ushort)code;

    [DllImport("user32.dll", SetLastError = true)]
    private static extern uint SendInput(uint nInputs, Input[] pInputs, int cbSize);

    [DllImport("user32.dll")]
    private static extern IntPtr GetMessageExtraInfo();

    private static Input GetKBInputDownFromScancode(ScanCode scancode, bool keyDown) => new Input
    {
        type = (int)InputType.Keyboard,
        u = new InputUnion
        {
            ki = new KeyboardInput
            {
                wVk = 0,
                wScan = ConvertToUInt(scancode),
                dwFlags = (uint)((keyDown ? KeyEventF.KeyDown : KeyEventF.KeyUp) | KeyEventF.Scancode),
                dwExtraInfo = GetMessageExtraInfo()
            }
        }
    };

    [StructLayout(LayoutKind.Sequential)]
    private struct KeyboardInput
    {
        public ushort wVk;
        public ushort wScan;
        public uint dwFlags;
        public uint time;
        public IntPtr dwExtraInfo;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct MouseInput
    {
        public int dx;
        public int dy;
        public uint mouseData;
        public uint dwFlags;
        public uint time;
        public IntPtr dwExtraInfo;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct HardwareInput
    {
        public uint uMsg;
        public ushort wParamL;
        public ushort wParamH;
    }

    [StructLayout(LayoutKind.Explicit)]
    private struct InputUnion
    {
        [FieldOffset(0)] public MouseInput mi;
        [FieldOffset(0)] public KeyboardInput ki;
        [FieldOffset(0)] public HardwareInput hi;
    }

    private struct Input
    {
        public int type;
        public InputUnion u;
    }

    [Flags]
    private enum InputType
    {
        Mouse = 0,
        Keyboard = 1,
        Hardware = 2
    }

    [Flags]
    private enum KeyEventF
    {
        KeyDown = 0x0000,
        ExtendedKey = 0x0001,
        KeyUp = 0x0002,
        Unicode = 0x0004,
        Scancode = 0x0008
    }

    [Flags]
    private enum MouseEventF
    {
        Absolute = 0x8000,
        HWheel = 0x01000,
        Move = 0x0001,
        MoveNoCoalesce = 0x2000,
        LeftDown = 0x0002,
        LeftUp = 0x0004,
        RightDown = 0x0008,
        RightUp = 0x0010,
        MiddleDown = 0x0020,
        MiddleUp = 0x0040,
        VirtualDesk = 0x4000,
        Wheel = 0x0800,
        XDown = 0x0080,
        XUp = 0x0100
    }
}