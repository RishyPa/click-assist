using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace autoClick
{
    class Click
    {
        public struct PointInfo
        {
            public Point point; // 点信息
            public int clickInterval; // 点击间隔
            public string hexColorValue; // 16进制色值

            public PointInfo(Point point) : this()
            {
                this.point = point;
            }

            public PointInfo(Point point, int clickInterval) : this()
            {
                this.point = point;
                this.clickInterval = clickInterval;
            }

            public PointInfo(Point point, int clickInterval, string hexColorValue) : this()
            {
                this.point = point;
                this.clickInterval = clickInterval;
                this.hexColorValue = hexColorValue;
            }
        };

        public const string UNENABLE = "未开启";
        public const string ENABLE = "已开启";
        public const string CLICKER_HEROES = "Clicker Heroes";

        // 是否开始连点
        bool isStart = false;

        Thread thread;

        // 点击间隔
        uint scanInterval;

        /**
         * 点击线程任务
         * 
         */
        public void clickScheduler(string interval, Label label, Dictionary<int, List<PointInfo>> intervalPointDic)
        {
            // 转换为数字
            if (!uint.TryParse(interval, out scanInterval))
            {
                return;
            }

            // 线程启用或终止
            if (thread != null && thread.IsAlive)
            {
                isStart = false;
                label.Text = UNENABLE; // 未开启
                label.ForeColor = Color.Red;
            }
            else
            {
                isStart = true;
                label.Text = ENABLE; // 已开启
                label.ForeColor = Color.Green;
                thread = new Thread(new ParameterizedThreadStart(doClick));
                thread.Start(intervalPointDic);//调用主处理程序
            }
        }

        /**
         * 点击主体
         */
        public void doClick(object intervalDicObj)
        {
            Dictionary<int, long> intervalStampDic = new Dictionary<int, long>();
            Dictionary<int, List<PointInfo>> intervalPointDic = (Dictionary<int, List<PointInfo>>)intervalDicObj;

            if (intervalPointDic == null || intervalPointDic.Count == 0)
            {
                return;
            }

            foreach (int interval in intervalPointDic.Keys)
            {
                intervalStampDic.Add(interval, getUnixTimestamp());
            }

            int time = (int) scanInterval;
            IntPtr hwnd = getHandle();
            IntPtr hdc = WinApi.GetDC(hwnd);
            Int64 tmpTime;
            while (isStart)
            {
                foreach (int interval in intervalPointDic.Keys)
                {
                    tmpTime = getUnixTimestamp();
                    if (tmpTime - intervalStampDic[interval] >= interval)
                    {
                        intervalStampDic[interval] = tmpTime;
                        foreach (Click.PointInfo pointInfo in intervalPointDic[interval])
                        {
                            // 若有颜色条件且色值不相等，则放弃此次点击
                            if (pointInfo.hexColorValue != null && 
                                !pointInfo.hexColorValue.Equals(getHexColorValue(hdc, pointInfo.point)))
                            {
                                continue;
                            }
                            clickMouse(hwnd, pointInfo.point.X, pointInfo.point.Y);
                        }
                    }
                }

                Thread.Sleep(time);
            }

            // 释放DC
            WinApi.ReleaseDC(hwnd, hdc);
        }

        /**
         * 获取当前时间时间戳
         */
        private Int64 getUnixTimestamp()
        {
            return (Int64)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalMilliseconds;
        }

        /**
         * 获取句柄
         */
        public IntPtr getHandle()
        {
            // 【1】找到窗口
            IntPtr hwnd = WinApi.FindWindow(null, CLICKER_HEROES); // CLICKER_HEROES
            if (hwnd == IntPtr.Zero)
            {
                MessageBox.Show("没有找到对应的窗口");
            }

            return hwnd;
        }

        /**
          * 获取窗口右上角坐标
          */
        public Point getWindowPoint()
        {
            IntPtr hwnd = getHandle();

            // 【2】获取窗口当前坐标
            WinApi.Rect rect = new WinApi.Rect();
            WinApi.GetWindowRect(hwnd, out rect);

            return new Point(rect.Left, rect.Top);
        }

        /**
         * 获取当前相对点
         */
        public Point getCurrPoint()
        {
            // 当前窗口的位置
            Point windowPoint = getWindowPoint();

            // 获取相对需点击窗口的相对位置
            Point point = new Point();
            point.X = Control.MousePosition.X - windowPoint.X - 8;
            point.Y = Control.MousePosition.Y - windowPoint.Y - 31;

            return point;
        }

        /**
         * 添加点击点
         */
        public Click.PointInfo addPointInfo(bool hasColor)
        {
            Point point = getCurrPoint();

            Click.PointInfo pointInfo = new Click.PointInfo(point);
            if (hasColor)
            {
                IntPtr hwnd = getHandle();
                IntPtr hdc = WinApi.GetDC(hwnd);
                pointInfo.hexColorValue = getHexColorValue(hdc, point);

                WinApi.ReleaseDC(hwnd, hdc);
            }
            //clickMouse(getHandle(), point.X, point.Y);

            return pointInfo;
        }

        /**
         * 添加全屏点击点
         */
        public Click.PointInfo addPointInfoFullScreen(bool hasColor)
        {
            Point point = new Point(Control.MousePosition.X, Control.MousePosition.Y);

            Click.PointInfo pointInfo = new Click.PointInfo(point);
            if (hasColor)
            {
                IntPtr hwnd = new IntPtr(0);
                IntPtr hdc = WinApi.GetDC(hwnd);
                pointInfo.hexColorValue = getHexColorValue(hdc, point);

                WinApi.ReleaseDC(hwnd, hdc);
            }
            //clickMouse(getHandle(), point.X, point.Y);

            return pointInfo;
        }
        /**
         * 鼠标左键点击效果
         */
        public void clickMouse(IntPtr h, int x, int y)
        {
            WinApi.PostMessage(h, WinApi.WM_LBUTTONDOWN, WinApi.MK_LBUTTON, WinApi.MakeLParam(x, y));
            WinApi.PostMessage(h, WinApi.WM_LBUTTONUP, WinApi.MK_LBUTTON, WinApi.MakeLParam(x, y));
            // PostMessage(h, WM_MOUSEMOVE, MK_LBUTTON, MakeLParam(0, 0));
        }

        public void stopClick()
        {
            isStart = false;
        }

        public void test()
        {
            IntPtr hwnd = getHandle();

            IntPtr hdc = WinApi.GetDC(hwnd);

            // UpdateWindow(hwnd);

            uint pixel = WinApi.GetPixel(hdc, 1107, 242);


            Color color = Color.FromArgb((int)(pixel & 0x000000FF),
                             (int)(pixel & 0x0000FF00) >> 8,
                             (int)(pixel & 0x00FF0000) >> 16);


        }

        /**
         * 获取对应点十六进制色值
         */
        public string getHexColorValue(IntPtr hdc, Point point)
        {
            uint pixelColor = WinApi.GetPixel(hdc, point.X, point.Y); // 0x00bbggrr
            uint hexColorValue = ((pixelColor & 0x000000FF) << 16) + (pixelColor & 0x0000FF00) + ((pixelColor & 0x00FF0000) >> 16); // rrggbb

            return Convert.ToString(hexColorValue, 16).ToUpper();
        }
     }
}