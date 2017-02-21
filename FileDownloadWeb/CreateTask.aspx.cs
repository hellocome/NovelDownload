using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

using FileDownload;

namespace FileDownloadWeb
{
    public partial class _Default : System.Web.UI.Page
    {
        FileDownloadForWeb work;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["work"] == null)
            {
                work = new FileDownloadForWeb();
                Session["work"] = work;
            }
            else
            {
                work = (FileDownloadForWeb)Session["work"];
            }

        }

        override protected void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
        }

        public string SecondsToHour(TimeSpan seconds)
        {
            int sec = (int)(seconds.TotalSeconds);

            return SecondsToHour(sec);
        }

        public string SecondsToHour(int sec)
        {
            bool present = false;

            int hours = sec / 3600;

            sec = sec - hours * 3600;

            int minute = sec / 60;

            sec = sec - minute * 60;

            string str = string.Empty;

            if (hours > 0)
            {
                str += string.Format("{0} Hours ", hours);
                present = true;
            }

            if (minute > 0)
            {
                str += string.Format("{0} Minutes ", minute);
                present = true;
            }

            if (!present || sec > 0)
            {
                str += string.Format("{0} Seconds ", sec);
            }

            return str;
        }

        protected void timer_Tick(object sender, EventArgs e)
        {
            try
            {
                switch (work.State.Progress)
                {
                    case ProgressEnum.NotStart:
                        {
                            this.LabelInfo.Text = "No task in the queue ";

                            break;
                        }
                    case ProgressEnum.Progressing:
                        {
                            this.LabelInfo.Text = "Task is running: " + 
                                SecondsToHour((TimeSpan)(DateTime.Now - work.StartTime))
                                + "   -   Time Remaining: " + SecondsToHour(work.State.RemainingTime);
                            this.btnStart.Enabled = false;

                            WriteLog(work.State);
                            break;
                        }
                    case ProgressEnum.FinishOK:
                        {
                            this.LabelInfo.Text = "Task finished SUCCESSFUL. Total Time: " +
                                SecondsToHour((TimeSpan)(DateTime.Now - work.StartTime)); SecondsToHour((TimeSpan)(DateTime.Now - work.StartTime));
                                    
                            this.btnStart.Enabled = true;

                            controlTimer(false);

                            WriteLog(work.State);
                            break;
                        }
                    case ProgressEnum.FinishFailed:
                        {
                            this.LabelInfo.Text = "Task finished FAILED. Total Time: " + SecondsToHour((TimeSpan)(DateTime.Now - work.StartTime));
                            this.btnStart.Enabled = true;

                            controlTimer(false);

                            WriteLog(work.State);
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                this.LabelInfo.Text = ex.Message;
            }
        }

        void controlTimer(object enabled)
        {
            int sleep = timer.Interval;
            System.Threading.Thread.Sleep(sleep);

            this.timer.Enabled = (Boolean)enabled;
        }

        void controlTimerStart()
        {
            this.timer.Enabled = true;
        }

        protected void btnStart_Click(object sender, EventArgs e)
        {
            this.LabelInfo.Text = "Start Pressed";

            if (work != null && work.State.Progress != ProgressEnum.Progressing)
            {
                this.LabelInfo.Text = "Starting...";

                this.btnStart.Enabled = false;

                controlTimerStart();
                work.Start(this.lbHost.Text, this.tbNovelUrl.Text, this.tbNovelName.Text, this.lbMaxPages.Text, "NO");

                HyperLinkLog.NavigateUrl = string.Format("ShowLog.aspx?File={0}", work.LogSaveToFileUrl);
            }
        }

        protected void btnCreateWebIndex_Click(object sender, EventArgs e)
        {
            try
            {
                new BuildWebSite.BuildWebSiteIndex().ProcessWeb();
            }
            catch (Exception ex)
            {
                this.LabelInfo.Text = ex.ToString();
            }
        }

        protected void btnStop_Click(object sender, EventArgs e)
        {
            if (work != null && work.State.Progress == ProgressEnum.Progressing)
            {
                work.Stop();
            }
        }

        private void WriteLog(ProgressArgs arg)
        {
            LabelProgress.Text  = string.Format("Progress {0}/{1} Failed: {2}", arg.Current, arg.Total, arg.Failed);
        }
    }
}