using System;
using System.Runtime.InteropServices;

namespace tw.moa.input
{
    [StructLayout(LayoutKind.Explicit)]
    public struct InputUnion
    {
        [FieldOffset(0)]
        public MOUSEINPUT mi;
        [FieldOffset(0)]
        public KEYBOARDINPUT ki;
        [FieldOffset(0)]
        public HARDWAREINPUT hi;
    }

}
