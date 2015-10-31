using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using tw.moa.input;
using tw.moa.win32api.user32;

namespace tw.moa.helper
{
    public class Win32Helper
    {
        /// <summary> Get the text for the window pointed to by hWnd </summary>
        public static string GetWindowText(IntPtr hWnd)
        {
            int size = Extern.GetWindowTextLength(hWnd);
            if (size > 0)
            {
                var builder = new StringBuilder(size + 1);
                Extern.GetWindowText(hWnd, builder, builder.Capacity);
                return builder.ToString();
            }

            return String.Empty;
        }

        /// <summary> Find all windows that match the given filter </summary>
        /// <param name="filter"> A delegate that returns true for windows
        ///    that should be returned and false for windows that should
        ///    not be returned </param>
        public static IEnumerable<IntPtr> FindWindows(EnumWindowsProc filter)
        {
            IntPtr found = IntPtr.Zero;
            List<IntPtr> windows = new List<IntPtr>();

            Extern.EnumWindows(delegate(IntPtr wnd, IntPtr param)
            {
                if (filter(wnd, param))
                {
                    // only add the windows that pass the filter
                    windows.Add(wnd);
                }

                // but return true here so that we iterate all windows
                return true;
            }, IntPtr.Zero);

            return windows;
        }

        /// <summary> Find all windows that contain the given title text </summary>
        /// <param name="titleText"> The text that the window title must contain. </param>
        public static IEnumerable<IntPtr> FindWindowsWithText(string titleText)
        {
            return FindWindows(delegate(IntPtr wnd, IntPtr param)
            {
                return GetWindowText(wnd).Contains(titleText);
            });
        }

        public static void MoveWindow(IntPtr hwnd, int x, int Y, int cx, int cy)
        {
            WINDOWINFO pwi;
            Extern.GetWindowInfo(hwnd, out pwi);
            Rectangle resolution = Screen.PrimaryScreen.Bounds;
#if DEBUG
            Console.WriteLine(String.Format("Screen {0},{1},{2},{3}", resolution.Left.ToString(), resolution.Right.ToString(), resolution.Top.ToString(), resolution.Bottom.ToString()));
            Console.WriteLine(String.Format("Window {0},{1},{2},{3}", pwi.rcClient.left.ToString(), pwi.rcClient.right.ToString(), pwi.rcClient.top.ToString(), pwi.rcClient.bottom.ToString()));
#endif
            int left = resolution.Right - resolution.Left - pwi.rcClient.right + pwi.rcClient.left;
            int bottom = resolution.Bottom - resolution.Top - pwi.rcClient.bottom + pwi.rcClient.top;
#if DEBUG
            Console.WriteLine("Move Window {0}, {1}", left, bottom);
#endif
            Extern.SetWindowPos(hwnd, 0, left, 0, 350, resolution.Bottom - 50, Const.SWP_NOZORDER | Const.SWP_NOSIZE | Const.SWP_SHOWWINDOW);

        }

        public static void MoveWindow(IntPtr hwnd)
        {
            WINDOWINFO pwi;
            Extern.GetWindowInfo(hwnd, out pwi);
            Rectangle resolution = Screen.PrimaryScreen.Bounds;
#if DEBUG
            Console.WriteLine(String.Format("Screen {0},{1},{2},{3}", resolution.Left.ToString(), resolution.Right.ToString(), resolution.Top.ToString(), resolution.Bottom.ToString()));
            Console.WriteLine(String.Format("Window {0},{1},{2},{3}", pwi.rcClient.left.ToString(), pwi.rcClient.right.ToString(), pwi.rcClient.top.ToString(), pwi.rcClient.bottom.ToString()));
#endif
            int left = resolution.Right - resolution.Left - pwi.rcClient.right + pwi.rcClient.left;
            int bottom = resolution.Bottom - resolution.Top - pwi.rcClient.bottom + pwi.rcClient.top;
#if DEBUG
            Console.WriteLine("Move Window {0}, {1}", left, bottom);
#endif
            Extern.SetWindowPos(hwnd, 0, left, 0, 350, resolution.Bottom - 50, Const.SWP_NOZORDER | Const.SWP_NOSIZE | Const.SWP_SHOWWINDOW);

        }
        
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);
        public delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool EnumChildWindows(IntPtr hwndParent, EnumWindowsProc lpEnumFunc, IntPtr lParam);
        public static List<IntPtr> GetChildWindows(IntPtr parent)
        {
            List<IntPtr> result = new List<IntPtr>();
            GCHandle listHandle = GCHandle.Alloc(result);
            try
            {
                EnumWindowsProc childProc = new EnumWindowsProc(EnumWindow);
                EnumChildWindows(parent, childProc, GCHandle.ToIntPtr(listHandle));
            }
            finally
            {
                if (listHandle.IsAllocated)
                    listHandle.Free();
            }
            return result;
        }

        public static bool EnumWindow(IntPtr handle, IntPtr pointer)
        {
            GCHandle gch = GCHandle.FromIntPtr(pointer);
            List<IntPtr> list = gch.Target as List<IntPtr>;
            if (list == null)
            {
                throw new InvalidCastException("GCHandle Target could not be cast as List<IntPtr>");
            }
            list.Add(handle);
            // You can modify this to check to see if you want to cancel the operation, then return a null here
            return true;
        }
        //#if TRUE
        static void Main(string[] args)
        {
            StringBuilder sb = new StringBuilder(1024);
            Win32Helper helper = new Win32Helper();
            IntPtr windows = Extern.FindWindow(null, "tsmgg12");
            GetClassName(windows, sb, sb.Capacity);
            Console.WriteLine(String.Format("main: {0:X8}:{1}", windows.ToInt32(), sb));
            sb.Remove(0, sb.Length);
            List<IntPtr> list = GetChildWindows(windows);
            list.ForEach(delegate(IntPtr ip){
                //Console.WriteLine(ip);
                GetClassName(ip, sb, sb.Capacity);
                Console.WriteLine(String.Format("sub:  {0:X8}:{1}", ip.ToInt32(), sb));
                sb.Remove(0, sb.Length);
                Extern.SendMessage(ip, Const.WM_GETTEXT, IntPtr.Zero, sb);
                Console.WriteLine(String.Format("text: {0:X8}:{1}", ip.ToInt32(), sb));
                sb.Remove(0, sb.Length);
            });
            Extern.SendMessage(new IntPtr(6359544), Const.WM_GETTEXT, IntPtr.Zero, sb);
            Console.WriteLine(String.Format("aaaa: {0:X8}:{1}", 6359544, sb));
            sb.Remove(0, sb.Length);

            //IntPtr textboxHandle = Extern.FindWindowEx(windows, IntPtr.Zero, "TextBoxClass", null);
            //Console.WriteLine(String.Format("{0:X8}", textboxHandle));
            //string message = "Hello World!";

            //Extern.SendMessage(textboxHandle, Const.WM_SETTEXT, IntPtr.Zero, message);
            //windows.ForEach(delegate(IntPtr window)
            //{
            //    Console.WriteLine(window);
            //});
            Console.ReadKey();
        }
        //#endif
    }
}