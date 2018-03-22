using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace autoClick
{
    class Click
    {
        public Int32 ClickTimes = 0;
        public Boolean check214 = false;
        public struct HwndInfo
        {
            public IntPtr Hwnd;//句柄
            public IntPtr Hdc;//设备
            public HwndInfo(IntPtr hwnd, IntPtr hdc)
                : this()
            {
                this.Hwnd = hwnd;
                this.Hdc = hdc;
            }
        }
        public Point TestPoint=new Point();
        public Boolean TestFlag = false;
        public struct PointInfo
        {
            public Point point; // 点信息
            public int clickInterval; // 点击间隔
            public string hexColorValue; // 16进制色值
            public string windowText;
            public Boolean status;
            public PointInfo(Point point)
                : this()
            {
                this.status = false;
                this.point = point;
            }

            public PointInfo(Point point, int clickInterval)
                : this()
            {
                this.status = false;
                this.point = point;
                this.clickInterval = clickInterval;
            }

            public PointInfo(Point point, int clickInterval, String windowtext, Boolean status,int i)
                : this()
            {
                //this.status = false;
                this.point = point;
                this.clickInterval = clickInterval;
                this.windowText = windowtext;
                this.status = status;
            }

            public PointInfo(Point point, int clickInterval, string hexColorValue, String windowtext,Boolean status)
                : this()
            {
                //this.status = false;
                this.point = point;
                this.clickInterval = clickInterval;
                this.hexColorValue = hexColorValue;
                this.windowText = windowtext;
                this.status = status;
            }
        };
        public Dictionary<String, HwndInfo> hwndDic = new Dictionary<string, HwndInfo>();
        public const string UNENABLE = "未开启";
        public const string ENABLE = "已开启";
        public string CLICKER_HEROES = "Clicker Heroes";
        public void SetWindowText(String text)
        {
            this.CLICKER_HEROES = text;
        }
        // 是否开始连点
        public bool isStart = false;

        Thread thread;
        private List<Thread> ThreadList = new List<Thread>();
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
        public void doClicks(PointInfo p)
        {
            if (hwndDic.ContainsKey(p.windowText))
            {
                Thread t = new Thread(new ParameterizedThreadStart(StartClicksThread));
                ThreadList.Add(t);
                t.Start(p);
            }
        }
        public void StartClicksThread(object point)
        {
            PointInfo p = (PointInfo)point;
            Int32 clicktimes = ClickTimes;
            
            if (hwndDic.ContainsKey(p.windowText))
            {
                HwndInfo hwi = hwndDic[p.windowText];
                for (int i = 0; i < clicktimes; i++)
                {
                    clickMouse(hwi.Hwnd, p.point.X, p.point.Y);
                    Thread.Sleep((int)this.scanInterval);
                }
            }
        }
        private void cleanThreadList()
        {
            for (int i = 0; i < ThreadList.Count; i++)
            {
                if (ThreadList[i].ThreadState == ThreadState.Stopped)
                {
                    ThreadList[i] = null;
                    ThreadList.RemoveAt(i--);
                }
            }
        }
        public HwndInfo getHwndInfo(String windowtext)
        {
            IntPtr hwnd = getHandle(windowtext);
            IntPtr hdc = WinApi.GetDC(hwnd);
            return new HwndInfo(hwnd, hdc);
        }
        public void checkClikHero()
        {
            if (hwndDic.ContainsKey("Clicker Heroes"))
            {
                System.IO.File.AppendAllText("click.log", String.Format("{0}:颜色检测开始\r\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                HwndInfo hd = hwndDic["Clicker Heroes"];
                IntPtr hwnd = hd.Hwnd;
                IntPtr hdc = hd.Hdc;
                Bitmap bmp = null;
                Int32 maxX = 0;
                Int32 MaxY = 0;
                check214 = check214Date();
                WinApi.Rect rect = new WinApi.Rect();
                try
                {
                    /*
                     WinApi.ShowWindow(hwnd, WinApi.CmdShow_Show);
                    //Thread.Sleep(10);
                    while(maxX<=0)
                    {
                        WinApi.GetWindowRect(hwnd, out rect);
                        maxX = rect.Right - rect.Left;
                        MaxY = rect.Bottom - rect.Top;
                        Thread.Sleep(10);
                    }
                    bmp=GetBitmapFromDC(hwnd, hdc, maxX, MaxY);
                    WinApi.ShowWindow(hwnd, WinApi.CmdShow_Min);
                    */

                    WinApi.GetWindowRect(hwnd, out rect);
                    maxX = rect.Right - rect.Left;
                    MaxY = rect.Bottom - rect.Top;
                    if (maxX <= 500)
                    {
                        WinApi.ShowWindow(hwnd, WinApi.CmdShow_Show);
                        Int32 times = 0;
                        while (maxX <= 500&&times<10)
                        {
                            WinApi.GetWindowRect(hwnd, out rect);
                            maxX = rect.Right - rect.Left;
                            MaxY = rect.Bottom - rect.Top;
                            times++;
                            Thread.Sleep(100);
                        }
                    }
                    test();
                    bmp = GetBitmapFromDC(hwnd, hdc, maxX, MaxY);
                    //tempcolor = getHexColorValue(hdc, new Point(332, 226));
                    HSVColor hsv = new HSVColor();
                    Color color = new Color();
                    Point point = new Point();
                    for (int X = 1; X < maxX; X = X + 2)
                    {
                        point.X = X;
                        for (int Y = 1; Y < MaxY; Y = Y + 2)
                        {
                            point.Y = Y;
                            color = bmp.GetPixel(point.X, point.Y);
                            hsv.H = color.GetHue();
                            hsv.S = color.GetSaturation();
                            hsv.V = color.GetBrightness();
                            //hsv = getHSVColorValue(hdc, point);
                            //Int32 H = (int)Math.Round(hsv.H, 0);
                            if (HSVCheck(hsv, point))
                            {
                                System.IO.File.AppendAllText("click.log", String.Format("{0}:颜色符合点颜色值H:{1}S:{2}V:{3}\r\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                    , hsv.H
                    , hsv.S
                    , hsv.V));
                                System.IO.File.AppendAllText("click.log", String.Format("{0}:发现颜色符合，位置（{1},{2}）\r\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), X.ToString(), Y.ToString()));
                                if (X < 1098)
                                { 
                                    clickMouse(hwnd, X, Y);
                                    //Y = Y + 6;
                                    //点击后重新获取图像。
                                    bmp.Dispose();
                                    bmp = null;
                                    Thread.Sleep(100);
                                    bmp=  GetBitmapFromDC(hwnd, hdc, maxX, MaxY);
                                }

                                //System.IO.File.AppendAllText("click.log", String.Format("{0}:颜色检测结束\r\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                                //return;
                            }
                            /*String tempcolor = getHexColorValue(hdc, point);
                            if (tempcolor == "F66B14" || tempcolor == "FA6F18" || tempcolor == "F78401" || tempcolor == "EF480A")
                            {

                                System.IO.File.AppendAllText("click.log", String.Format("{0}:发现颜色符合，位置（{1},{2}）\r\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), X.ToString(), Y.ToString()));
                                clickMouse(hwnd, X, Y);
                                System.IO.File.AppendAllText("click.log", String.Format("{0}:颜色检测结束\r\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                                return;
                            }*/
                        }
                    }
                }
                catch
                { }
                finally
                {
                    if(bmp!=null)
                    {
                        bmp.Dispose();
                        bmp = null;
                    }
                }
                System.IO.File.AppendAllText("click.log", String.Format("{0}:颜色检测结束\r\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            }
            
        }
        private Boolean DataCompare(float A, double value1, double value2)
        {
            return (A >= value1 && A <= value2);
        }
        private Boolean HSVCheck(HSVColor hsv, Point point)
        {
            Boolean result = false;
            Int32 H = (int)Math.Round(hsv.H, 0);
            if(((H == 23 || H == 20) && DataCompare(hsv.S,0.9,0.99) && DataCompare(hsv.V, 0.5, 0.7)) || (H >= 22 && H <= 24 && DataCompare(hsv.S, 0.98, 1) && DataCompare(hsv.V, 0.4, 0.5)))
                result = true;
            if (point.X > 1098)
                result = false;
            
            if ((H == 47 && DataCompare(hsv.S, 0.18, 0.19) && DataCompare(hsv.V, 0.39, 0.40)) || ((H == 45||H==44) && DataCompare(hsv.S, 0.22, 0.33) && DataCompare(hsv.V, 0.6, 0.72)))
            {
                result = true;
                if(point.X >= 1071 && point.X <= 1123 && point.Y >= 282 && point.Y <= 327)
                    result = false;
            }
               
            #region --check 2.14heart
            if (check214)
            {
                if ((H == 0 && hsv.S > 0.6 && hsv.S < 0.7 && hsv.V > 0.4 && hsv.V < 0.5) && check214heart(point))
                    result = true;
            }
            #endregion
            return result;
        }
        public Boolean check214Date()
        {
            DateTime date = DateTime.Now;
            DateTime date214 = Convert.ToDateTime(String.Format("{0}-2-14", date.Year.ToString()));
            TimeSpan t = date.Subtract(date214);
            if (t.TotalDays > -4 && t.TotalDays < 2)
                return true;
            return false;
        }
        public Boolean check214heart(Point point)
        {
            Boolean result = true;
            if (point.X >= 9 && point.X <= 37 && point.Y >= 29 && point.Y <= 47)
                result = false;
            if ((point.X >= 491 && point.X <= 497 && point.Y >= 469 && point.Y <= 499) || (point.X >= 499 && point.X <= 501 && point.Y >= 417 && point.Y <= 421))
                result = false;
            if (point.X >= 543 && point.X <= 569 && point.Y >= 33 && point.Y <= 53)
                result = false;
            if (point.X >= 587 && point.X <= 627 && point.Y >= 570 && point.Y <= 610)
                result = false;
            if ((point.X == 591 && point.Y == 599) || (point.X == 593 && point.Y == 599))
                result = false;
            return result;
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
            String widowtext = "";
            foreach (int interval in intervalPointDic.Keys)
            {
                intervalStampDic.Add(interval, getUnixTimestamp());
                List<PointInfo> temp = intervalPointDic[interval];
                for (int i = 0; i < temp.Count; i++)
                {
                    if (temp[i].windowText == null || temp[i].windowText == "")//兼容以前无windtext对象的记录
                    {
                        widowtext = "Clicker Heroes";
                    }
                    else
                        widowtext = temp[i].windowText;
                    if (!hwndDic.ContainsKey(widowtext))
                    {
                        HwndInfo hd = getHwndInfo(widowtext);
                        if (hd.Hdc != IntPtr.Zero)
                            hwndDic.Add(widowtext, getHwndInfo(widowtext));
                    }
                }
            }

            int time = (int)scanInterval;
            Int64 tmpTime;
            string tempcolor = "";
            while (isStart)
            {
                foreach (int interval in intervalPointDic.Keys)
                {
                    tmpTime = getUnixTimestamp();
                    if (tmpTime - intervalStampDic[interval] >= interval)
                    {
                        intervalStampDic[interval] = tmpTime;
                        for (int i = 0; i < intervalPointDic[interval].Count; i++)
                        {
                            Click.PointInfo pointInfo = intervalPointDic[interval][i];
                            IntPtr hwnd = IntPtr.Zero;
                            IntPtr hdc = IntPtr.Zero;
                            if (pointInfo.windowText == null || pointInfo.windowText == "")//兼容以前无windtext对象的记录
                            {
                                widowtext = "Clicker Heroes";
                            }
                            else
                                widowtext = pointInfo.windowText;
                            if (hwndDic.ContainsKey(widowtext))
                            {
                                hwnd = hwndDic[widowtext].Hwnd;
                                hdc = hwndDic[widowtext].Hdc;
                            }
                            // 若有颜色条件且色值不相等，则放弃此次点击
                            if (pointInfo.hexColorValue != null)
                                tempcolor = getHexColorValue(hdc, pointInfo.point);
                           /* if (pointInfo.hexColorValue != null &&
                                pointInfo.hexColorValue.Equals(tempcolor))
                            {
                                continue;
                            }*/
                            if (pointInfo.hexColorValue != null &&
                               !pointInfo.hexColorValue.Equals(tempcolor))
                            {
                                //System.IO.File.AppendAllText("click.log", String.Format("{0}:发现颜色改变\r\n",DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                                //pointInfo.hexColorValue = tempcolor;
                                continue;

                            }
                            if(!pointInfo.status)
                                clickMouse(hwnd, pointInfo.point.X, pointInfo.point.Y);
                        }
                    }
                }
                cleanThreadList();
                Thread.Sleep(time);
            }

            // 释放DC
            foreach (String key in hwndDic.Keys)
            {
                WinApi.ReleaseDC(hwndDic[key].Hwnd, hwndDic[key].Hdc);
            }
            hwndDic.Clear();
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
                // MessageBox.Show("没有找到对应的窗口");
            }

            return hwnd;
        }
        /**
         * 获取句柄
         */
        public IntPtr getHandle(String widowText)
        {
            // 【1】找到窗口
            IntPtr hwnd = WinApi.FindWindow(null, widowText); // CLICKER_HEROES
            if (hwnd == IntPtr.Zero)
            {
                // MessageBox.Show("没有找到对应的窗口");
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
            pointInfo.windowText = this.CLICKER_HEROES;
            pointInfo.status = false;
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
            if (h == IntPtr.Zero)
                return;
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
            if (TestFlag)
            {
                HwndInfo hd = hwndDic["Clicker Heroes"];
                IntPtr hwnd = hd.Hwnd;
                IntPtr hdc = hd.Hdc;

                // UpdateWindow(hwnd);

                //uint pixel = WinApi.GetPixel(hdc, 1107, 242);

                //Color color = getColorValue(hdc, new Point(220, 198));
                String s = getHexColorValue(hdc, TestPoint);

                HSVColor hsv = getHSVColorValue(hdc, TestPoint);
                Int32 H = (int)Math.Round(hsv.H, 0);
                System.IO.File.AppendAllText("click.log", String.Format("{0}:颜色测试点值{1},H:{2}S:{3}V:{4};(int)H-{5}\r\n"
                    , DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                    , s
                    , hsv.H
                    , hsv.S
                    , hsv.V
                    , H));
                /* for (Int32 X = 1000; X < 1046; X++)
                 {
                     point.X = X;
                     for (Int32 Y = 591; Y < 633; Y++)
                     {
                         point.Y = Y;
                         String s = getHexColorValue(hdc, point);
                         HSVColor hsv = getHSVColorValue(hdc, point);
                         System.IO.File.AppendAllText("click.log", String.Format("{0}:颜色测试点值{1},H:{2}S:{3}V:{4}\r\n"
                             , DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                             , s
                             , hsv.H
                             , hsv.S
                             , hsv.V));
                     }
                 }*/
                //WinApi.ReleaseDC(hwnd, hdc);
            }
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
        public struct HSVColor
        {
            public float H;
            public float S;
            public float V;
        }
        /**
         * 获取对应点十六进制色值
         */
        public HSVColor getHSVColorValue(IntPtr hdc, Point point)
        {
            uint pixelColor = WinApi.GetPixel(hdc, point.X, point.Y); // 0x00bbggrr
            //uint hexColorValue = ((pixelColor & 0x000000FF) << 16) + (pixelColor & 0x0000FF00) + ((pixelColor & 0x00FF0000) >> 16); // rrggbb
            Color a = Color.FromArgb(Convert.ToInt32(pixelColor & 0x000000FF), Convert.ToInt32((pixelColor & 0x0000FF00) >> 8), Convert.ToInt32((pixelColor & 0x00FF0000) >> 16));
            HSVColor hsv = new HSVColor();
            hsv.H = a.GetHue();
            hsv.S = a.GetSaturation();
            hsv.V = a.GetBrightness();
            return hsv;
        }
        /**
         * 获取对应点十六进制色值
         */
        public Color getColorValue(IntPtr hdc, Point point)
        {
            HwndInfo hd = getHwndInfo("Clicker Heroes");
            IntPtr hwnd1 = hd.Hwnd;
            IntPtr hdc1 = hd.Hdc;
            uint pixelColor = WinApi.GetPixel(hdc1, point.X, point.Y); // 0x00bbggrr
            WinApi.ReleaseDC(hwnd1, hdc1);
            uint hexColorValue = ((pixelColor & 0x000000FF) << 16) + (pixelColor & 0x0000FF00) + ((pixelColor & 0x00FF0000) >> 16); // rrggbb

            string s = Convert.ToString(hexColorValue, 16).ToUpper();
            int l = (int)(pixelColor & 0xFF000000) >> 24;
            Color a;
            if (l > 0)
                a = Color.FromArgb(l,
                                 (int)(pixelColor & 0x000000FF),
                                 (int)(pixelColor & 0x0000FF00) >> 8,
                                 (int)(pixelColor & 0x00FF0000) >> 16);
            else
                a = Color.FromArgb(
                             (int)(pixelColor & 0x000000FF),
                             (int)(pixelColor & 0x0000FF00) >> 8,
                             (int)(pixelColor & 0x00FF0000) >> 16);

            return a;
        }
        /// <summary>
        /// 获取截图
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public Bitmap GetBitmapFromDC(IntPtr hwnd, IntPtr windc, int width, int height)
        {
            //IntPtr windc = WinApi.GetDC(hwnd);//获取窗口DC
            IntPtr hDCMem = WinApi.CreateCompatibleDC(windc);//为设备描述表创建兼容的内存设备描述表
            IntPtr hbitmap = WinApi.CreateCompatibleBitmap(windc, width, height);
            IntPtr hOldBitmap = (IntPtr)WinApi.SelectObject(hDCMem, hbitmap);
            WinApi.BitBlt(hDCMem, 0, 0, width, height, windc, 0, 0, WinApi.SRCCOPY);
            hbitmap = (IntPtr)WinApi.SelectObject(hDCMem, hOldBitmap);
            Bitmap bitmap = Bitmap.FromHbitmap(hbitmap);
            WinApi.DeleteObject(hbitmap);//删除用过的对象
            WinApi.DeleteObject(hOldBitmap);//删除用过的对象
            WinApi.DeleteDC(hDCMem);//删除用过的对象
            //WinApi.ReleaseDC(hwnd, windc);
            return bitmap;
        }
    }
}