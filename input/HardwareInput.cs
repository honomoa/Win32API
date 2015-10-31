using System;
using System.Runtime.InteropServices;

namespace tw.moa.input
{
    [StructLayout(LayoutKind.Sequential)]
    public struct HARDWAREINPUT
    {
        internal int uMsg;
        internal short wParamL;
        internal short wParamH;
    }

}
