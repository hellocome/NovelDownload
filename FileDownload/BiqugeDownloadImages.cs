using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using FileDownloadLib;

namespace FileDownload
{
    public class BiqugeDownloadImages
    {
        private const string HttpMainHost = "www.biquge.com";
        private const string HttpImageHost = "http://img.biquge.com:8080/pic";

        private const string PageContentNotExist = "<div class=\"divimage\">Image Not Exist [{0}]</div>";
        private const string PageContentLine = "<div class=\"divimage\"><img src=\"{0}\" border=\"0\" class=\"imagecontent\"></div>";
        private const string IndexContentLine = "<dd><a href=\"{0}.html\" title=\"{1}\">{2}</a></dd>";

        private string mIndexUri = string.Empty;
        private string mSaveToRoot = string.Empty;
        private string mSaveToImageRoot = string.Empty;
        private string mNovelTitle = string.Empty;

        private string htmlIndexTemplate = "";
        private string htmlPageTemplate = "";

        public bool OverWriteImage = false;



        private const string TAG_PAGE_CONTENT = "[PAGE_CONTENT]";
        private const string TAG_TITLE = "[TITLE]";

        private const string TAG_PREVIOUS_PAGE = "[PREVIOUS_PAGE]";
        private const string TAG_NEXT_PAGE = "[NEXT_PAGE]";
        private const string TAG_INDEX_PAGE  = "[INDEX_PAGE]";
        private const string TAG_BOOK_ID = "[BOOK_ID]";
        private const string TAG_READ_ID = "[READ_ID]";

        public BiqugeDownloadImages(string indexUri, string saveToRoot, string novelTitle)
        {
            DownloadConfig.Protocol = "http";
            mIndexUri = indexUri;
            mSaveToRoot = string.Format("{0}\\{1}", saveToRoot, novelTitle);
            mSaveToImageRoot = string.Format("{0}\\Images", mSaveToRoot);

            htmlIndexTemplate = string.Format("{0}\\HTTPTemplateIndex.html", AppDomain.CurrentDomain.BaseDirectory);
            htmlPageTemplate = string.Format("{0}\\HTMLTemplatePages.html", AppDomain.CurrentDomain.BaseDirectory);
        }


        #region Helper Method For Each Process


        /*
         * <div class="divimage">
                <img src="http://img.biquge.com:8080/pic/0/201/440893/844376.gif" border="0" class="imagecontent">
           </div>
         */

        public void ProcessAll()
        {
            List<PageInfo> pageInfoList = new List<PageInfo>();
            Encoding encoding = null;
            if (ProcessToGetPageInfos(ref pageInfoList, ref encoding))
            {
                DownloadImages(pageInfoList, OverWriteImage);
                BuildIndex(pageInfoList, encoding, mNovelTitle);
                BuildPages(pageInfoList, encoding);
                
            }
        }

