using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FileDownload
{
    public enum ProgressEnum
    {
        NotStart,
        Progressing,
        FinishOK,
        FinishFailed
    }

    public class ProgressArgs
    {
        private int mTotal = 0;
        public int Total
        {
            get { return mTotal; }
            set { mTotal = value; }
        }

        private int mCurrent = 0;
        public int Current
        {
            get { return mCurrent; }
            set { mCurrent = value; }
        }

        private int mFailed = 0;
        public int Failed
        {
            get { return mFailed; }
            set { mFailed = value; }
        }

        private ProgressEnum mProgress = ProgressEnum.NotStart;
        public ProgressEnum Progress
        {
            get { return mProgress; }
            set { mProgress = value; }
        }

        public int RemainingTime = 0;

        public int Percentage
        {
            get
            {
                if (mProgress == ProgressEnum.NotStart)
                {
                    return 0;
                }

                if (mProgress != ProgressEnum.Progressing)
                {
                    return 100;
                }
                else
                {
                    if (mTotal > 0)
                    {
                        return mCurrent * 100 / mTotal;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
        }

    }
    public delegate void ProgressChangeDelegate(ProgressArgs arg);

    public class NovelDownloadArguments
    {
        public int StartFromIndex = 0;
    }

    public interface IDownloadImagesText
    {
        void ProcessAllOneByOne();
        bool SaveToConfig();
        bool CopyFiles();
        bool SaveLastWorkingArguments(string lastValidArgument);
        void Stop();

        event ProgressChangeDelegate OnProgressChanged;

        NovelDownloadArguments Argument
        {
            get;
        }
    }
}
