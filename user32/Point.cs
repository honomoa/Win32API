using System;
using System.Runtime.InteropServices;
using System.Drawing;

namespace tw.moa.win32api.user32
{
    [StructLayout(LayoutKind.Sequential)]
    public struct POINT
    {
        public int X;
        public int Y;

        public static implicit operator Point(POINT point)
        {
            return new Point(point.X, point.Y);
        }
    }

}
