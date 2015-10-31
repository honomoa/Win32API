using System;
using System.Collections.Generic;
using System.Text;

namespace tw.moa.win32api.user32
{
    // Delegate to filter which windows to include 
    public delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);
}
