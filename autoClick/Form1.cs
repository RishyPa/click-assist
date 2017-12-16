using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Text;

namespace autoClick
{
    public partial class Form1 : Form
    {
        // 存档保存路径 当前目录下的文件
        string path = Application.StartupPath + Path.Combine("\\pointInfo.json");

        // 点击点映射表
        Dictionary<string, List<Click.PointInfo>> pointListDic = new Dictionary<string, List<Click.PointInfo>>();

        // 点击类
        Click click = new Click();

        protected override void WndProc(ref Message m)//监视Windows消息  
        {
            switch (m.Msg)
            {
                case WinApi.WM_HOTKEY:
                    switch (m.WParam.ToInt32())
                    {
                        case WinApi.HOTKEY_ID_F8: click.doClicks(click.addPointInfo(false)); break;
                        case WinApi.HOTKEY_ID_F9  : click.clickScheduler(textBox1.Text, label5, getPointListGroupByInterval()); break;
                        case WinApi.HOTKEY_ID_F10 : addPointToDataView(click.addPointInfo(false)); break;
                        case WinApi.HOTKEY_ID_F11 : 
                            addPointToDataView(click.addPointInfo(true));
                            //addPointToDataView(click.addPointInfoFullScreen(true)); 
                            break;
                    }
                    break;

            }
            base.WndProc(ref m);
        }

        public Form1()
        {
            InitializeComponent();

            // 加载点击点信息
            loadPointInfo();
            click.ClickTimes = Convert.ToInt32(textClickTimes.Text);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            registerAllHotKey();
            this.button1.PerformClick();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            unregisterAllHotKey();
            click.stopClick();
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Hide();
                this.notifyIcon1.Visible = true;
                this.ShowInTaskbar = false;
            }

            bindHotKey();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            click.clickScheduler(textBox1.Text, label5, getPointListGroupByInterval());
            if (click.isStart)
            {
                if (testCheck.Checked)
                {
                    try
                    {
                        click.TestPoint.X = Convert.ToInt32(testPointText.Text.Split(new char[] { ',' })[0]);
                        click.TestPoint.Y = Convert.ToInt32(testPointText.Text.Split(new char[] { ',' })[1]);
                        click.TestFlag = true;
                    }
                    catch { }
                }
                Int32 times = 0;
                while (!click.hwndDic.ContainsKey("Clicker Heroes") && times < 10)
                {
                    System.Threading.Thread.Sleep(100);
                    times++;
                }
                timer1.Start();
                timer1.Interval = 60 * 1000;
                if (!backgroundWorker1.IsBusy)
                    backgroundWorker1.RunWorkerAsync();
            }
            else
            {
                click.TestFlag = false;
                timer1.Stop();
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            this.notifyIcon1.Visible = false;
            this.ShowInTaskbar = true;
            bindHotKey();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.notifyIcon1.Dispose();
            System.Environment.Exit(0);//这是最彻底的退出方式，不管什么线程都被强制退出，把程序结束的很干净。
        }

        /**
         * 添加计划按钮
         */
        private void add_btn_Click(object sender, EventArgs e)
        {
            string title = list_title_text.Text;
            if (!string.IsNullOrWhiteSpace(title) && !comboBox1.Items.Contains(title.Trim()))
            {
                comboBox1.Items.Add(title.Trim());
                comboBox1.SelectedIndex = comboBox1.Items.IndexOf(title);
                switchDataView(getPointIntervalList(pointListDic, title));
                list_title_text.Text = "";

                // 保存信息
                savePointInfo();
            }
        }

        /**
         * 删除计划按钮
         */
        private void del_btn_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == -1)
            {
                return;
            }

            object selectValue = comboBox1.Items[comboBox1.SelectedIndex];
            if (selectValue == null || selectValue.ToString().Trim().Length == 0)
            {
                MessageBox.Show("请选择需删除的计划");
                return;
            }

            DialogResult result = MessageBox.Show("是否删除" + selectValue + "?", "确认删除", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            if (DialogResult.OK == result)
            {
                comboBox1.Items.RemoveAt(comboBox1.SelectedIndex);
                dataGridView1.Rows.Clear();
                pointListDic.Remove(selectValue.ToString());

                // 保存信息
                savePointInfo();
            }
        }

        private void update_btn_Click(object sender, EventArgs e)
        {

            string title = list_title_text.Text;
            if (!string.IsNullOrWhiteSpace(title) && !comboBox1.Items.Contains(title.Trim()))
            {
                pointListDic.Add(title, pointListDic[comboBox1.SelectedItem.ToString()]);
                pointListDic.Remove(comboBox1.SelectedItem.ToString());
                comboBox1.Items[comboBox1.SelectedIndex] = title;

                // 保存信息
                savePointInfo();
            }
        }

        /**
         * 计划切换事件
         */
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedValue = (string)comboBox1.SelectedItem;

