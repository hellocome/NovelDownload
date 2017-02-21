using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using FileDownloadLib;

namespace FileDownload
{
    public sealed class YaokanShuDownloadImagesText: BaseDownloadImagesText
    {

        public YaokanShuDownloadImagesText(string indexUri, string saveToRoot, string novelTitle, bool UpdateMode, int MaxPages)
            : base(indexUri, saveToRoot, novelTitle, UpdateMode, MaxPages, "www.1kanshu.com", string.Empty)
        {
        }


        #region download One by One

        protected override List<IndexContent> GetIndex(string htmlContent)
        {
            Logger.Instance.Info("Enter GetIndex");

            List<IndexContent> indexList = new List<IndexContent>();

            htmlContent = htmlContent.ToLower();
            htmlContent = htmlContent.Replace(System.Environment.NewLine, "");

            string[] patterns = new string[] { @"<dd>([\s])*<a href=""(?<url>((?!<a href)[\s\S])*)"">(?<titleContent>((?!<a href)[\s\S])*)</a>[\s]*<a href=""(?<url_2>((?!<a href)[\s\S])*)"">(?<titleContent_2>((?!<a href)[\s\S])*)</a>[\s]*</dd>",
                @"<dd>([\s])*<a href=""(?<url>((?!<a href)[\s\S])*)"">(?<titleContent>((?!<a href)[\s\S])*)</a>((?!<a href)[\s\S])*</dd>"};
  
            List<Match> collections = new List<Match>();

            foreach (string pattern in patterns)
            {
                MatchCollection collectionsTemp = Regex.Matches(htmlContent, pattern);

                if (collectionsTemp != null && collectionsTemp.Count > 0)
                {
                    Match[] matches = new Match[collectionsTemp.Count];
                    collectionsTemp.CopyTo(matches, 0);
                    collections.AddRange(matches);
                }
            }

            foreach (Match match in collections)
            {
                IndexContent content = new IndexContent();

                content.Url = match.Groups["url"].Value;
                content.Title = match.Groups["titleContent"].Value;
                content.TitleContent = match.Groups["titleContent"].Value;
                content.LineContent = match.Value;

                if (!string.IsNullOrEmpty(content.Url))
                {
                    string FileName = content.Url.Substring(content.Url.LastIndexOf("/") + 1,
                        (content.Url.Length - content.Url.LastIndexOf("/") - 1));

                    if (!string.IsNullOrEmpty(FileName))
                    {
                        content.FileName = FileName;
                    }
                }


                indexList.Add(content);


                IndexContent content2 = new IndexContent();

                content2.Url = match.Groups["url_2"].Value;
                content2.Title = match.Groups["titleContent_2"].Value;
                content2.TitleContent = match.Groups["titleContent_2"].Value;
                content2.LineContent = match.Value;

                if (!string.IsNullOrEmpty(content2.Url))
                {
                    string FileName = content2.Url.Substring(content2.Url.LastIndexOf("/") + 1,
                        (content2.Url.Length - content2.Url.LastIndexOf("/") - 1));

                    if (!string.IsNullOrEmpty(FileName))
                    {
                        content2.FileName = FileName;
                    }

                    indexList.Add(content2);
                }  
            }

            Logger.Instance.Info("Exit GetIndex");

            indexList.Sort(CompareIndexContent);

            return indexList;
        }

        private static int CompareIndexContent(IndexContent ic1, IndexContent ic2)
        {
            return string.Compare(ic1.FileName, ic2.FileName);
        }

        protected override PageInfo GetPageInfo(string htmlContent, IndexContent indexContent)
        {
            Logger.Instance.Info("Enter GetPageInfo");

            PageInfo ps = new PageInfo();

            ps.IndexContent = indexContent;

            htmlContent = htmlContent.ToLower();
            string pattern_preview_page = @"var preview_page = ""(?<matchValue>.*?)"";";
            string pattern_next_page = @"var next_page = ""(?<matchValue>.*?)"";";
            string pattern_index_page = @"var index_page = ""(?<matchValue>.*?)"";";
            string pattern_bookid = @"var article_id = ""(?<matchValue>.*?)"";";
            string pattern_readid = @"var chapter_id = ""(?<matchValue>.*?)"";";

            string pattern_images = string.Format(@"<div class=""divimage""><img src=""(?<imageValue>{0}.*?)"" border=""0"" class=""imagecontent""></div>", HttpImageHost);
            //string pattern_text = "<div id=\"content\" name=\"content\">(?<textValue>((?!</div>)[\\s\\S])*)</div>";
            string pattern_text = @"<div id=""content"">[\s]*<div id=""text_area"">(?<textValue>((?!</div>)[\s\S])*)</div>";

            string pattern_Script = "<script(?<scriptValue>[\\s\\S]*)</script>";


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

            MatchCollection collections = Regex.Matches(htmlContent, pattern_images, RegexOptions.IgnoreCase);

            if (collections.Count > 0)
            {
                ps.ContentType = ContentTypeEnum.Image;

                foreach (Match match in collections)
                {
                    string imageValue = match.Groups["imageValue"].Value;

                    if (!string.IsNullOrEmpty(imageValue))
                    {
                        ps.ImageList.Add(imageValue);
                    }
                }
            }
            else
            {
                Match match = Regex.Match(htmlContent, pattern_text);
                string textValue = match.Groups["textValue"].Value;

                if (!string.IsNullOrEmpty(textValue))
                {
                    match = Regex.Match(textValue, pattern_Script);
                    string scriptValue = match.Groups["scriptValue"].Value;

                    scriptValue = "<script" + scriptValue + "</script>";

                    if (!string.IsNullOrEmpty(scriptValue))
                    {
                        textValue = textValue.Replace(scriptValue, "");
                    }

                    textValue = RemoveInvalidString(textValue);
                }

                ps.ContentType = ContentTypeEnum.Text;
                ps.TextContent = textValue;
            }

            Logger.Instance.Info("Exit GetPageInfo");

            return ps;
        }

        #endregion
    }
}
