using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using FileDownloadLib;

namespace FileDownload
{
    public sealed class AoyeCCDownloadImagesText : BaseDownloadImagesText
    {
        public AoyeCCDownloadImagesText(string indexUri, string saveToRoot, string novelTitle, bool UpdateMode, int MaxPages)
            : base(indexUri, saveToRoot, novelTitle, UpdateMode, MaxPages, "www.aoye.cc", string.Empty)
        {
        }

        #region download One by One
        //** Need to Change ****
        protected override List<IndexContent> GetIndex(string htmlContent)
        {
            Logger.Instance.Info("Enter GetIndex");

            List<IndexContent> indexList = new List<IndexContent>();

            htmlContent = htmlContent.ToLower();
            htmlContent = htmlContent.Replace(System.Environment.NewLine, "");
 
            //string pattern = @"<dd><a href=""(?<url>.*?)"">(?<titleContent>.*?)</a></dd>";
            string [] patterns = new string[]{@"<td class=""ccss"">[\s]*<a href=""(?<url>.*?)"">(?<titleContent>.*?)</a>[\s]*</td>",
                @"<td class=""ccss""><div class=""dccss""><a href=""(?<url>.*?)"" alt=""(?<titleVaue>.*?)"">(?<titleContent>.*?)</a></div></td>",
                @"<li><a href=""(?<url>.*?)"" title=""(?<titleVaue>.*?)"">(?<titleContent>.*?)</a></li>",
            };


            MatchCollection collections = null;

            int index = htmlContent.IndexOf("<div id=\"detaillist\">");

            if (index > 0)
            {
                htmlContent = htmlContent.Substring(index);
            }

            foreach (string pattern in patterns)
            {
                collections = Regex.Matches(htmlContent, pattern);

                if (collections != null && collections.Count > 0)
                {
                    break;
                }
            }

            foreach (Match match in collections)
            {
                IndexContent content = new IndexContent();
                content.Url = match.Groups["url"].Value;
                content.Title = match.Groups["titleVaue"].Value;
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
            }

            Logger.Instance.Info("Exit GetIndex");

            return indexList;
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

            string pattern_text = "<div id=\"txtright\"><script type=\"text/javascript\">txtrightshow\\(\\);</script></div>(?<textValue>((?!</div>)(?!</pre>)[\\s\\S])*)</pre>";
            string[] pattern_Scripts = new string[] { "\\[<a id=(?<scriptValue>((?!</a>)[\\s\\S])*)</a>\\]",
            "<a id=(?<scriptValue>((?!</a>)[\\s\\S])*)</a>"};


            Match match_preview_page = Regex.Match(htmlContent, pattern_preview_page);
            ps.preview_page = GetFileNameFromUrl(match_preview_page.Groups["matchValue"].Value);

            Match match_next_page = Regex.Match(htmlContent, pattern_next_page);
            ps.next_page = GetFileNameFromUrl(match_next_page.Groups["matchValue"].Value);

            Match match_index_page = Regex.Match(htmlContent, pattern_index_page);
            ps.index_page = "index.html";

            Match match_bookid = Regex.Match(htmlContent, pattern_bookid);
            ps.bookid = match_bookid.Groups["matchValue"].Value;

            Match match_readid = Regex.Match(htmlContent, pattern_readid);
            ps.readid = match_readid.Groups["matchValue"].Value;

            MatchCollection collections = Regex.Matches(htmlContent, pattern_images);

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
                htmlContent = htmlContent.Replace(System.Environment.NewLine, "<br/>");

                Match match = Regex.Match(htmlContent, pattern_text);
                string textValue = match.Groups["textValue"].Value;

                if (!string.IsNullOrEmpty(textValue))
                {
                    foreach (string pattern_Script in pattern_Scripts)
                    {
                        textValue = Regex.Replace(textValue, pattern_Script, "");
                    }

                    textValue = RemoveInvalidString(textValue);
                }

                ps.ContentType = ContentTypeEnum.Text;
                ps.TextContent = textValue;
            }

            Logger.Instance.Info("Exit GetPageInfo");

            return ps;
        }


        public string GetFileNameFromUrl(string url)
        {
            int index = url.LastIndexOf("/");

            string fileName = url;

            if (index > 0)
            {
                if (index + 1 < url.Length)
                {
                    fileName = url.Substring(index + 1);
                }
                else
                {
                    fileName = "index.html";
                }
            }

            return fileName;
        }
        
        #endregion
    }
}
