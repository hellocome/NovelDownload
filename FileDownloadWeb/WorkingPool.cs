using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FileDownload;

namespace FileDownloadWeb
{
    public class WorkingPool
    {
        private List<FileDownloadForWeb> mTaskList = new List<FileDownloadForWeb>();

        private static WorkingPool mInstance;
        public static WorkingPool Instance
        {
            get
            {
                if (mInstance == null)
                {
                    mInstance = new WorkingPool();
                }

                return mInstance;
            }
        }
    }

    public class WorkingPoolConfig
    {
        private static WorkingPoolConfig mInstance;
        public static WorkingPoolConfig Instance
        {
            get
            {
                if (mInstance == null)
                {
                    mInstance = new WorkingPoolConfig();
                }

                return mInstance;
            }
        } 
    }
}