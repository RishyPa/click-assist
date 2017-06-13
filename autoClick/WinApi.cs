using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace autoClick
{
    class WinApi
    {
        public const int MK_LBUTTON = 0x01; // 鼠标左键点击事件
        public const int MOUSEEVENTF_LEFTDOWN = 0x02;// 鼠标按下事件
        public const int MOUSEEVENTF_LEFTUP = 0x04;// 鼠标放开事件
        public const int WM_MOUSEMOVE = 0x0200; // 鼠标移动事件
        public const int WM_LBUTTONDOWN = 0x0201; //Left mousebutton down
        public const int WM_LBUTTONUP = 0x0202;  //Left mousebutton up
        public const int HOTKEY_ID_F9 = 100;// F9快捷键ID
        public const int HOTKEY_ID_F10 = 101;// F10快捷键ID
        public const int HOTKEY_ID_F11 = 102;// F11快捷键ID
        public const int WM_HOTKEY = 0x0312;// 按快捷键 

        [Flags()]
        public enum KeyModifiers
        {
            None = 0,
            Alt = 1,
            Control = 2,
            Shift = 4,
            Windows = 8
        }

        public struct Rect
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        // 注册热键
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool RegisterHotKey(
            IntPtr hWnd, // handle to window   
            int id, // hot key identifier   
            KeyModifiers fsModifiers, // key-modifier options   
            Keys vk // virtual-key code   
        );

        // 注销热键
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool UnregisterHotKey(
            IntPtr hWnd, // handle to window   
            int id // hot key identifier   
        );

        // 鼠标事件
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(
            uint dwFlags,
            uint dx,
            uint dy,
            uint cButtons,
            uint dwExtraInfo
        );

        // 获取窗口句柄
        [DllImport("user32.dll", EntryPoint = "FindWindow")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        // 获取子窗口句柄
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern IntPtr FindWindowEx(IntPtr parentHandle, IntPtr childAfter, string lclassName, string windowTitle);

        // 获取窗口坐标
        [DllImport("user32.dll")]
        public static extern int GetWindowRect(IntPtr hwnd, out Rect lpRect);

        [DllImport("user32.dll")]
        static extern bool ClientToScreen(IntPtr hWnd, ref Point lpPoint);

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, uint wMsg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern bool PostMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);

        public static int MakeLParam(int LoWord, int HiWord)
        {
            return (int)((HiWord << 16) | (LoWord & 0xFFFF));
        }

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr GetWindow(IntPtr hWnd, uint uCmd);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        [DllImport("user32.dll")]
        public static extern bool IsZoomed(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern bool IsIconic(IntPtr hWnd);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetCursorPos(out Point lpPoint);

        [DllImport("user32.dll")]
        public static extern IntPtr GetDC(IntPtr hwnd);

        [DllImport("user32.dll")]
        public static extern Int32 ReleaseDC(IntPtr hwnd, IntPtr hdc);

        [DllImport("gdi32.dll")]
        public static extern uint GetPixel(IntPtr hdc, int nXPos, int nYPos);

        [DllImport("user32.dll")]
        public static extern bool UpdateWindow(IntPtr hwnd);
    }
}
