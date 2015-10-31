using System;
using System.Runtime.InteropServices;

namespace tw.moa.input
{
    public enum INPUT_TYPE : uint
    {
        INPUT_MOUSE = 0,
        INPUT_KEYBOARD = 1,
        INPUT_HARDWARE = 2
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct INPUT
    {
        public INPUT_TYPE type;
        public InputUnion U;
        public static int Size
        {
            get { return Marshal.SizeOf(typeof(INPUT)); }
        }
    }

}
