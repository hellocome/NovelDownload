using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Text.RegularExpressions;


using FileDownloadLib;

namespace FileDownload
{
    public abstract class BaseDownloadImagesText : IDownloadImagesText
    {
        protected const string PageContentNotExist = "<div class=\"divimage\">Image Not Exist [{0}]</div>";
        protected const string PageContentLine = "<div class=\"divimage\"><img src=\"{0}\" border=\"0\" class=\"imagecontent\"></div>";
        protected const string IndexContentLine = "<dd><a href=\"{0}\" title=\"{1}\">{2}</a></dd>";

        protected const string TAG_PAGE_CONTENT = "[PAGE_CONTENT]";
        protected const string TAG_TITLE = "[TITLE]";

        protected const string TAG_PREVIOUS_PAGE = "[PREVIOUS_PAGE]";
        protected const string TAG_NEXT_PAGE = "[NEXT_PAGE]";
        protected const string TAG_INDEX_PAGE = "[INDEX_PAGE]";
        protected const string TAG_BOOK_ID = "[BOOK_ID]";
        protected const string TAG_READ_ID = "[READ_ID]";

        protected string mIndexUri = string.Empty;
        protected string mSaveToRoot = string.Empty;
        protected string mNovelRoot = string.Empty;
        protected string mSaveToImageRoot = string.Empty;
        protected string mNovelTitle = string.Empty;

        protected string htmlIndexTemplate = "";
        protected string htmlPageTemplate = "";
        protected string novelJS = "";

        protected int Retry = 10;

        protected IndexContent mLastExistIndexContent = null;

        protected bool mUpdateMode = true;

        protected string[] REMOVESTRS = new string[] { @"http(s)?://([\w-]+.)+[\w-]+(/[\w- ./?%&=])?",
                                                       @"(?<http>(http:[/][/]|www.)([a-z]|[A-Z]|[0-9]|[/.]|[~])*)"};
        protected int mMaxPages = 1000;

        protected string CurrentUrl = string.Empty;

        protected string HttpMainHost = string.Empty;

        protected string HttpImageHost = string.Empty;

        private NovelDownloadArguments mArgument = new NovelDownloadArguments();
        public NovelDownloadArguments Argument
        {
            get
            {
                return mArgument;
            }
        }

        public BaseDownloadImagesText(string indexUri, string saveToRoot, string novelTitle,
            bool UpdateMode, int MaxPages, string httpMainHost, string httpImageHost)
        {
            novelTitle = FileDownloadUtil.GetInvalidPath(novelTitle);
            saveToRoot = FileDownloadUtil.GetInvalidPath(saveToRoot);

            mUpdateMode = UpdateMode;
            mNovelTitle = novelTitle;

            mIndexUri = FileDownloadUtil.GetCorrectCurrentUrl(indexUri);

            mSaveToRoot = string.Format("{0}\\{1}", saveToRoot, novelTitle);
            mSaveToImageRoot = string.Format("{0}\\Images", mSaveToRoot);

            mNovelRoot = mSaveToRoot;
            mMaxPages = MaxPages;

            htmlIndexTemplate = string.Format("{0}\\HTTPTemplateIndex.html", AppDomain.CurrentDomain.BaseDirectory);
            htmlPageTemplate = string.Format("{0}\\HTMLTemplatePages.html", AppDomain.CurrentDomain.BaseDirectory);
            novelJS = string.Format("{0}\\novel.js", AppDomain.CurrentDomain.BaseDirectory);

            HttpMainHost = httpMainHost;

            HttpImageHost = httpImageHost;

            CurrentUrl = FileDownloadUtil.GetCorrectCurrentUrl(FileDownloadUtil.CombindCorrectUrl(HttpMainHost, indexUri));
        }


        private bool mStop = false;
        protected bool StopJob
        {
            get { return mStop; }
            private set { mStop = value; }
        }
        public virtual void Stop()
        {
            Logger.Instance.Info("Stop is trigged, but it might take a while to stop");
            StopJob = true;
        }

        public event ProgressChangeDelegate OnProgressChanged = delegate { }; 

