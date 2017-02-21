namespace FileDownloadWinForm
{
    partial class FileDownloadForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FileDownloadForm));
            this.gbConfig = new System.Windows.Forms.GroupBox();
            this.CBShutDown = new System.Windows.Forms.CheckBox();
            this.btnOpenSave = new System.Windows.Forms.Button();
            this.btnOpenApp = new System.Windows.Forms.Button();
            this.tbNovelName = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.btnClearLogs = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.TBSaveTo = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.CBUpdateMode = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.CBMaxPages = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.LinkLabelToNovel = new System.Windows.Forms.LinkLabel();
            this.tbUrl = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.CBHost = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.gbLog = new System.Windows.Forms.GroupBox();
            this.RTBLogs = new System.Windows.Forms.RichTextBox();
            this.statusStripProgress = new System.Windows.Forms.StatusStrip();
            this.toolLabelResize = new System.Windows.Forms.ToolStripStatusLabel();
            this.progressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.ToolStripProgress = new System.Windows.Forms.ToolStripStatusLabel();
            this.gbConfig.SuspendLayout();
            this.gbLog.SuspendLayout();
            this.statusStripProgress.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbConfig
            // 
            this.gbConfig.Controls.Add(this.CBShutDown);
            this.gbConfig.Controls.Add(this.btnOpenSave);
            this.gbConfig.Controls.Add(this.btnOpenApp);
            this.gbConfig.Controls.Add(this.tbNovelName);
            this.gbConfig.Controls.Add(this.label6);
            this.gbConfig.Controls.Add(this.btnClearLogs);
            this.gbConfig.Controls.Add(this.btnStop);
            this.gbConfig.Controls.Add(this.btnStart);
            this.gbConfig.Controls.Add(this.TBSaveTo);
            this.gbConfig.Controls.Add(this.label5);
            this.gbConfig.Controls.Add(this.CBUpdateMode);
            this.gbConfig.Controls.Add(this.label4);
            this.gbConfig.Controls.Add(this.CBMaxPages);
            this.gbConfig.Controls.Add(this.label3);
            this.gbConfig.Controls.Add(this.LinkLabelToNovel);
            this.gbConfig.Controls.Add(this.tbUrl);
            this.gbConfig.Controls.Add(this.label2);
            this.gbConfig.Controls.Add(this.CBHost);
            this.gbConfig.Controls.Add(this.label1);
            this.gbConfig.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbConfig.Location = new System.Drawing.Point(0, 0);
            this.gbConfig.Name = "gbConfig";
            this.gbConfig.Size = new System.Drawing.Size(1037, 207);
            this.gbConfig.TabIndex = 0;
            this.gbConfig.TabStop = false;
            this.gbConfig.Text = "Config";
            // 
            // CBShutDown
            // 
            this.CBShutDown.AutoSize = true;
            this.CBShutDown.Location = new System.Drawing.Point(902, 171);
            this.CBShutDown.Name = "CBShutDown";
            this.CBShutDown.Size = new System.Drawing.Size(129, 17);
            this.CBShutDown.TabIndex = 18;
            this.CBShutDown.Text = "Shutdown After Finish";
            this.CBShutDown.UseVisualStyleBackColor = true;
            // 
            // btnOpenSave
            // 
            this.btnOpenSave.Location = new System.Drawing.Point(942, 19);
            this.btnOpenSave.Name = "btnOpenSave";
            this.btnOpenSave.Size = new System.Drawing.Size(89, 30);
            this.btnOpenSave.TabIndex = 17;
            this.btnOpenSave.Text = "Open Save Dir";
            this.btnOpenSave.UseVisualStyleBackColor = true;
            this.btnOpenSave.Click += new System.EventHandler(this.btnOpenSave_Click);
            // 
            // btnOpenApp
            // 
            this.btnOpenApp.Location = new System.Drawing.Point(942, 73);
            this.btnOpenApp.Name = "btnOpenApp";
            this.btnOpenApp.Size = new System.Drawing.Size(89, 30);
            this.btnOpenApp.TabIndex = 16;
            this.btnOpenApp.Text = "Open App Dir";
            this.btnOpenApp.UseVisualStyleBackColor = true;
            this.btnOpenApp.Click += new System.EventHandler(this.btnOpenApp_Click);
            // 
            // tbNovelName
            // 
            this.tbNovelName.Location = new System.Drawing.Point(85, 73);
            this.tbNovelName.Name = "tbNovelName";
            this.tbNovelName.Size = new System.Drawing.Size(832, 20);
            this.tbNovelName.TabIndex = 15;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(16, 73);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(69, 13);
            this.label6.TabIndex = 14;
            this.label6.Text = "Novel Name:";
            // 
            // btnClearLogs
            // 
            this.btnClearLogs.Location = new System.Drawing.Point(942, 126);
            this.btnClearLogs.Name = "btnClearLogs";
            this.btnClearLogs.Size = new System.Drawing.Size(89, 30);
            this.btnClearLogs.TabIndex = 13;
            this.btnClearLogs.Text = "Clear Log";
            this.btnClearLogs.UseVisualStyleBackColor = true;
            this.btnClearLogs.Click += new System.EventHandler(this.btnClearLogs_Click);
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(526, 163);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(106, 30);
            this.btnStop.TabIndex = 12;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(383, 163);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(106, 30);
            this.btnStart.TabIndex = 11;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // TBSaveTo
            // 
            this.TBSaveTo.Location = new System.Drawing.Point(85, 136);
            this.TBSaveTo.Name = "TBSaveTo";
            this.TBSaveTo.Size = new System.Drawing.Size(832, 20);
            this.TBSaveTo.TabIndex = 10;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(34, 139);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(51, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Save To:";
            // 
            // CBUpdateMode
            // 
            this.CBUpdateMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CBUpdateMode.FormattingEnabled = true;
            this.CBUpdateMode.Items.AddRange(new object[] {
            "TRUE",
            "FALSE"});
            this.CBUpdateMode.Location = new System.Drawing.Point(526, 100);
            this.CBUpdateMode.Name = "CBUpdateMode";
            this.CBUpdateMode.Size = new System.Drawing.Size(314, 21);
            this.CBUpdateMode.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(445, 103);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(75, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Update Mode:";
            // 
            // CBMaxPages
            // 
            this.CBMaxPages.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CBMaxPages.FormattingEnabled = true;
            this.CBMaxPages.Items.AddRange(new object[] {
            "10",
            "20",
            "50",
            "100",
            "200",
            "500",
            "1000",
            "2000",
            "5000",
            "10000",
            "50000"});
            this.CBMaxPages.Location = new System.Drawing.Point(85, 99);
            this.CBMaxPages.Name = "CBMaxPages";
            this.CBMaxPages.Size = new System.Drawing.Size(306, 21);
            this.CBMaxPages.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(22, 103);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Max Pages:";
            // 
            // LinkLabelToNovel
            // 
            this.LinkLabelToNovel.Location = new System.Drawing.Point(82, 45);
            this.LinkLabelToNovel.Name = "LinkLabelToNovel";
            this.LinkLabelToNovel.Size = new System.Drawing.Size(835, 20);
            this.LinkLabelToNovel.TabIndex = 4;
            this.LinkLabelToNovel.TabStop = true;
            this.LinkLabelToNovel.Text = "No Link";
            // 
            // tbUrl
            // 
            this.tbUrl.Location = new System.Drawing.Point(520, 21);
            this.tbUrl.Name = "tbUrl";
            this.tbUrl.Size = new System.Drawing.Size(397, 20);
            this.tbUrl.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(463, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Novel Url:";
            // 
            // CBHost
            // 
            this.CBHost.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CBHost.FormattingEnabled = true;
            this.CBHost.Items.AddRange(new object[] {
            "Biquge.com",
            "Biquge.co",
            "Biquge.cc",
            "Dayanwenxue.com",
            "Xiaoshuom.com",
            "Shukeju.com",
            "Wenxuelou.com",
            "1kanshu.com",
            "aoye.cc"});
            this.CBHost.Location = new System.Drawing.Point(85, 19);
            this.CBHost.Name = "CBHost";
            this.CBHost.Size = new System.Drawing.Size(349, 21);
            this.CBHost.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Novel Host:";
            // 
            // gbLog
            // 
            this.gbLog.Controls.Add(this.RTBLogs);
            this.gbLog.Controls.Add(this.statusStripProgress);
            this.gbLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbLog.Location = new System.Drawing.Point(0, 207);
            this.gbLog.Name = "gbLog";
            this.gbLog.Size = new System.Drawing.Size(1037, 555);
            this.gbLog.TabIndex = 2;
            this.gbLog.TabStop = false;
            // 
            // RTBLogs
            // 
            this.RTBLogs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RTBLogs.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RTBLogs.Location = new System.Drawing.Point(3, 16);
            this.RTBLogs.Name = "RTBLogs";
            this.RTBLogs.ReadOnly = true;
            this.RTBLogs.Size = new System.Drawing.Size(1031, 514);
            this.RTBLogs.TabIndex = 2;
            this.RTBLogs.Text = "";
            this.RTBLogs.WordWrap = false;
            // 
            // statusStripProgress
            // 
            this.statusStripProgress.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolLabelResize,
            this.progressBar,
            this.ToolStripProgress});
            this.statusStripProgress.Location = new System.Drawing.Point(3, 530);
            this.statusStripProgress.Name = "statusStripProgress";
            this.statusStripProgress.Size = new System.Drawing.Size(1031, 22);
            this.statusStripProgress.TabIndex = 1;
            this.statusStripProgress.Text = "statusStrip1";
            // 
            // toolLabelResize
            // 
            this.toolLabelResize.AutoSize = false;
            this.toolLabelResize.Name = "toolLabelResize";
            this.toolLabelResize.Size = new System.Drawing.Size(0, 17);
            // 
            // progressBar
            // 
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(400, 16);
            // 
            // ToolStripProgress
            // 
            this.ToolStripProgress.Name = "ToolStripProgress";
            this.ToolStripProgress.Size = new System.Drawing.Size(0, 17);
            // 
            // FileDownloadForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1037, 762);
            this.Controls.Add(this.gbLog);
            this.Controls.Add(this.gbConfig);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FileDownloadForm";
            this.Text = "Novel Download";
            this.gbConfig.ResumeLayout(false);
            this.gbConfig.PerformLayout();
            this.gbLog.ResumeLayout(false);
            this.gbLog.PerformLayout();
            this.statusStripProgress.ResumeLayout(false);
            this.statusStripProgress.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbConfig;
        private System.Windows.Forms.GroupBox gbLog;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox CBHost;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbUrl;
        private System.Windows.Forms.LinkLabel LinkLabelToNovel;
        private System.Windows.Forms.ComboBox CBMaxPages;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox CBUpdateMode;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox TBSaveTo;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnClearLogs;
        private System.Windows.Forms.TextBox tbNovelName;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.StatusStrip statusStripProgress;
        private System.Windows.Forms.ToolStripProgressBar progressBar;
        private System.Windows.Forms.ToolStripStatusLabel toolLabelResize;
        private System.Windows.Forms.Button btnOpenApp;
        private System.Windows.Forms.Button btnOpenSave;
        private System.Windows.Forms.RichTextBox RTBLogs;
        private System.Windows.Forms.ToolStripStatusLabel ToolStripProgress;
        private System.Windows.Forms.CheckBox CBShutDown;
    }
}

