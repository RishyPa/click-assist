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
        public const int HOTKEY_ID_F8 = 99;// F8快捷键ID
        public const int WM_HOTKEY = 0x0312;// 按快捷键 
        public const int WS_EX_TOOLWINDOW = 0x00000080;//TOOL窗口 不显示桌面图标
        public const int WS_EX_APPWINDOW = 0x00040000;//TOOL窗口 不显示桌面图标
        [Flags()]
        public enum KeyModifiers
        {
            None = 0,
            Alt = 1,
            Control = 2,
            Shift = 4,
            Windows = 8
        }
        /// <summary>
        /// 范围
        /// </summary>
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
        /// <summary>
        /// 显示窗口
        /// </summary>
        /// <param name="hwnd">窗口句柄</param>
        /// <param name="nCmdShow">0关闭窗口 1正常大小显示窗口 2最小化窗口 3最大化窗口</param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern int ShowWindow(IntPtr hwnd, int nCmdShow);
        public const int CmdShow_Close = 0;
        public const int CmdShow_Show = 1;
        public const int CmdShow_Min = 2;
        public const int CmdShow_Max = 3;

        /// <summary>
        /// 设置窗口位置
        /// </summary>
        /// <param name="hWnd">窗口句柄</param>
        /// <param name="hWndInsertAfter">窗口的 Z 顺序，HWND_TOP,HWND_BOTTOM,HWND_TOPMOST,HWND_NOTOPMOST</param>
        /// <param name="x">位置X</param>
        /// <param name="y">位置Y</param>
        /// <param name="Width">宽度</param>
        /// <param name="Height">高度</param>
        /// <param name="flags">选项-SWP_</param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int y, int Width, int Height, int flags);
        /// <summary>
        /// 在前面
        /// </summary>
        public const int HWND_TOP = 0;
        /// <summary>
        /// 在后面
        /// </summary>
        public const int HWND_BOTTOM = 1;
        /// <summary>
        /// 在前面, 位于任何顶部窗口的前面
        /// </summary>
        public const int HWND_TOPMOST = -1;
        /// <summary>
        /// 在后面, 位于其他顶部窗口的后面
        /// </summary>
        public const int HWND_NOTOPMOST = -2;

        public const int SWP_NOZORDER = 0x0004;

        /// <summary>
        /// 移动窗口
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="nWidth"></param>
        /// <param name="nHeight"></param>
        /// <param name="bRepaint"></param>
        /// <returns></returns>
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern int GetWindowLong(IntPtr hWnd, int nlndex);
        [DllImport("user32.dll", SetLastError = true)]
        public static extern int SetWindowLong(IntPtr hWnd, int nlndex, int dwNewLong);
        /*
         GDI32 接口
         */
        public const int CAPTUREBLT = 1073741824;
        public const int SRCCOPY = 0x00CC0020; // BitBlt dwRop parameter  
        [DllImport("gdi32.dll")]
        public static extern bool BitBlt(IntPtr hObject, int nXDest, int nYDest,
            int nWidth, int nHeight, IntPtr hObjectSource,
            int nXSrc, int nYSrc, int dwRop);
        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateCompatibleBitmap(IntPtr hDC, int nWidth,
            int nHeight);
        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateCompatibleDC(IntPtr hDC);
        [DllImport("gdi32.dll")]
        public static extern bool DeleteDC(IntPtr hDC);
        [DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);
        [DllImport("gdi32.dll")]
        public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);
    }
}