            if (!string.IsNullOrWhiteSpace(selectedValue))
            {
                list_title_text.Text = selectedValue;
                switchDataView(getPointIntervalList(pointListDic, selectedValue));
            }
        }

        /**
         * 表格值变化
         */
        private void dataGridView1_CellValueChanged(object sender, EventArgs e)
        {
            reloadCurrTitlePointList();
        }

        /**********************************************************  非事件私有方法 ***************************************************************/

        /**
         * 绑定热键
         * 
         * <pre>
         * 不知道为啥，窗口最大化最小化后，热键会失效，所以会重复绑定
         * </pre>
         */
        private void bindHotKey()
        {
            unregisterAllHotKey();
            registerAllHotKey();
        }

        private void registerAllHotKey()
        {
            WinApi.RegisterHotKey(this.Handle, WinApi.HOTKEY_ID_F9, 0, Keys.F9);
            WinApi.RegisterHotKey(this.Handle, WinApi.HOTKEY_ID_F10, 0, Keys.F10);
            WinApi.RegisterHotKey(this.Handle, WinApi.HOTKEY_ID_F11, 0, Keys.F11);
            WinApi.RegisterHotKey(this.Handle, WinApi.HOTKEY_ID_F8, 0, Keys.F8);
        }

        private void unregisterAllHotKey()
        {
            WinApi.UnregisterHotKey(this.Handle, WinApi.HOTKEY_ID_F9);
            WinApi.UnregisterHotKey(this.Handle, WinApi.HOTKEY_ID_F10);
            WinApi.UnregisterHotKey(this.Handle, WinApi.HOTKEY_ID_F11);
            WinApi.UnregisterHotKey(this.Handle, WinApi.HOTKEY_ID_F8);
        }

        /**
         * 通过key获取点击点列表
         */
        private List<Click.PointInfo> getPointIntervalList(Dictionary<string, List<Click.PointInfo>> pointListDic, string key)
        {
            List<Click.PointInfo> pointIntervalList = pointListDic.ContainsKey(key) ? pointListDic[key] : new List<Click.PointInfo>();
            if (pointIntervalList.Count == 0)
            {
                addToDic(key, pointIntervalList);
            }
            return pointIntervalList;
        }

        /**
         * 添加点击点渲染至控件
         */ 
        private void addPointToDataView(Click.PointInfo pointInfo)
        {
            click.SetWindowText(windowText.SelectedItem.ToString().Trim());
            string selectText = (string) comboBox1.SelectedItem;

            if (string.IsNullOrWhiteSpace(selectText))
            {
                return;
            }

            List<Click.PointInfo> pointIntervalList = getPointIntervalList(pointListDic, selectText);
            pointInfo.clickInterval = 22;
            pointIntervalList.Add(pointInfo);
            switchDataView(pointIntervalList);
        }

        /**
         * 切换数据栏数据
         */
        private void switchDataView(List<Click.PointInfo> pointList)
        {
            dataGridView1.Rows.Clear();

            int index;
            foreach (Click.PointInfo pointInfo in pointList)
            {
                index = dataGridView1.Rows.Add();
                dataGridView1.Rows[index].Cells[0].Value = pointInfo.point.X;
                dataGridView1.Rows[index].Cells[1].Value = pointInfo.point.Y;
                dataGridView1.Rows[index].Cells[2].Value = pointInfo.clickInterval;
                dataGridView1.Rows[index].Cells[3].Value = pointInfo.hexColorValue;
                dataGridView1.Rows[index].Cells[4].Value = pointInfo.windowText;
            }
        }

        /**
         * 将列表加入dic，若key存在值，则修改，否则添加
         */
        private void addToDic(string key, List<Click.PointInfo> pointList)
        {
            if (pointListDic.ContainsKey(key))
            {
                pointListDic[key] = pointList;
            }
            else
            {
                pointListDic.Add(key, pointList);
            }
        }

        /**
         * 重新加载当前下拉选择对应的点击点列表
         */
        private void reloadCurrTitlePointList()
        {
            string title = (string)comboBox1.SelectedItem;

            if (string.IsNullOrWhiteSpace(title))
            {
                return;
            }

            object XObj;
            object YObj;
            object intervalObj;
            object hexColorValueObj;
            object windowText;
            uint X;
            uint Y;
            uint clickInterval;
            Point point;
            Click.PointInfo pointInterval;
            List<Click.PointInfo> pointList = new List<Click.PointInfo>();
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                XObj = dataGridView1.Rows[i].Cells[0].Value;
                YObj = dataGridView1.Rows[i].Cells[1].Value;
                intervalObj = dataGridView1.Rows[i].Cells[2].Value;
                hexColorValueObj = dataGridView1.Rows[i].Cells[3].Value;
                windowText = dataGridView1.Rows[i].Cells[4].Value;
                if (windowText == null || windowText.ToString().Trim() == "")
                {
                    windowText = "Clicker Heroes";
                } 
                if (XObj != null && YObj != null && intervalObj != null
                    && uint.TryParse(XObj.ToString(), out X)
                    && uint.TryParse(YObj.ToString(), out Y)
                    && uint.TryParse(intervalObj.ToString(), out clickInterval))
                {
                    point = new Point((int)X, (int)Y);
                    if (hexColorValueObj != null && hexColorValueObj.ToString().Length == 6)
                    {
                        pointInterval = new Click.PointInfo(point, (int)clickInterval, hexColorValueObj.ToString(), windowText.ToString());
                    }
                    else
                    {
                        pointInterval = new Click.PointInfo(point, (int)clickInterval, windowText.ToString(), 1);
                    }
                    pointList.Add(pointInterval);
                }
            }

            // 将最新结果添加至字典
            addToDic(title, pointList);

            // 保存信息
            savePointInfo();
        }

        /**
         * 根据时间间隔对点击点列表进行分组
         */
        private Dictionary<int, List<Click.PointInfo>> getPointListGroupByInterval()
        {
            string title = (string)comboBox1.SelectedItem;

            if (string.IsNullOrWhiteSpace(title))
            {
                return null;
            }

            Dictionary<int, List<Click.PointInfo>> intervalDic = new Dictionary<int, List<Click.PointInfo>>();

            foreach (Click.PointInfo pointInfo in pointListDic[title])
            {
                if (!intervalDic.ContainsKey(pointInfo.clickInterval))
                {
                    intervalDic.Add(pointInfo.clickInterval, new List<Click.PointInfo>());
                }
                intervalDic[pointInfo.clickInterval].Add(pointInfo);
            }

            return intervalDic;
        }

        /**
         * 加载点击点信息
         */
        private void loadPointInfo()
        {
            if (!File.Exists(path))
            {
                return;
            }
            string text = File.ReadAllText(path, Encoding.UTF8);
            if (string.IsNullOrWhiteSpace(text))
            {
                return;
            }

            Dictionary<string, List<Click.PointInfo>> tmpDic = JsonConvert.DeserializeObject<Dictionary<string, List<Click.PointInfo>>>(text);
            int index = 0;
            foreach (string key in tmpDic.Keys)
            {
                comboBox1.Items.Add(key);

                if (index == 0)
                {
                    comboBox1.SelectedIndex = 0;
                    switchDataView(getPointIntervalList(tmpDic, comboBox1.SelectedItem.ToString()));
                }

                index++;
            }

            pointListDic = tmpDic;
        }

        /**
         * 保存点击点信息
         */
        private void savePointInfo()
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }

            string text = JsonConvert.SerializeObject(pointListDic);
            File.WriteAllText(path, text, Encoding.UTF8);
        }

        private void Form1_FormClosing_1(object sender, FormClosingEventArgs e)
        {
            this.notifyIcon1.Dispose();
            System.Environment.Exit(0);//这是最彻底的退出方式，不管什么线程都被强制退出，把程序结束的很干净。
        }

        private void windowText_SelectedIndexChanged(object sender, EventArgs e)
        {
            click.SetWindowText(windowText.SelectedItem.ToString().Trim());
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (!backgroundWorker1.IsBusy)
                backgroundWorker1.RunWorkerAsync();
        }

        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            click.checkClikHero();
        }
        private Dictionary<String, bool> WindowControlDic = new Dictionary<string, bool>();
        private void showControlButton_Click(object sender, EventArgs e)
        {
            IntPtr hwd =click.getHandle();
            WinApi.Rect rect = new WinApi.Rect();
            WinApi.ShowWindow(hwd, WinApi.CmdShow_Show);
            WinApi.GetWindowRect(hwd, out rect);
            if (WindowControlDic.ContainsKey(click.CLICKER_HEROES))
            {
                if (WindowControlDic[click.CLICKER_HEROES])
                    WinApi.SetWindowPos(hwd, WinApi.HWND_NOTOPMOST, -rect.Right, -rect.Bottom, rect.Right - rect.Left, rect.Bottom - rect.Top, 1);
                else
                    WinApi.SetWindowPos(hwd, WinApi.HWND_NOTOPMOST, 50, 50, rect.Right - rect.Left, rect.Bottom - rect.Top, 1);
                WindowControlDic[click.CLICKER_HEROES] = !WindowControlDic[click.CLICKER_HEROES];
            }
            else
            {
                if (rect.Top >= 0)
                {
                    WinApi.SetWindowPos(hwd, WinApi.HWND_NOTOPMOST, -rect.Right, -rect.Bottom, rect.Right - rect.Left, rect.Bottom - rect.Top, 1);
                    WindowControlDic.Add(click.CLICKER_HEROES, false);
                }
                else
                {
                    WinApi.SetWindowPos(hwd, WinApi.HWND_NOTOPMOST, 50, 50, rect.Right - rect.Left, rect.Bottom - rect.Top, 1);
                    WindowControlDic.Add(click.CLICKER_HEROES, true);
                }
            }
        }

        private void textClickTimes_TextChanged(object sender, EventArgs e)
        {
            try
            {
                click.ClickTimes = Convert.ToInt32(textClickTimes.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