        private bool DownloadImages(List<PageInfo> pageInfoList, bool overwrite)
        {
            try
            {
                if (!Directory.Exists(mSaveToImageRoot))
                {
                    Directory.CreateDirectory(mSaveToImageRoot);
                }


                foreach (PageInfo pi in pageInfoList)
                {
                    foreach (string image in pi.ImageList)
                    {
                        string FileName = image.Substring(image.LastIndexOf("/") + 1,
                            (image.Length - image.LastIndexOf("/") - 1));

                        string saveToLoc = string.Format("{0}\\{1}", mSaveToImageRoot, FileName);

                        if (File.Exists(saveToLoc) && overwrite)
                        {
                            File.Delete(saveToLoc);
                        }
                        else if(File.Exists(saveToLoc) && !overwrite)
                        {
                            pi.DownloadedImageList.Add(FileName);
                            continue;
                        }
                        
                        if (FileDownloadUtil.DownloadFile(image, saveToLoc))
                        {
                            Logger.Instance.Info(string.Format("Dwonload OK: {0}",image));
                            pi.DownloadedImageList.Add(FileName);
                        }
                        else
                        {
                            Logger.Instance.Info(string.Format("Dwonload Failed: {0}", image));
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Logger.Instance.Error(ex.ToString());
                return false;
            }
        }

        private bool BuildPages(List<PageInfo> pageInfoList, Encoding encoding)
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

        private bool BuildPage(PageInfo pi, string pageContentTemplate, Encoding encoding)
        {
            try
            {
                StringBuilder indexStr = new StringBuilder();
                string tempContent = string.Empty;

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

                pageContentTemplate = pageContentTemplate.Replace(TAG_PAGE_CONTENT, tempContent);
                pageContentTemplate = pageContentTemplate.Replace(TAG_PREVIOUS_PAGE, pi.preview_page);
                pageContentTemplate = pageContentTemplate.Replace(TAG_NEXT_PAGE, pi.next_page);
                pageContentTemplate = pageContentTemplate.Replace(TAG_INDEX_PAGE, pi.index_page);
                pageContentTemplate = pageContentTemplate.Replace(TAG_BOOK_ID, pi.bookid);
                pageContentTemplate = pageContentTemplate.Replace(TAG_READ_ID, pi.readid);
                pageContentTemplate = pageContentTemplate.Replace(TAG_TITLE, pi.IndexContent.TitleContent);


                string file = string.Format("{0}\\{1}.html", mSaveToRoot, pi.readid);

                if (File.Exists(file))
                {
                    File.Delete(file);
                }

                StreamWriter SW = new StreamWriter(new FileStream(file, FileMode.CreateNew, FileAccess.Write), encoding);

                SW.Write(pageContentTemplate);
                SW.Flush();

                SW.Close();

                return true;
            }
            catch (Exception ex)
            {
                Logger.Instance.Error(ex.ToString());
                return false;
            }
        }

        private bool BuildIndex(List<PageInfo> pageInfoList, Encoding encoding, string book)
        {
            try
            {
                StringBuilder indexStr = new StringBuilder();
                foreach (PageInfo pi in pageInfoList)
                {
                    indexStr.AppendLine(string.Format(IndexContentLine, pi.readid, pi.IndexContent.Title, pi.IndexContent.TitleContent));
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

        private bool ProcessToGetPageInfos(ref List<PageInfo> pageInfoList, ref Encoding encoding)
        {
            int count = 0;

            List<IndexContent> indexList = null;

            pageInfoList = new List<PageInfo>();

            if (ProcessIndex(ref indexList, ref encoding))
            {
                if (indexList != null)
                {
                    foreach (IndexContent indexContent in indexList)
                    {
#if DEBUG
                        count++;

                        if (count >= 1000)
                        {
                            break;
                        }
#endif
                        PageInfo pi = null;
                        if (ProcessOnePage(indexContent.Url, indexContent, ref pi))
                        {
                            if (pi != null && pi.ValidPageInfor())
                            {
                                pageInfoList.Add(pi);
                            }
                        }
                    }

                    Logger.Instance.Info("Get Page Infor List OK!");
                    return true;
                }
            }

            Logger.Instance.Info("Get Page Infor List Failed!");
            return false;
        }

        private bool ProcessIndex(ref List<IndexContent> indexList, ref Encoding encoding )
        {
            string content = string.Empty;

            if (!FileDownloadUtil.DownloadWebPage(HttpMainHost, mIndexUri, ref content,ref encoding))
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

        private bool ProcessOnePage(string url, IndexContent indexContent, ref PageInfo pi)
        {
            string content = string.Empty;
            Encoding encoding = null;

            if (!FileDownloadUtil.DownloadWebPage(HttpMainHost, url, ref content, ref encoding))
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

        private List<IndexContent> GetIndex(string htmlContent)
        {
            Logger.Instance.Info("Enter GetIndex");

            List<IndexContent> indexList = new List<IndexContent>();

            htmlContent = htmlContent.ToLower();
            string pattern = @"<dd><a href=""(?<url>.*?)"" title=""(?<title>.*?)"">(?<titleContent>.*?)</a></dd>";
            
            MatchCollection collections = Regex.Matches(htmlContent, pattern);

            foreach (Match match in collections)
            {
                IndexContent content = new IndexContent();
                content.Url = match.Groups["url"].Value;
                content.Title = match.Groups["title"].Value;
                content.TitleContent = match.Groups["titleContent"].Value;
                content.LineContent = match.Value;

                indexList.Add(content);
            }

            Logger.Instance.Info("Exit GetIndex");

            return indexList;
        }

        private PageInfo GetPageInfo(string htmlContent, IndexContent indexContent)
        {
            Logger.Instance.Info("Enter GetPageInfo");

            PageInfo ps = new PageInfo();

            ps.IndexContent = indexContent;

            htmlContent = htmlContent.ToLower();
            string pattern_preview_page = @"var preview_page = ""(?<matchValue>.*?)"";";
            string pattern_next_page = @"var next_page = ""(?<matchValue>.*?)"";";
            string pattern_index_page = @"var index_page = ""(?<matchValue>.*?)"";";
            string pattern_bookid = @"var bookid = ""(?<matchValue>.*?)"";";
            string pattern_readid = @"var readid = ""(?<matchValue>.*?)"";";

            string pattern_images = string.Format(@"<div class=""divimage""><img src=""(?<imageValue>{0}.*?)"" border=""0"" class=""imagecontent""></div>", HttpImageHost);

            Match match_preview_page = Regex.Match(htmlContent, pattern_preview_page);
            ps.preview_page = match_preview_page.Groups["matchValue"].Value;

            Match match_next_page = Regex.Match(htmlContent, pattern_next_page);
            ps.next_page = match_next_page.Groups["matchValue"].Value;

            Match match_index_page = Regex.Match(htmlContent, pattern_index_page);
            ps.index_page = match_index_page.Groups["matchValue"].Value;

            Match match_bookid = Regex.Match(htmlContent, pattern_bookid);
            ps.bookid = match_bookid.Groups["matchValue"].Value;

            Match match_readid = Regex.Match(htmlContent, pattern_readid);
            ps.readid = match_readid.Groups["matchValue"].Value;

            MatchCollection collections = Regex.Matches(htmlContent, pattern_images);

            foreach (Match match in collections)
            {
                string imageValue = match.Groups["imageValue"].Value;

                if (!string.IsNullOrEmpty(imageValue))
                {
                    ps.ImageList.Add(imageValue);
                }
            }

            Logger.Instance.Info("Exit GetPageInfo");

            return ps;
        }

        #endregion

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
    }

    public class PageInfo
    {
        public string preview_page = string.Empty;
        public string next_page = string.Empty;
        public string index_page = string.Empty;
        public string bookid = string.Empty;
        public string readid = string.Empty;
        public IndexContent IndexContent = null;


        public List<string> DownloadedImageList = new List<string>();
        public List<string> ImageList = new List<string>();

        public bool ValidPageInfor()
        {
            return !string.IsNullOrEmpty(bookid) && !string.IsNullOrEmpty(readid) && IndexContent!=null;
        }
    }
}
