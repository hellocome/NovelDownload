using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;
using FileDownloadLib;

namespace FileDownload
{
    public class FileDownloadForWinForm
    {
        bool UpdateMode = true;
        string Host = string.Empty;
        string Url = string.Empty;
        string SaveTo = string.Empty;
        string NovelName = string.Empty;
        int MaxPages = 1000;
        IDownloadImagesText download = null;
        Thread thread = null;


        public ProgressArgs State = new ProgressArgs();

        public bool IsRunning = false;

        public DateTime StartTime;
        public DateTime EndTime;

        public event ProgressChangeDelegate OnProgressChanged;
        public void FireOnProgressChangedEvent(ProgressArgs arg)
        {
            try
            {
                State = arg;

                if (OnProgressChanged != null)
                {
                    OnProgressChanged(arg);
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Error(ex.ToString());
            }
        }

        public void Stop()
        {
            if (download != null)
            {
                download.Stop();
            }
        }

        private string BuildArguments(string host, string url, string novelName, string maxPage, string updateMode, string saveTo)
        {
            string arguments = string.Empty;

            if (!string.IsNullOrEmpty(host))
            {
                arguments += " -h " + host;
            }

            if (!string.IsNullOrEmpty(url))
            {
                arguments += " -u " + url;
            }

            if (!string.IsNullOrEmpty(novelName))
            {
                arguments += " -n " + novelName;
            }

            if (!string.IsNullOrEmpty(maxPage))
            {
                arguments += " -mp " + maxPage;
            }

            if (updateMode.Equals("FALSE", StringComparison.OrdinalIgnoreCase))
            {
                arguments += " -new ";
            }

            if (!string.IsNullOrEmpty(saveTo))
            {
                //arguments += " -s \"" + saveTo + "\"";
            }


            return arguments;
        }

        public void Start(string host, string url, string novelName, string maxPage, string updateMode, string saveTo)
        {
            try
            {
                IsRunning = true;
                StartTime = DateTime.Now;

                Url = url;
                Host = host;

                ProgressArgs arg = new ProgressArgs();
                arg.Progress = ProgressEnum.Progressing;
                FireOnProgressChangedEvent(arg);

                if (string.IsNullOrEmpty(novelName))
                {
                    Logger.Instance.Error("Novel Name can't be empty");

                    EndTime = DateTime.Now;
                    arg.Progress = ProgressEnum.FinishFailed;
                    FireOnProgressChangedEvent(arg);

                    return;
                }

                SaveTo = saveTo;
                NovelName = novelName;

                if (string.IsNullOrEmpty(SaveTo))
                {
                    SaveTo = string.Format("{0}\\DOWNLOAD", AppDomain.CurrentDomain.BaseDirectory);
                }

                if (!int.TryParse(maxPage, out MaxPages))
                {
                    MaxPages = 1000;
                    Logger.Instance.Error("Use Default Max Pages 1000");
                }

                UpdateMode = !updateMode.Equals("YES", StringComparison.OrdinalIgnoreCase);


                if (Host.TrimStart().TrimEnd().Equals("BIQUGE", StringComparison.OrdinalIgnoreCase) ||
                    Host.TrimStart().TrimEnd().Equals("BIQUGE.COM", StringComparison.OrdinalIgnoreCase))
                {
                    download = new BiqugeDownloadImagesText(Url, SaveTo, NovelName, UpdateMode, MaxPages);
                }
                else if (Host.TrimStart().TrimEnd().Equals("BIQUGE.CO", StringComparison.OrdinalIgnoreCase))
                {
                    download = new BiqugeCODownloadImagesText(Url, SaveTo, NovelName, UpdateMode, MaxPages);
                }
                else if (Host.TrimStart().TrimEnd().Equals("DAYANWENXUE", StringComparison.OrdinalIgnoreCase) ||
                    Host.TrimStart().TrimEnd().Equals("DAYANWENXUE.COM", StringComparison.OrdinalIgnoreCase))
                {
                    download = new DayanwenxueDownloadImagesText(Url, SaveTo, NovelName, UpdateMode, MaxPages);
                }
                else if (Host.TrimStart().TrimEnd().Equals("XIAOSHUOM", StringComparison.OrdinalIgnoreCase) ||
                    Host.TrimStart().TrimEnd().Equals("XIAOSHUOM.COM", StringComparison.OrdinalIgnoreCase))
                {
                    download = new XiaoshuomDownloadImagesText(Url, SaveTo, NovelName, UpdateMode, MaxPages);
                }
                else if (Host.TrimStart().TrimEnd().Equals("SHUKEJU", StringComparison.OrdinalIgnoreCase) ||
                    Host.TrimStart().TrimEnd().Equals("SHUKEJU.COM", StringComparison.OrdinalIgnoreCase))
                {
                    download = new ShukejuDownloadImagesText(Url, SaveTo, NovelName, UpdateMode, MaxPages);
                }
                else if (Host.TrimStart().TrimEnd().Equals("WENXUELOU", StringComparison.OrdinalIgnoreCase) ||
                    Host.TrimStart().TrimEnd().Equals("WENXUELOU.COM", StringComparison.OrdinalIgnoreCase))
                {
                    download = new WenxuelouDownloadImagesText(Url, SaveTo, NovelName, UpdateMode, MaxPages);
                }
                else if (Host.TrimStart().TrimEnd().Equals("BIQUGE", StringComparison.OrdinalIgnoreCase) ||
                    Host.TrimStart().TrimEnd().Equals("BIQUGE.CC", StringComparison.OrdinalIgnoreCase))
                {
                    download = new BiqugeCCDownloadImagesText(Url, SaveTo, NovelName, UpdateMode, MaxPages);
                }
                else if (Host.TrimStart().TrimEnd().Equals("1KANSHU", StringComparison.OrdinalIgnoreCase) ||
                    Host.TrimStart().TrimEnd().Equals("1KANSHU.COM", StringComparison.OrdinalIgnoreCase))
                {
                    download = new YaokanShuDownloadImagesText(Url, SaveTo, NovelName, UpdateMode, MaxPages);
                }
                else if (Host.TrimStart().TrimEnd().Equals("AOYECC", StringComparison.OrdinalIgnoreCase) ||
                    Host.TrimStart().TrimEnd().Equals("AOYE.CC", StringComparison.OrdinalIgnoreCase))
                {
                    download = new AoyeCCDownloadImagesText(Url, SaveTo, NovelName, UpdateMode, MaxPages);
                }
                else
                {
                    Logger.Instance.Error("Can't find plug-in for this host");

                    EndTime = DateTime.Now;
                    arg.Progress = ProgressEnum.FinishFailed;
                    FireOnProgressChangedEvent(arg);

                    return;
                }

   
                if (download != null)
                {
                    download.CopyFiles();
                    download.SaveToConfig();

                    string arguments = BuildArguments(host, url, novelName, maxPage, updateMode, saveTo);

                    download.SaveLastWorkingArguments(arguments);

                    download.OnProgressChanged += new ProgressChangeDelegate(download_OnProgressChanged);

                    thread = new Thread(new ThreadStart(download.ProcessAllOneByOne));
                    thread.Start();
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Error(ex.ToString());
                ProgressArgs arg = new ProgressArgs();
                arg.Progress = ProgressEnum.FinishFailed;
                FireOnProgressChangedEvent(arg);
            }
        }

        void download_OnProgressChanged(ProgressArgs arg)
        {
            if (arg.Progress == ProgressEnum.FinishOK || arg.Progress == ProgressEnum.FinishFailed)
            {
                EndTime = DateTime.Now;
                IsRunning = false;
            }

            Logger.Instance.Info(string.Format("Current Progress: Total={0} Current={1} Percentage={2} Progress={3}", arg.Total, arg.Current, arg.Percentage, Enum.GetName(typeof(ProgressEnum), arg.Progress)));
            FireOnProgressChangedEvent(arg);
        }
    }
}
