using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using FileDownload;
using FileDownloadLib;

namespace FileDownloadWinForm
{
    public partial class FileDownloadForm : Form
    {
        UpdateLogsDelegate UpdateLogs;
        WriteLogDelegate writeLog;
        UpdateControlsDelegate UpdateControls;

        FileDownloadForWinForm downloader = new FileDownloadForWinForm();

        public FileDownloadForm()
        {
            InitializeComponent();

            writeLog = new WriteLogDelegate(UpdateLogsInRTB);
            UpdateControls = new UpdateControlsDelegate(OnProgressChanged);
            UpdateLogs = new UpdateLogsDelegate(UpdateLogsInRTB);

            Logger.Instance.Init(writeLog, Color.Red, Color.Green, Color.Black);

            this.CBMaxPages.SelectedIndex = 6;
            this.CBHost.SelectedIndex = 0;
            this.CBUpdateMode.SelectedIndex = 0;

            downloader.OnProgressChanged += new ProgressChangeDelegate(downloader_OnProgressChanged);

            toolLabelResize.Width = (this.statusStripProgress.Width - this.progressBar.Width) / 2;

            this.tbUrl.TextChanged += new EventHandler(tbUrl_TextChanged);
            this.CBHost.TextChanged += new EventHandler(CBHost_TextChanged);

            this.FormClosing += new FormClosingEventHandler(FileDownloadForm_FormClosing);
            this.Shown += new EventHandler(FileDownloadForm_Shown);

            this.FormClosed += new FormClosedEventHandler(FileDownloadForm_FormClosed);

            this.LinkLabelToNovel.Click += new EventHandler(LinkLabelToNovel_Click);
        }

