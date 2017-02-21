using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using FileDownloadLib;

namespace FileDownload
{
    public sealed class BiqugeCODownloadImagesText: BaseDownloadImagesText
    {

        public BiqugeCODownloadImagesText(string indexUri, string saveToRoot, string novelTitle, bool UpdateMode, int MaxPages)
            : base(indexUri, saveToRoot, novelTitle, UpdateMode, MaxPages, "www.biquge.co", string.Empty)
        {
        }


        #region download One by One

        protected override List<IndexContent> GetIndex(string htmlContent)
        {
            Logger.Instance.Info("Enter GetIndex");

            List<IndexContent> indexList = new List<IndexContent>();

            htmlContent = htmlContent.ToLower();
            htmlContent = htmlContent.Replace(System.Environment.NewLine, "");
            string pattern = @"<dd><a href=""(?<url>.*?)"">(?<titleContent>.*?)</a></dd>";
            MatchCollection collections = Regex.Matches(htmlContent, pattern);

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

            string pattern_text = "<!--章节内容开始-->(?<textValue>([\\s\\S])*)<!--章节内容结束-->";

            string pattern_Script = "<script>(?<scriptValue>[\\s\\S]*)</script>";


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