        public void FireOnProgressChangedEvent(int total, int current, ProgressEnum progress)
        {
            try
            {
                ProgressArgs arg = new ProgressArgs();
                arg.Total = total;
                arg.Current = current;
                arg.Progress = progress;

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

        public void FireOnProgressChangedEvent(int total, int current, int timeRemaining, int failed, ProgressEnum progress)
        {
            try
            {
                ProgressArgs arg = new ProgressArgs();
                arg.Total = total;
                arg.Current = current;
                arg.Progress = progress;
                arg.RemainingTime = timeRemaining; 
                arg.Failed = failed;


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

        public virtual void ProcessAllOneByOne()
        {
            bool Test = false;
            if (Test)
            {
                Test_RegexTest_Text();
            }

            StopJob = false;

            List<PageInfo> pageInfoList = new List<PageInfo>();
            List<IndexContent> pageIndexList = new List<IndexContent>();
            Encoding encoding = null;

            try
            {
                FireOnProgressChangedEvent(0, 0, ProgressEnum.Progressing);

#if DEBUG 
                if(ProcessToGetPageInfosOneByOne(ref pageInfoList, ref pageIndexList, ref encoding))
                {
                    Logger.Instance.Info("ProcessAllOneByOne OK");
                    FireOnProgressChangedEvent(pageIndexList.Count, pageIndexList.Count, ProgressEnum.FinishOK);
                }
#else
                if (ProcessToGetPageInfosOneByOneInThreadPool(ref pageInfoList, ref pageIndexList, ref encoding))
                {
                    Logger.Instance.Info("ProcessAllOneByOne OK");
                }
#endif
                else
                {
                    Logger.Instance.Info("ProcessAllOneByOne Failed");
                    FireOnProgressChangedEvent(pageIndexList.Count, pageIndexList.Count, ProgressEnum.FinishOK);
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Error(ex.ToString());
                FireOnProgressChangedEvent(pageIndexList.Count, pageIndexList.Count, ProgressEnum.FinishFailed);
                return;
            }

            
        }

        protected virtual bool ProcessToGetPageInfosOneByOne(ref List<PageInfo> pageInfoList, ref List<IndexContent> indexList, ref Encoding encoding)
        {
            int count = 0;
            int failed = 0;

            pageInfoList = new List<PageInfo>();

            if (ProcessIndex(ref indexList, ref encoding))
            {
                if (indexList != null)
                {
                    Logger.Instance.Info("Build Index!");
                    BuildIndex(indexList, encoding, mNovelTitle);

                    Logger.Instance.Info("Get Page Infor List OK!");

                    FireOnProgressChangedEvent(indexList.Count, 0, ProgressEnum.Progressing);

                    if (!Directory.Exists(mSaveToRoot))
                    {
                        Directory.CreateDirectory(mSaveToRoot);
                    }

                    DirectoryInfo di = new DirectoryInfo(mSaveToRoot);
                    Logger.Instance.Info("Save To: " + di.FullName);

                    StreamReader SR = new StreamReader(new FileStream(htmlPageTemplate, FileMode.Open, FileAccess.Read));

                    string tempContent = SR.ReadToEnd();

                    SR.Close();

                    if (encoding == null)
                    {
                        encoding = Encoding.UTF8;
                    }

                    int total = indexList.Count > mMaxPages ? mMaxPages : indexList.Count;
                    double totalTime = 0;

                    DateTime oneJobStart = DateTime.Now;

                    bool onePageRes = true;
                    foreach (IndexContent indexContent in indexList)
                    {
                        onePageRes = true;

                        if (StopJob)
                        {
                            break;
                        }

                        count++;

                        if (Argument.StartFromIndex > 0 && count < Argument.StartFromIndex)
                        {
                            continue;
                        }

                        if (count > mMaxPages)
                        {
                            Logger.Instance.InfoImportant("Reach Max Pages: " + mMaxPages);
                            break;
                        }

                        Logger.Instance.Info("Processing Record: " + count);

                        string file = string.Format("{0}/{1}", mSaveToRoot, indexContent.FileName);

                        if (File.Exists(file) && mUpdateMode)
                        {
                            Logger.Instance.InfoImportant("Update Mode File Exists: " + new FileInfo(file).Name);

                            mLastExistIndexContent = indexContent;

                            continue;
                        }


                        PageInfo piLastExists = null;
                        if (mLastExistIndexContent != null && mUpdateMode)
                        {
                            if (ProcessOnePage(mLastExistIndexContent.Url, mLastExistIndexContent, ref piLastExists))
                            {
                                if (piLastExists != null && piLastExists.ValidPageInfor())
                                {
                                    pageInfoList.Add(piLastExists);
                                    DownloadImages(piLastExists, !mUpdateMode);

                                    if (piLastExists.ValidPageContent())
                                    {
                                        BuildPage(piLastExists, tempContent, encoding);
                                    }
                                }
                                else
                                {
                                    Logger.Instance.InfoImportant("Invalid File Content: (could be a coding issue)" + new FileInfo(file).Name);
                                }
                            }

                            mLastExistIndexContent = null;
                        }


                        PageInfo pi = null;
                        if (ProcessOnePage(indexContent.Url, indexContent, ref pi))
                        {
                            if (pi != null && pi.ValidPageInfor())
                            {
                                pageInfoList.Add(pi);
                                DownloadImages(pi, !mUpdateMode);

                                if (pi.ValidPageContent())
                                {
                                    if (!BuildPage(pi, tempContent, encoding))
                                    {
                                        onePageRes = false;
                                    }
                                }

                            }
                            else
                            {
                                Logger.Instance.InfoImportant("Invalid File Content: (could be a coding issue)" + new FileInfo(file).Name);
                                onePageRes = false;
                            }
                        }
                        else
                        {
                            onePageRes = false;
                        }

                        if (!onePageRes)
                        {
                            failed++;
                        }

                        int timeRemaining = ((count == 0 || count == 1) ? 0 : (int)((indexList.Count - count - 1) * totalTime / count - 1));
                        FireOnProgressChangedEvent(indexList.Count, count, timeRemaining, failed, ProgressEnum.Progressing);

                        DateTime oneJobEnd= DateTime.Now;

                        totalTime = oneJobEnd.Subtract(oneJobStart).TotalSeconds;
                    }


                    return true;
                }
            }

            Logger.Instance.Info("Get Page Infor List Failed!");
            return false;
        }

        protected virtual bool ProcessToGetPageInfosOneByOneInThreadPool(ref List<PageInfo> pageInfoList, ref List<IndexContent> indexList, ref Encoding encoding)
        {
            int count = 0;
            int failed = 0;

            pageInfoList = new List<PageInfo>();

            if (ProcessIndex(ref indexList, ref encoding))
            {
                if (indexList != null)
                {
                    Logger.Instance.Info("Build Index!");
                    BuildIndex(indexList, encoding, mNovelTitle);

                    Logger.Instance.Info("Get Page Infor List OK!");

                    FireOnProgressChangedEvent(indexList.Count, 0, ProgressEnum.Progressing);

                    if (!Directory.Exists(mSaveToRoot))
                    {
                        Directory.CreateDirectory(mSaveToRoot);
                    }

                    DirectoryInfo di = new DirectoryInfo(mSaveToRoot);
                    Logger.Instance.Info("Save To: " + di.FullName);

                    StreamReader SR = new StreamReader(new FileStream(htmlPageTemplate, FileMode.Open, FileAccess.Read));

                    string tempContent = SR.ReadToEnd();

                    SR.Close();

                    if (encoding == null)
                    {
                        encoding = Encoding.UTF8;
                    }

                    int total = indexList.Count > mMaxPages ? mMaxPages : indexList.Count;
                    

                    DateTime oneJobStart = DateTime.Now;

                    double totalTime = 0;

                    Queue<IndexContent> indexQueue = new Queue<IndexContent>();

                    foreach (IndexContent indexContent in indexList)
                    {
                        if (StopJob)
                        {
                            break;
                        }

                        if (Argument.StartFromIndex > 0 && count < Argument.StartFromIndex)
                        {
                            count++;
                            continue;
                        }
                        else
                        {
                            indexQueue.Enqueue(indexContent);
                        }
                    }

                    Arg arg = new Arg();
                    arg.indexQueue = indexQueue;
                    arg.pageInfoList = pageInfoList;
                    arg.count = count;
                    arg.failed = failed;
                    arg.encoding = encoding;
                    arg.tempContent = tempContent;
                    arg.indexListCount = indexList.Count;
                    arg.oneJobStart = oneJobStart;
                    arg.totalTime = totalTime;

                    ProcessByThreadPool(arg);

                    return true;
                }
            }

            Logger.Instance.Info("Get Page Infor List Failed!");
            return false;
        }

        class Arg
        {
            public Queue<IndexContent> indexQueue;
            public List<PageInfo> pageInfoList;
            public int count;
            public int failed;
            public Encoding encoding;
            public string tempContent;
            public int indexListCount;
            public DateTime oneJobStart;
            public double totalTime;
        }

        Dictionary<int, bool> threadPools = new Dictionary<int, bool>();

        public void ProcessByThreadPool(object obj)
        {
            threadPools.Clear();

            for (int i = 0; i < 10; i++)
            {
                System.Threading.Thread thread = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(Run));

                threadPools.Add(thread.ManagedThreadId, false);
                thread.Start(obj);
            }
        }

        public void Run(object obj)
        {
            Arg arg = obj as Arg;

            Run(ref arg.indexQueue, ref arg.pageInfoList, ref arg.count,
            ref arg.failed, arg.encoding, arg.tempContent, arg.indexListCount, arg.oneJobStart, ref arg.totalTime);
        }

        protected virtual void FailedRetryQueueProcess()
        {
            while (!StopJob)
            {

            }
        }

        protected virtual void Run(ref Queue<IndexContent> indexQueue, ref List<PageInfo> pageInfoList, ref int count, 
            ref int failed, Encoding encoding, string tempContent, int indexListCount, DateTime oneJobStart, ref double totalTime)
        {
            threadPools[System.Threading.Thread.CurrentThread.ManagedThreadId] = true;

            while (!StopJob)
            {
                if (count > mMaxPages)
                {
                    Logger.Instance.InfoImportant("Reach Max Pages: " + mMaxPages);
                    break;
                }

                bool onePageRes = true;

                IndexContent indexContent = null;

                lock(indexQueue)
                {
                    if(indexQueue.Count>0)
                    {
                        indexContent = indexQueue.Dequeue();
                    }
                    else
                    {
                        break;
                    }
                }

                count++;

                Logger.Instance.Info("Processing Record: " + count);

                string file = string.Format("{0}/{1}", mSaveToRoot, indexContent.FileName);

                if (File.Exists(file) && mUpdateMode)
                {
                    Logger.Instance.InfoImportant("Update Mode File Exists: " + new FileInfo(file).Name);

                    mLastExistIndexContent = indexContent;

                    continue;
                }


                PageInfo piLastExists = null;
                if (mLastExistIndexContent != null && mUpdateMode)
                {
                    if (ProcessOnePage(mLastExistIndexContent.Url, mLastExistIndexContent, ref piLastExists))
                    {
                        if (piLastExists != null && piLastExists.ValidPageInfor())
                        {
                            pageInfoList.Add(piLastExists);
                            DownloadImages(piLastExists, !mUpdateMode);

                            if (piLastExists.ValidPageContent())
                            {
                                BuildPage(piLastExists, tempContent, encoding);
                            }
                        }
                        else
                        {
                            Logger.Instance.InfoImportant("Invalid File Content: (could be a coding issue)" + new FileInfo(file).Name);
                        }
                    }

                    mLastExistIndexContent = null;
                }


                PageInfo pi = null;
                if (ProcessOnePage(indexContent.Url, indexContent, ref pi))
                {
                    if (pi != null && pi.ValidPageInfor())
                    {
                        pageInfoList.Add(pi);
                        DownloadImages(pi, !mUpdateMode);

                        if (pi.ValidPageContent())
                        {
                            if (!BuildPage(pi, tempContent, encoding))
                            {
                                onePageRes = false;
                            }
                        }

                    }
                    else
                    {
                        Logger.Instance.InfoImportant("Invalid File Content: (could be a coding issue)" + new FileInfo(file).Name);
                        onePageRes = false;
                    }
                }
                else
                {
                    onePageRes = false;
                }

                if (!onePageRes)
                {
                    failed++;
                }

                int timeRemaining = ((count == 0 || count == 1) ? 0 : (int)((indexListCount - count - 1) * totalTime / count - 1));
                FireOnProgressChangedEvent(indexListCount, count, timeRemaining, failed, ProgressEnum.Progressing);

                DateTime oneJobEnd = DateTime.Now;

                totalTime = oneJobEnd.Subtract(oneJobStart).TotalSeconds;
            }

            threadPools[System.Threading.Thread.CurrentThread.ManagedThreadId] = false;

            lock (indexQueue)
            {
                indexQueue.Clear();

                bool result = true;
                foreach (int key in threadPools.Keys)
                {
                    if (threadPools[key])
                    {
                        result = false;
                    }
                }

                if (result)
                {
                    FireOnProgressChangedEvent(indexListCount, count, ProgressEnum.FinishOK);
                }
            }
        }

        protected virtual bool DownloadImages(PageInfo pi, bool overwrite)
        {
            try
            {
                if (!Directory.Exists(mSaveToImageRoot))
                {
                    Directory.CreateDirectory(mSaveToImageRoot);
                }

                int piCount = 0;
                int imgCount = 0;

                foreach (string image in pi.ImageList)
                {
                    string FileName = image.Substring(image.LastIndexOf("/") + 1,
                        (image.Length - image.LastIndexOf("/") - 1));

                    string saveToLoc = string.Format("{0}\\{1}", mSaveToImageRoot, FileName);

                    if (File.Exists(saveToLoc) && overwrite)
                    {
                        Logger.Instance.InfoImportant("Over write mode, Delete: " + new FileInfo(saveToLoc).Name);
                        File.Delete(saveToLoc);
                    }
                    else if (File.Exists(saveToLoc) && !overwrite)
                    {
                        Logger.Instance.InfoImportant("Update Mode, File Exists: " + new FileInfo(saveToLoc).Name);
                        pi.DownloadedImageList.Add(FileName);
                        continue;
                    }

                    if (FileDownloadUtil.DownloadFile(image, saveToLoc))
                    {
                        Logger.Instance.Info(string.Format("Dwonload OK: {0}", image));
                        pi.DownloadedImageList.Add(FileName);

                        imgCount++;
                    }
                    else
                    {
                        Logger.Instance.Info(string.Format("Dwonload Failed: {0}", image));
                    }

                    piCount++;
                    Logger.Instance.Info("Download Image For PI: " + piCount);
                    Logger.Instance.Info("Download Image: " + imgCount);
                }


                return true;
            }
            catch (Exception ex)
            {
                Logger.Instance.Error(ex.ToString());
                return false;
            }
        }

        protected virtual bool BuildPages(List<PageInfo> pageInfoList, Encoding encoding)
        {
            try
            {
                if (!Directory.Exists(mSaveToRoot))
                {
                    Directory.CreateDirectory(mSaveToRoot);
                }


                StreamReader SR = new StreamReader(new FileStream(htmlPageTemplate, FileMode.Open, FileAccess.Read));

                string tempContent = SR.ReadToEnd();

                SR.Close();

                if (encoding == null)
                {
                    encoding = Encoding.UTF8;
                }

                foreach (PageInfo pi in pageInfoList)
                {
                    Logger.Instance.InfoImportant("Build PI: " + pi.readid);
                    BuildPage(pi, tempContent, encoding);
                }

                return true;
            }
            catch (Exception ex)
            {
                Logger.Instance.Error(ex.ToString());
                return false;
            }
        }

        protected virtual bool BuildPage(PageInfo pi, string pageContentTemplate, Encoding encoding)
        {
            try
            {
                StringBuilder indexStr = new StringBuilder();
                string tempContent = string.Empty;

                if (pi.ContentType == ContentTypeEnum.Image)
                {
                    Logger.Instance.InfoImportant("Image Mode");
                    foreach (string image in pi.DownloadedImageList)
                    {
                        if (File.Exists(string.Format("{0}\\{1}", mSaveToImageRoot, image)))
                        {
                            tempContent += string.Format(PageContentLine, string.Format("Images\\{0}", image));
                        }
                        else
                        {
                            tempContent += string.Format(PageContentNotExist, string.Format("Images\\{0}", image));
                        }
                    }
                }
                else if (pi.ContentType == ContentTypeEnum.Text)
                {
                    Logger.Instance.InfoImportant("Text Mode");
                    tempContent = pi.TextContent;
                }

                pageContentTemplate = pageContentTemplate.Replace(TAG_PAGE_CONTENT, tempContent);
                pageContentTemplate = pageContentTemplate.Replace(TAG_PREVIOUS_PAGE, pi.preview_page);
                pageContentTemplate = pageContentTemplate.Replace(TAG_NEXT_PAGE, pi.next_page);
                pageContentTemplate = pageContentTemplate.Replace(TAG_INDEX_PAGE, pi.index_page);
                pageContentTemplate = pageContentTemplate.Replace(TAG_BOOK_ID, pi.bookid);
                pageContentTemplate = pageContentTemplate.Replace(TAG_READ_ID, pi.readid);
                pageContentTemplate = pageContentTemplate.Replace(TAG_TITLE, pi.IndexContent.TitleContent);


                string file = string.Format("{0}\\{1}", mSaveToRoot, pi.IndexContent.FileName);

                if (File.Exists(file) && !mUpdateMode)
                {
                    Logger.Instance.InfoImportant("Over write mode, Delete: " + new FileInfo(file).Name);
                    File.Delete(file);
                }

                if (!File.Exists(file))
                {
                    Logger.Instance.InfoImportant("Create New File: " + new FileInfo(file).Name);
                    StreamWriter SW = new StreamWriter(new FileStream(file, FileMode.CreateNew, FileAccess.Write), encoding);

                    SW.Write(pageContentTemplate);
                    SW.Flush();
                    SW.Close();
                }
                else
                {
                    Logger.Instance.InfoImportant("File Exists Skip: " + file);
                }

                return true;
            }
            catch (Exception ex)
            {
                Logger.Instance.Error(ex.ToString());
                return false;
            }
        }

        protected virtual bool BuildIndex(List<IndexContent> pageIndexList, Encoding encoding, string book)
        {
            try
            {
                StringBuilder indexStr = new StringBuilder();
                foreach (IndexContent ic in pageIndexList)
                {
                    string appendstr = string.Format(IndexContentLine, ic.FileName, ic.Title, ic.TitleContent);
                    indexStr.AppendLine(appendstr);
                }

                StreamReader SR = new StreamReader(new FileStream(htmlIndexTemplate, FileMode.Open, FileAccess.Read));

                string tempContent = SR.ReadToEnd();

                tempContent = tempContent.Replace(TAG_PAGE_CONTENT, indexStr.ToString());
                tempContent = tempContent.Replace(TAG_TITLE, book);

                string file = string.Format("{0}\\index.html", mSaveToRoot);

                if (File.Exists(file))
                {
                    File.Delete(file);
                }

                if (encoding == null)
                {
                    encoding = Encoding.UTF8;
                }

                FileInfo fi = new FileInfo(file);

                if (!Directory.Exists(fi.DirectoryName))
                {
                    Directory.CreateDirectory(fi.DirectoryName);
                }

                StreamWriter SW = new StreamWriter(new FileStream(file, FileMode.CreateNew, FileAccess.Write), encoding);

                SW.Write(tempContent);
                SW.Flush();

                SR.Close();
                SW.Close();

                return true;
            }
            catch (Exception ex)
            {
                Logger.Instance.Error(ex.ToString());
                return false;
            }
        }

        protected virtual bool ProcessIndex(ref List<IndexContent> indexList, ref Encoding encoding)
        {
            string content = string.Empty;

            int count = 0;
            bool res = false;

            while (count < Retry)
            {
                res = FileDownloadUtil.DownloadWebPage(HttpMainHost, mIndexUri, ref content, ref encoding);

                if (res)
                {
                    break;
                }

                Logger.Instance.Error(string.Format("Download Page Failed! [{0}] = Index", count));
                count++;
            }

            if (!res)
            {
                Logger.Instance.Error("Download Index Failed!");
                return false;
            }
            else
            {
                Logger.Instance.Info("Download Index OK!");

                indexList = GetIndex(content);

                return true;
            }
        }



        protected virtual bool ProcessOnePage(string url, IndexContent indexContent, ref PageInfo pi)
        {
            string content = string.Empty;
            Encoding encoding = null;

            string rooturl = FileDownloadUtil.GetRootUrl(HttpMainHost, CurrentUrl, url);

            int count = 0;
            bool res = false;

            while (count < Retry)
            {
                res = FileDownloadUtil.DownloadWebPage(rooturl, url, ref content, ref encoding);

                if(res)
                {
                    break;
                }

                Logger.Instance.Error(string.Format("Download Page Failed! [{0}] = {1}", count, url));
                count++;
            }

            if (!res )
            {
                Logger.Instance.Error(string.Format("Download Page Failed! [{0}]", url));
                return false;
            }
            else
            {
                pi = GetPageInfo(content, indexContent);

                return true;
            }
        }

        #region need to override most of time
        protected abstract List<IndexContent> GetIndex(string htmlContent);

        protected abstract PageInfo GetPageInfo(string htmlContent, IndexContent indexContent);
        #endregion

        public virtual bool SaveToConfig()
        {
            try
            {
                string configFile = string.Format("{0}\\config.xml", mNovelRoot);
                XmlDocument xmlDoc = new XmlDocument();

                if (!Directory.Exists(mNovelRoot))
                {
                    Directory.CreateDirectory(mNovelRoot);
                }

                if (File.Exists(configFile))
                {
                    xmlDoc.Load(configFile);

                    XmlNode rootNode = xmlDoc.SelectSingleNode("NovelConfig");

                    if (rootNode == null)
                    {
                        rootNode = xmlDoc.CreateElement("NovelConfig");
                        xmlDoc.AppendChild(rootNode);
                    }


                    Dictionary<string, XmlElement> dic = new Dictionary<string, XmlElement>();

                    XmlElement configEleIndexUri = xmlDoc.CreateElement("config");
                    configEleIndexUri.SetAttribute("name", "IndexUri");
                    configEleIndexUri.SetAttribute("value", mIndexUri);
                    dic.Add("IndexUri", configEleIndexUri);

                    XmlElement configEleSaveToRoot = xmlDoc.CreateElement("config");
                    configEleSaveToRoot.SetAttribute("name", "SaveToRoot");
                    configEleSaveToRoot.SetAttribute("value", mSaveToRoot);
                    dic.Add("SaveToRoot", configEleSaveToRoot);

                    XmlElement configEleNovelRoot = xmlDoc.CreateElement("config");
                    configEleNovelRoot.SetAttribute("name", "NovelRoot");
                    configEleNovelRoot.SetAttribute("value", mNovelRoot);
                    dic.Add("NovelRoot", configEleNovelRoot);

                    XmlElement configEleSaveToImageRoot = xmlDoc.CreateElement("config");
                    configEleSaveToImageRoot.SetAttribute("name", "SaveToImageRoot");
                    configEleSaveToImageRoot.SetAttribute("value", mSaveToImageRoot);
                    dic.Add("SaveToImageRoot", configEleSaveToImageRoot);


                    XmlElement configElemUpdateMode = xmlDoc.CreateElement("config");
                    configElemUpdateMode.SetAttribute("name", "UpdateMode");
                    configElemUpdateMode.SetAttribute("value", mUpdateMode ? "true" : "false");
                    dic.Add("UpdateMode", configElemUpdateMode);

                    XmlElement configEleMaxPages = xmlDoc.CreateElement("config");
                    configEleMaxPages.SetAttribute("name", "MaxPages");
                    configEleMaxPages.SetAttribute("value", mMaxPages.ToString());
                    dic.Add("MaxPages", configEleMaxPages);

                    XmlNodeList nodeList = xmlDoc.SelectNodes("NovelConfig/config");

                    foreach (XmlNode node in nodeList)
                    {
                        XmlElement ele = node as XmlElement;

                        if (ele.GetAttribute("name").Equals("IndexUri", StringComparison.OrdinalIgnoreCase))
                        {
                            ele.SetAttribute("value", mIndexUri);
                            dic.Remove("IndexUri");
                        }

                        if (ele.GetAttribute("name").Equals("SaveToRoot", StringComparison.OrdinalIgnoreCase))
                        {
                            ele.SetAttribute("value", mSaveToRoot);
                            dic.Remove("SaveToRoot");
                        }


                        if (ele.GetAttribute("name").Equals("NovelRoot", StringComparison.OrdinalIgnoreCase))
                        {
                            ele.SetAttribute("value", mNovelRoot);
                            dic.Remove("NovelRoot");
                        }

                        if (ele.GetAttribute("name").Equals("SaveToImageRoot", StringComparison.OrdinalIgnoreCase))
                        {
                            ele.SetAttribute("value", mSaveToImageRoot);
                            dic.Remove("SaveToImageRoot");
                        }


                        if (ele.GetAttribute("name").Equals("NovelTitle", StringComparison.OrdinalIgnoreCase))
                        {
                            ele.SetAttribute("value", mNovelTitle);
                            dic.Remove("NovelTitle");
                        }


                        if (ele.GetAttribute("name").Equals("UpdateMode", StringComparison.OrdinalIgnoreCase))
                        {
                            ele.SetAttribute("value", mUpdateMode ? "true" : "false");
                            dic.Remove("UpdateMode");
                        }


                        if (ele.GetAttribute("name").Equals("MaxPages", StringComparison.OrdinalIgnoreCase))
                        {
                            ele.SetAttribute("value", mMaxPages.ToString());
                            dic.Remove("MaxPages");
                        }
                    }

                    foreach (string key in dic.Keys)
                    {
                        rootNode.AppendChild(dic[key]);
                    }


                    return true;
                }
                else
                {

                    XmlElement rootNode = xmlDoc.CreateElement("NovelConfig");

                    xmlDoc.AppendChild(rootNode);

                    XmlElement configEle = xmlDoc.CreateElement("config");
                    configEle.SetAttribute("name", "IndexUri");
                    configEle.SetAttribute("value", mIndexUri);
                    rootNode.AppendChild(configEle);


                    configEle = xmlDoc.CreateElement("config");
                    configEle.SetAttribute("name", "SaveToRoot");
                    configEle.SetAttribute("value", mSaveToRoot);
                    rootNode.AppendChild(configEle);


                    configEle = xmlDoc.CreateElement("config");
                    configEle.SetAttribute("name", "NovelRoot");
                    configEle.SetAttribute("value", mNovelRoot);
                    rootNode.AppendChild(configEle);

                    configEle = xmlDoc.CreateElement("config");
                    configEle.SetAttribute("name", "SaveToImageRoot");
                    configEle.SetAttribute("value", mSaveToImageRoot);
                    rootNode.AppendChild(configEle);

                    configEle = xmlDoc.CreateElement("config");
                    configEle.SetAttribute("name", "NovelTitle");
                    configEle.SetAttribute("value", mNovelTitle);
                    rootNode.AppendChild(configEle);

                    configEle = xmlDoc.CreateElement("config");
                    configEle.SetAttribute("name", "UpdateMode");
                    configEle.SetAttribute("value", mUpdateMode ? "true" : "false");
                    rootNode.AppendChild(configEle);

                    configEle = xmlDoc.CreateElement("config");
                    configEle.SetAttribute("name", "MaxPages");
                    configEle.SetAttribute("value", mMaxPages.ToString());
                    rootNode.AppendChild(configEle);
                }

                xmlDoc.Save(configFile);

                return true;
            }
            catch (Exception ex)
            {
                Logger.Instance.Error(ex.ToString());
                return false;

            }
        }

        public virtual bool SaveLastWorkingArguments(string lastValidArgument)
        {
            try
            {
                string configFile = string.Format("{0}\\config.xml", mNovelRoot);

                XmlDocument xmlDoc = new XmlDocument();

                if (!Directory.Exists(mNovelRoot))
                {
                    Directory.CreateDirectory(mNovelRoot);
                }

                if (File.Exists(configFile))
                {
                    xmlDoc.Load(configFile);
                }

                XmlNode rootNode = xmlDoc.SelectSingleNode("NovelConfig");

                if (rootNode == null)
                {
                    rootNode = xmlDoc.CreateElement("NovelConfig");
                    xmlDoc.AppendChild(rootNode);
                }

                XmlNodeList nodeList = xmlDoc.SelectNodes("NovelConfig/config");

                if (nodeList != null)
                {
                    foreach (XmlNode node in nodeList)
                    {
                        XmlElement ele = node as XmlElement;

                        if (ele.GetAttribute("name").Equals("LastValidArgument", StringComparison.OrdinalIgnoreCase))
                        {
                            ele.SetAttribute("value", lastValidArgument);
                            xmlDoc.Save(configFile);
                            return true;
                        }
                    }
                }

                XmlElement configEle = xmlDoc.CreateElement("config");
                configEle.SetAttribute("name", "LastValidArgument");
                configEle.SetAttribute("value", lastValidArgument);
                rootNode.AppendChild(configEle);
                xmlDoc.Save(configFile);
                return true;
            }
            catch (Exception ex)
            {
                Logger.Instance.Error(ex.ToString());
                return false;
            }
        }

        public virtual bool CopyFiles()
        {
            bool res = true;
            try
            {
                if (!Directory.Exists(mNovelRoot))
                {
                    Directory.CreateDirectory(mNovelRoot);

                    Logger.Instance.Info("Directory Doesn't Exist And Create: " + mNovelRoot);
                }
                else
                {
                    Logger.Instance.Info("Directory Exists: " + mNovelRoot);
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Error(ex.ToString());
                res = false;
            }

            try
            {
                if (File.Exists(htmlIndexTemplate))
                {
                    FileInfo fi = new FileInfo(htmlIndexTemplate);
                    string sourceFile = string.Format("{0}\\{1}", mNovelRoot, fi.Name);
                    File.Copy(htmlIndexTemplate, sourceFile, true);
                }

            }
            catch (Exception ex)
            {
                Logger.Instance.Error(ex.ToString());
                res = false;
            }

            try
            {
                if (File.Exists(htmlPageTemplate))
                {
                    FileInfo fi = new FileInfo(htmlPageTemplate);
                    string sourceFile = string.Format("{0}\\{1}", mNovelRoot, fi.Name);
                    File.Copy(htmlPageTemplate, sourceFile, true);
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Error(ex.ToString());
                res = false;
            }

            try
            {
                if (File.Exists(novelJS))
                {
                    FileInfo fi = new FileInfo(novelJS);
                    string sourceFile = string.Format("{0}\\{1}", mNovelRoot, fi.Name);
                    File.Copy(novelJS, sourceFile, true);
                }

            }
            catch (Exception ex)
            {
                Logger.Instance.Error(ex.ToString());
                res = false;
            }

            try
            {
                string exeFile = string.Format("{0}\\FileDownload.exe", AppDomain.CurrentDomain.BaseDirectory);//System.Reflection.Assembly.GetEntryAssembly().Location;
                
                Logger.Instance.Info(exeFile);
                if (File.Exists(exeFile))
                {
                    FileInfo fi = new FileInfo(exeFile);
                    string sourceFile = string.Format("{0}\\{1}", mNovelRoot, fi.Name);
                    File.Copy(exeFile, sourceFile, true);
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Error(ex.ToString());
                res = false;
            }


            return res;
        }

        public virtual string RemoveInvalidString(string baseStr)
        {
            if (REMOVESTRS != null)
            {
                foreach (string regStr in REMOVESTRS)
                {
                    baseStr = System.Text.RegularExpressions.Regex.Replace(baseStr, regStr, "");
                }
            }

            return baseStr;
        }

        #region Test Cases
        public void Test_RegexTest_Index()
        {
            try
            {
                StreamReader fs = new StreamReader(new FileStream("Test.txt", FileMode.Open, FileAccess.Read), Encoding.Unicode);

                string content = fs.ReadToEnd();

                GetIndex(content);
            }
            catch (Exception ex)
            {
                Logger.Instance.Error(ex.ToString());
            }
        }

        public void Test_RegexTest_Text()
        {
            try
            {
                while (true)
                {
                    StreamReader fs = new StreamReader(new FileStream("TestText.txt", FileMode.Open, FileAccess.Read), Encoding.Unicode);

                    string content = fs.ReadToEnd();

                    string pattern_text = "<div id=\"txtright\"><script type=\"text/javascript\">txtrightshow\\(\\);</script></div>(?<textValue>((?!</div>)(?!</pre>)[\\s\\S])*)</pre>";
                    //"<div id=\"content\">(?<textValue>((?!</div>)[\\s\\S])*)</div>";

                    Match match = Regex.Match(content, pattern_text);
                    string textValue = match.Groups["textValue"].Value;
                    fs.Close();
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Error(ex.ToString());
            }
        }

        public void Test_RegexTest_Page()
        {
            try
            {
                StreamReader fs = new StreamReader(new FileStream("TestPage.txt", FileMode.Open, FileAccess.Read), Encoding.Unicode);

                string content = fs.ReadToEnd();

                //GetPageInfo(content);
            }
            catch (Exception ex)
            {
                Logger.Instance.Error(ex.ToString());
            }
        }
        #endregion


    }

    public class IndexContent
    {
        public string LineContent = string.Empty;
        public string Url = string.Empty;
        public string Title = string.Empty;
        public string TitleContent = string.Empty;
        public string FileName = string.Empty;
    }

    public enum ContentTypeEnum
    {
        Unknown,
        Text,
        Image
    }

    public class PageInfo
    {
        public string preview_page = string.Empty;
        public string next_page = string.Empty;
        public string index_page = string.Empty;
        public string bookid = string.Empty;
        public string readid = string.Empty;
        public IndexContent IndexContent = null;

        public ContentTypeEnum ContentType = ContentTypeEnum.Unknown;

        public string TextContent = string.Empty;


        public List<string> DownloadedImageList = new List<string>();
        public List<string> ImageList = new List<string>();

        public bool ValidPageInfor()
        {
            bool validRes = (IndexContent != null) ;

            if (!validRes)
            {
                Logger.Instance.Error("IndexContent is NULL");
            }

            return validRes;
        }

        public bool ValidPageContent()
        {
            bool validRes = (DownloadedImageList.Count > 0 || !string.IsNullOrEmpty(TextContent));

            if (!validRes)
            {
                Logger.Instance.Error("Download Image and Text content is Emptty");
            }

            return validRes;
        }
    }

}