        void LinkLabelToNovel_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(this.LinkLabelToNovel.Text);
            }
            catch (Exception ex)
            {
                Logger.Instance.Error(ex.ToString());
            }
        }

        void FileDownloadForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(0);
        }

        void FileDownloadForm_Shown(object sender, EventArgs e)
        {
            LoadAppData();
        }

        void FileDownloadForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveToAppData();
        }

        private void SaveToAppData()
        {
            try
            {
                Properties.Settings.Default.NovelHost = this.CBHost.Text;
                Properties.Settings.Default.NovelMaxPages = this.CBMaxPages.Text;
                Properties.Settings.Default.NovelName = this.tbNovelName.Text;
                Properties.Settings.Default.NovelSaveTo = this.TBSaveTo.Text;
                Properties.Settings.Default.NovelUpdateMode = this.CBUpdateMode.Text;
                Properties.Settings.Default.NovelUrl = this.tbUrl.Text;


                if (!string.IsNullOrEmpty(this.CBHost.Text) && !string.IsNullOrEmpty(this.tbNovelName.Text))
                {
                    string key = string.Format("{0}_{1}", this.CBHost.Text.ToUpper().Trim(), this.tbNovelName.Text.ToUpper().Trim());

                    NovelConfigDictionaryValue saveValue = new NovelConfigDictionaryValue();

                    saveValue.NovelHost = this.CBHost.Text;
                    saveValue.NovelMaxPages = this.CBMaxPages.Text;
                    saveValue.NovelName = this.tbNovelName.Text;
                    saveValue.NovelSaveTo = this.TBSaveTo.Text;
                    saveValue.NovelUpdateMode = this.CBUpdateMode.Text;
                    saveValue.NovelUrl = this.tbUrl.Text;
                }


                Properties.Settings.Default.Save();
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("Save Config Failed: " + ex.Message);
            }
        }

        private void LoadAppData()
        {
            try
            {
                this.CBHost.Text = Properties.Settings.Default.NovelHost;
                this.CBMaxPages.Text = Properties.Settings.Default.NovelMaxPages;
                this.tbNovelName.Text = Properties.Settings.Default.NovelName;
                this.TBSaveTo.Text = Properties.Settings.Default.NovelSaveTo;
                this.CBUpdateMode.Text = Properties.Settings.Default.NovelUpdateMode;
                this.tbUrl.Text = Properties.Settings.Default.NovelUrl;
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("Load Config Failed: " + ex.Message);
            }
        }

        void CBHost_TextChanged(object sender, EventArgs e)
        {
            ApplyUrl();
        }

        void tbUrl_TextChanged(object sender, EventArgs e)
        {
            string format = FileDownloadUtil.GetUrlFromHost(this.tbUrl.Text);

            if (!format.Equals(this.tbUrl.Text))
            {
                Logger.Instance.Info("Format Url");
                this.tbUrl.Text = format;
            }
            else
            {
                ApplyUrl();
            }
        }

        private void ApplyUrl()
        {
            this.LinkLabelToNovel.Text = DownloadConfig.Protocol + "://" + FileDownloadUtil.CombindCorrectUrl(this.CBHost.Text, tbUrl.Text);
        }

        void downloader_OnProgressChanged(ProgressArgs arg)
        {
            OnProgressChanged(arg);
        }

        void OnProgressChanged(ProgressArgs arg)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(UpdateControls, new object[] { arg });
            }
            else
            {
                if (arg.Progress == ProgressEnum.Progressing)
                {
                    if (arg.Percentage >= this.progressBar.Minimum && arg.Percentage <= this.progressBar.Maximum)
                    {
                        this.progressBar.Value = arg.Percentage;
                    }

                    this.btnStart.Enabled = false;

                    int minutes = arg.RemainingTime / 60;
                    int seconds = arg.RemainingTime - minutes * 60;

                    string timeRemaining = string.Format("[{0} minutes {1} seconds remaining]", minutes, seconds);

                    ToolStripProgress.Text = string.Format("{0}/{1}  {2}", arg.Current, arg.Total, timeRemaining);
                }
                else if (arg.Progress == ProgressEnum.NotStart)
                {
                    this.progressBar.Value = 0;

                    this.btnStart.Enabled = true;

                    ToolStripProgress.Text = string.Format("0/0");
                }
                else
                {
                    this.progressBar.Value = 100;

                    this.btnStart.Enabled = true;

                    ToolStripProgress.Text = string.Format("{0}/{1}", arg.Current, arg.Total);
                }
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (downloader.IsRunning)
            {
                downloader.Stop();
            }

            if (string.IsNullOrEmpty(this.TBSaveTo.Text))
            {
                this.TBSaveTo.Text = string.Format("{0}\\DOWNLOAD", AppDomain.CurrentDomain.BaseDirectory);
            }

            downloader.Start(this.CBHost.Text, this.tbUrl.Text, this.tbNovelName.Text, this.CBMaxPages.Text, this.CBUpdateMode.Text, this.TBSaveTo.Text);
        }

        public void UpdateLogsInRTB(Object color, string log)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(UpdateLogs, new object[] { color, log });
            }
            else
            {
                try
                {
                    Color newColor = (Color)color;

                    if (newColor != Color.Empty)
                    {
                        int currentIndex = this.RTBLogs.Text.Length;

                        this.RTBLogs.AppendText(log);
                        this.RTBLogs.AppendText("\r\n");
                        this.RTBLogs.SelectionStart = currentIndex;
                        this.RTBLogs.SelectionLength = log.Length;
                        this.RTBLogs.SelectionColor = newColor;
                    }
                    else
                    {
                        this.RTBLogs.AppendText(log);
                        this.RTBLogs.AppendText("\r\n");
                    }
                }
                catch (Exception)
                {
                    this.RTBLogs.AppendText(log);
                    this.RTBLogs.AppendText("\r\n");
                }
            }
        }

        private void btnClearLogs_Click(object sender, EventArgs e)
        {
            this.RTBLogs.Text = string.Empty;
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            if (downloader.IsRunning)
            {
                downloader.Stop();
            }

            //this.btnStart.Enabled = true;
        }

        private void btnOpenSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (System.IO.Directory.Exists(this.TBSaveTo.Text))
                {
                    System.Diagnostics.Process.Start(this.TBSaveTo.Text);
                }
                else
                {
                    Logger.Instance.Error(string.Format("Directory [{0}] doesn't exist", this.TBSaveTo.Text));
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Error(ex.Message);
            }
        }

        private void btnOpenApp_Click(object sender, EventArgs e)
        {
            try
            {
                if (System.IO.Directory.Exists(AppDomain.CurrentDomain.BaseDirectory))
                {
                    System.Diagnostics.Process.Start(AppDomain.CurrentDomain.BaseDirectory);
                }
                else
                {
                    Logger.Instance.Error(string.Format("Directory [{0}] doesn't exist", AppDomain.CurrentDomain.BaseDirectory));
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Error(ex.Message);
            }
        }

        private const int ShutdownCount = 600;

        private void Shutdown()
        {

            if (this.CBShutDown.Checked)
            {
                Logger.Instance.InfoImportant("Shut down computer in {0} mins");
            }
        }
    }
}
