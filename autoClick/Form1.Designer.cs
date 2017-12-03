using System.Windows.Forms;

namespace autoClick
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.button1 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.interval = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.hexColorValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.wText = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.list_title_text = new System.Windows.Forms.TextBox();
            this.add_btn = new System.Windows.Forms.Button();
            this.del_btn = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.update_btn = new System.Windows.Forms.Button();
            this.windowText = new System.Windows.Forms.ComboBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.testCheck = new System.Windows.Forms.CheckBox();
            this.testPointText = new System.Windows.Forms.TextBox();
            this.showControlButton = new System.Windows.Forms.Button();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(119, 49);
            this.button1.Margin = new System.Windows.Forms.Padding(4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 29);
            this.button1.TabIndex = 0;
            this.button1.Text = "开始";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(120, 15);
            this.textBox1.Margin = new System.Windows.Forms.Padding(4);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(97, 25);
            this.textBox1.TabIndex = 1;
            this.textBox1.Text = "100";
            this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(-1, 19);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(112, 15);
            this.label1.TabIndex = 2;
            this.label1.Text = "扫描点间隔时间";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(23, 55);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 15);
            this.label2.TabIndex = 3;
            this.label2.Text = "按F9开始";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(228, 19);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(23, 15);
            this.label3.TabIndex = 4;
            this.label3.Text = "ms";
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "鼠标连点器";
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(111, 30);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(110, 26);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllHeaders;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.interval,
            this.hexColorValue,
            this.wText});
            this.dataGridView1.Location = new System.Drawing.Point(3, 118);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(4);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(569, 288);
            this.dataGridView1.TabIndex = 6;
            this.dataGridView1.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellValueChanged);
            this.dataGridView1.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.dataGridView1_CellValueChanged);
            // 
            // Column1
            // 
            this.Column1.HeaderText = "X";
            this.Column1.Name = "Column1";
            // 
            // Column2
            // 
            this.Column2.HeaderText = "Y";
            this.Column2.Name = "Column2";
            // 
            // interval
            // 
            this.interval.HeaderText = "时间间隔(ms)";
            this.interval.Name = "interval";
            // 
            // hexColorValue
            // 
            this.hexColorValue.HeaderText = "16进制色值";
            this.hexColorValue.Name = "hexColorValue";
            // 
            // wText
            // 
            this.wText.HeaderText = "窗口名称";
            this.wText.Name = "wText";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(224, 55);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(91, 15);
            this.label4.TabIndex = 3;
            this.label4.Text = "按F10记录点";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.Red;
            this.label5.Location = new System.Drawing.Point(259, 19);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(52, 15);
            this.label5.TabIndex = 7;
            this.label5.Text = "未开始";
            // 
            // comboBox1
            // 
            this.comboBox1.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(25, 86);
            this.comboBox1.Margin = new System.Windows.Forms.Padding(4);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(84, 23);
            this.comboBox1.TabIndex = 8;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // list_title_text
            // 
            this.list_title_text.Location = new System.Drawing.Point(121, 85);
            this.list_title_text.Margin = new System.Windows.Forms.Padding(4);
            this.list_title_text.Name = "list_title_text";
            this.list_title_text.Size = new System.Drawing.Size(96, 25);
            this.list_title_text.TabIndex = 9;
            // 
            // add_btn
            // 
            this.add_btn.Location = new System.Drawing.Point(227, 84);
            this.add_btn.Margin = new System.Windows.Forms.Padding(4);
            this.add_btn.Name = "add_btn";
            this.add_btn.Size = new System.Drawing.Size(40, 29);
            this.add_btn.TabIndex = 10;
            this.add_btn.Text = "增";
            this.add_btn.UseVisualStyleBackColor = true;
            this.add_btn.Click += new System.EventHandler(this.add_btn_Click);
            // 
            // del_btn
            // 
            this.del_btn.Location = new System.Drawing.Point(313, 84);
            this.del_btn.Margin = new System.Windows.Forms.Padding(4);
            this.del_btn.Name = "del_btn";
            this.del_btn.Size = new System.Drawing.Size(40, 29);
            this.del_btn.TabIndex = 10;
            this.del_btn.Text = "删";
            this.del_btn.UseVisualStyleBackColor = true;
            this.del_btn.Click += new System.EventHandler(this.del_btn_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(321, 55);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(136, 15);
            this.label6.TabIndex = 11;
            this.label6.Text = "按F11记录点、色值";
            // 
            // update_btn
            // 
            this.update_btn.Location = new System.Drawing.Point(268, 84);
            this.update_btn.Margin = new System.Windows.Forms.Padding(4);
            this.update_btn.Name = "update_btn";
            this.update_btn.Size = new System.Drawing.Size(40, 29);
            this.update_btn.TabIndex = 12;
            this.update_btn.Text = "改";
            this.update_btn.UseVisualStyleBackColor = true;
            this.update_btn.Click += new System.EventHandler(this.update_btn_Click);
            // 
            // windowText
            // 
            this.windowText.FormattingEnabled = true;
            this.windowText.Items.AddRange(new object[] {
            "Clicker Heroes",
            "Realm Grinder"});
            this.windowText.Location = new System.Drawing.Point(313, 15);
            this.windowText.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.windowText.Name = "windowText";
            this.windowText.Size = new System.Drawing.Size(144, 23);
            this.windowText.TabIndex = 13;
            this.windowText.Text = "Clicker Heroes";
            this.windowText.SelectedIndexChanged += new System.EventHandler(this.windowText_SelectedIndexChanged);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            // 
            // testCheck
            // 
            this.testCheck.AutoSize = true;
            this.testCheck.Location = new System.Drawing.Point(493, 88);
            this.testCheck.Name = "testCheck";
            this.testCheck.Size = new System.Drawing.Size(59, 19);
            this.testCheck.TabIndex = 14;
            this.testCheck.Text = "测试";
            this.testCheck.UseVisualStyleBackColor = true;
            // 
            // testPointText
            // 
            this.testPointText.Location = new System.Drawing.Point(387, 84);
            this.testPointText.Name = "testPointText";
            this.testPointText.Size = new System.Drawing.Size(100, 25);
            this.testPointText.TabIndex = 15;
            this.testPointText.Text = "1113,200";
            // 
            // showControlButton
            // 
            this.showControlButton.Location = new System.Drawing.Point(477, 15);
            this.showControlButton.Name = "showControlButton";
            this.showControlButton.Size = new System.Drawing.Size(60, 23);
            this.showControlButton.TabIndex = 16;
            this.showControlButton.Text = "隐/显";
            this.showControlButton.UseVisualStyleBackColor = true;
            this.showControlButton.Click += new System.EventHandler(this.showControlButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(575, 404);
            this.Controls.Add(this.showControlButton);
            this.Controls.Add(this.testPointText);
            this.Controls.Add(this.testCheck);
            this.Controls.Add(this.windowText);
            this.Controls.Add(this.update_btn);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.del_btn);
            this.Controls.Add(this.add_btn);
            this.Controls.Add(this.list_title_text);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form1";
            this.Text = "鼠标连点器";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing_1);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.SizeChanged += new System.EventHandler(this.Form1_SizeChanged);
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label label4;
        public System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.TextBox list_title_text;
        private System.Windows.Forms.Button add_btn;
        private System.Windows.Forms.Button del_btn;
        private DataGridViewTextBoxColumn Column1;
        private DataGridViewTextBoxColumn Column2;
        private DataGridViewTextBoxColumn interval;
        private DataGridViewTextBoxColumn hexColorValue;
        private Label label6;
        private Button update_btn;
        private ComboBox windowText;
        private DataGridViewTextBoxColumn wText;
        private Timer timer1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private CheckBox testCheck;
        private TextBox testPointText;
        private Button showControlButton;
    }
}

