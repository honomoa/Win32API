using System;
using System.Runtime.InteropServices;

namespace tw.moa.win32api.user32
{
    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public Int32 left;
        public Int32 top;
        public Int32 right;
        public Int32 bottom;
    }

}
