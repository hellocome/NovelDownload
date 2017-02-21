 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace BuildWebSite
{
    public class BuildWebSiteIndex
    {
        const string TEMP = @"<style type=""text/css"">
                    html,body,ul,ol,li,p,h1,h2,h3,h4,h5,h6,table,td,th,img,div,dl,dt,dd,input,select,form,fieldset {
	                    margin:auto;
	                    padding:0;
                    }
                    img {
	                    border:0;
                    }
                    ul li {
	                    list-style:none;
                    }
                    a {
	                    color:#6F78A7;
	                    text-decoration:none;
                    }
                    a:hover {
	                    text-decoration:underline;
                    }
                    .clear {
	                    clear:both;
	                    font-size:0;
	                    line-height:0;
	                    height:0;
	                    padding:0;
	                    margin:auto;
                    }
                    body {
	                    font-size:13px;
	                    color:#555555;
	                    font-family:""Microsoft YaHei"",""?￠èí??oú"",""??ì?""; background-color:#E9FAFF;
                    }
                    .header {
	                    width:980px;
	                    margin:auto;
	                    height:61px;
                    }
                    .header_logo a {
	                    background:url(/images/logo.gif) no-repeat scroll;
	                    display:block;
	                    width:250px;
	                    height:60px;
	                    float:left;
	                    text-indent:-9999px;
                    }
                    .header_search {
	                    float:left;
	                    margin:20px auto auto 30px;
	                    overflow:hidden;
	                    width:450px;
                    }
                    .header_search input.search {
	                    width:320px;
	                    height:24px;
	                    padding:3px;
	                    margin-right:5px;
	                    border:1px solid #A6D3E8;
	                    font:16px/22px arial;
                    }
                    .header_search button {
	                    background:#DDDDDD;
	                    cursor:pointer;
	                    font-size:14px;
	                    height:32px;
	                    width:95px;
                    }
                    #auto_div {
	                    position:absolute;
	                    background-color:white;
	                    padding:0px;
                    }
                    .autoinput {
	                    padding-left:4px;
	                    padding-right:0px;
	                    padding-top:3px;
                    }
                    .sug1 {
	                    padding-top:3px;
	                    padding-bottom:3px;
	                    font-size:10pt;
	                    line-height:18px;
                    }
                    .addborder {
	                    border:1px solid #8c8c8c;
                    }
                    .userpanel {
	                    width:220px;
	                    float:right;
	                    color:#9E9E9E;
	                    padding:5px 3px;
	                    margin-top:3px;
	                    margin-right:5px;
	                    border:1px dotted #88C6E5;
	                    text-align:center;
                    }
                    .userpanel p {
	                    width:220px;
	                    float:left;
	                    text-align:center;
	                    height:26px;
                    }
                    .userpanel a {
	                    line-height:200%;
	                    color:#9E9E9E;
                    }
                    .nav {
	                    margin:auto;
	                    width:980px;
	                    margin-top:10px;
	                    height:40px;
	                    overflow:hidden;
	                    background:#88C6E5;
                    }
                    .nav ul li {
	                    float:left;
	                    line-height:44px;
                    }
                    .nav ul li a {
	                    padding:0 17px;
	                    color:#FFF;
	                    font-weight:bold;
	                    font-size:15px;
                    }
                    .ywtop {
	                    background-color:#E1ECED;
	                    border-bottom:1px solid #A6D3E8;
	                    color:#808080;
	                    height:35px;
	                    min-width:950px;
	                    width:100%;
                    }
                    .ywtop a {
	                    color:#808080;
                    }
                    .ywtop_con {
	                    line-height:35px;
	                    margin:0 auto;
	                    text-indent:16px;
	                    vertical-align:middle;
	                    width:950px;
                    }
                    .ywtop_sethome {
	                    background:url(""/images/sethome.gif"") no-repeat scroll 0 10px transparent;
	                    display:inline;
	                    float:left;
	                    margin-right:20px;
                    }
                    .ywtop_addfavorite {
	                    background:url(""/images/addfavorites.gif"") no-repeat scroll 0 10px transparent;
	                    display:inline;
	                    float:left;
                    }
                    .ywtop_input {
	                    float:right;
                    }
                    .navt {
	                    height:28px;
	                    overflow:hidden;
	                    padding:7px 0 0;
                    }
                    .navt .nle {
	                    float:left;
	                    overflow:hidden;
	                    width:250px;
                    }
                    .nle .sy {
	                    float:left;
	                    width:120px;
                    }
                    .navt .nri {
	                    float:right;
	                    overflow:hidden;
                    }
                    .nri .cc {
	                    float:left;
	                    overflow:hidden;
	                    width:155px;
                    }
                    .cc .txt {
	                    color:#808080;
	                    float:left;
	                    text-align:right;
                    }
                    .cc .inp {
	                    float:left;
	                    padding-top:7px;
	                    width:90px;
                    }
                    .inp input {
	                    float:left;
	                    width:87px;
	                    background-color:#FFFFFF;
	                    border:1px solid #A6D3E8;
	                    height:18px;
	                    margin:1px 0;
                    }
                    .nri {
	                    float:right;
	                    font-size:13px;
                    }
                    .nri .frii {
	                    float:left;
	                    font-size:14px;
	                    margin-left:5px;
	                    padding-top:8px;
	                    width:55px;
                    }
                    .frii .int {
	                    background:url(""/images/login_oa_bar.gif"") no-repeat scroll 0 0 transparent;
	                    border:medium none;
	                    color:#2A4E8A;
	                    float:left;
	                    font-size:12px;
	                    height:21px;
	                    line-height:21px;
	                    text-align:center;
	                    width:51px;
                    }
                    .nri .ccc {
	                    float:left;
	                    overflow:hidden;
	                    padding-left:10px;

                    }
                    .ccc .txtt {
	                    color:#808080;
	                    float:left;
	                    padding-left:10px;
                    }
                    .txtt a,.txtt a:hover {
	                    color:#808080;
	                    text-decoration:none;
                    }
                    .MessageDiv {
                        background: #FFF9D9;
                        border: 1px solid #FFCC33;
                        line-height: 150%;
                        width:800px;
                        margin: 10px auto auto;
                        padding: 10px;
                        text-align:center;}
                    .footer {
	                    margin:auto; 
	                    margin-top:10px;
	                    overflow:hidden;
	                    width:980px;
	                    text-align:center;
                    }
                    #footer {
	                    margin:auto; 
	                    margin-top:10px;
	                    overflow:hidden;
	                    width:980px;
	                    text-align:center;
                    }
                    .footer_link {
	                    width:92%;
	                    border-bottom:2px solid #88C6E5; 
	                    height:25px;
	                    line-height:25px;
	                    overflow:hidden;
	                    margin:5px auto;
                    }
                    .footer_cont p{
	                    line-height:20px;
	                    width:88%;
	                    color:#b2b2b2;
                    }

                    .box_con {
	                    border:2px solid #88C6E5;
	                    margin:10px auto;
	                    line-height:100%;
	                    width:976px;
	                    overflow:hidden;
                    }
                    .con_top {
	                    background-color:#E1ECED;
	                    height:40px;
	                    line-height:40px;
	                    border-bottom:1px solid #88C6E5;
	                    width:100%;
	                    padding-left:10px;
                    }
                    .con_top #bdshare {
	                    float:right;
	                    text-align:right;
	                    height:20px;
	                    line-height:20px;
	                    padding-right:20px;
	                    padding-top:9px;
                    }
                    #sidebar {
	                    float:right;
	                    width:264px;
	                    _width:270px;
	                    border-left:1px dashed #88C6E5;
	                    text-align:left;
                    }
                    #sidebar .xian {
	                    height:1px;
	                    border-top:1px dashed #88C6E5;
	                    overflow:hidden;
	                    width:100%;
                    }
                    .sidebartitle {
	                    font-weight:bold;
	                    line-height:150%;
	                    font-size:11pt;
	                    padding-left:10px;
	                    padding-top:2px;
	                    width:100%;
	                    white-space:nowrap;
	                    text-overflow:ellipsis;
	                    -o-text-overflow:ellipsis;
	                    overflow:hidden;
                    }
                    .sidebarlist {
	                    line-height:100%;
	                    padding-left:20px;
	                    margin-bottom:5px;
	                    overflow:hidden;
	                    _height:187px;
                    }
                    .sidebarlist a {
	                    float:left;
	                    line-height:200%;
	                    width:49.5%;
	                    _width:49%;
	                    white-space:nowrap;
	                    text-overflow:ellipsis;
	                    -o-text-overflow:ellipsis;
	                    overflow:hidden;
                    }
                    #maininfo {
	                    float:left;
	                    width:700px;
	                    _width:670px;
	                    max-height:430px;
                    }
                    #fmimg {
	                    background-color:#E1ECED;
	                    float:left;
	                    padding:12px;
	                    margin:12px;
	                    width:126px;
                    }
                    #fmimg img {
	                    height:150px;
	                    margin:3px;
	                    width:120px;
	                    border:none;
                    }
                    #info {
	                    padding:0 10px;
	                    margin:10px;
	                    font-size:15px;
	                    height:203px;
	                    _height:190px;
	                    max-height:195px;
                    }
                    #info h1 {
	                    font-size:28px;
	                    _font-size:22px;
	                    font-family:""oúì?"";
	                    font-weight:bold;
	                    height:44px;
	                    _height:22px;
	                    line-height:44px;
	                    _line-height:22px;
	                    padding:1px;
	                    margin:auto;
	                    overflow:hidden;
                    }
                    #info p {
	                    height:25px;
	                    line-height:25px;
	                    padding-top:2px;
	                    float:left;
	                    width:440px;
	                    margin:auto;
	                    overflow:hidden;
                    }
                    #intro {
	                    width:100%;
	                    overflow:hidden;
	                    line-height:200%;
	                    border-top:1px dashed #88C6E5;
	                    padding:10px;
	                    font-size:13px;
	                    _height:190px;
                    }
                    #list {
	                    padding:2px;
                    }
                    #list dl {
	                    float:left;
	                    margin:auto;
	                    overflow:hidden;
	                    padding-bottom:1px;
                    }
                    #list dt {
	                    background:#C3DFEA;
	                    display:inline;
	                    line-height:28px;
	                    float:left;
	                    margin:auto auto 5px auto;
	                    width:98%;
	                    *width:97.5%;
	                    font-size:14px;
	                    padding:5px 10px;
	                    overflow:hidden;
	                    text-align:center;
	                    vertical-align:middle;
                    }
                    #list dd {
	                    border-bottom:1px dashed #CCCCCC;
	                    display:inline;
	                    float:left;
	                    height:25px;
	                    line-height:200%;
	                    margin-bottom:5px;
	                    overflow:hidden;
	                    text-align:left;
	                    text-indent:10px;
	                    vertical-align:middle;
	                    width:33%;
                    }
                    #list dd A:link {
	                    color:#444444;
	                    TEXT-DECORATION:none;
                    }
                    #list dd A:visited {
	                    COLOR:#178102;
	                    TEXT-DECORATION:none;
	                    text-decoration:underline;
                    }
                    </style>



                    <html xmlns=""http://www.w3.org/1999/xhtml"">
                    <meta http-equiv=""Content-Type"" content=""text/html; charset=gbk"" />

                    <head>
                    <title>Download Content</title>
                    </head>
                    <body>

                    <div class=""box_con"">
                    <div id=""list"">
                    <dl>
                    <dt>Novels</dt>
                        [CONTENT_NOVEL]
                    </dl>

                    </div></div>
            
            
                    <div class=""box_con"">
                    <div id=""list"">
                    <dt>Download Content
                    </dt>
                        [CONTENT_ZIP]
                    </dl>
                    </div></div></body>";

        public string[] DefaultPages = new string[] { "index.html", "index.htm", "default.html", "default.htm", "index.asp", "index.aspx" };

        public bool Process()
        {
            bool resFirst = true;
            bool resSecond = true;

            resFirst = Process(AppDomain.CurrentDomain.BaseDirectory, "Downloads/", "index_web.htm");

            resSecond = Process(AppDomain.CurrentDomain.BaseDirectory, "", "index.htm");

            return resFirst & resSecond;
        }

        public bool ProcessWeb()
        {
            return Process(AppDomain.CurrentDomain.BaseDirectory, "Downloads/", "index.htm");
        }

        public bool Process(string currDir, string SubDirPath, string tempFileName)
        {
            try
            {
                Console.WriteLine(string.Format("{0} Start", DateTime.Now.ToString("HH:MM:ss:fff")));

                string tempFile = currDir + System.IO.Path.DirectorySeparatorChar + tempFileName;

                if (File.Exists(tempFile))
                {
                    File.Delete(tempFile);
                }

                string contentNovel = string.Empty;
                string contentZIP = string.Empty;
                string content = string.Empty;

                string scanDir = currDir + System.IO.Path.DirectorySeparatorChar + SubDirPath;

                string[] files = Directory.GetFiles(scanDir);
                string[] dirs = Directory.GetDirectories(scanDir);

                List<KeyValuePair<DirectoryInfo, FileInfo>> diList = new List<KeyValuePair<DirectoryInfo, FileInfo>>();

                if (files != null)
                {
                    foreach (string dir in dirs)
                    {
                        foreach (string defaultFile in DefaultPages)
                        {
                            string indexFile = dir + System.IO.Path.DirectorySeparatorChar + defaultFile;

                            FileInfo fi = new FileInfo(indexFile);

                            if (fi.Exists)
                            {
                                DirectoryInfo DI = new DirectoryInfo(dir);

                                diList.Add(new KeyValuePair<DirectoryInfo, FileInfo>(DI, fi));
                            }
                        }
                    }

                    diList.Sort(new Comparison<KeyValuePair<DirectoryInfo, FileInfo>>(Comparison));

                    foreach (KeyValuePair<DirectoryInfo, FileInfo> df in diList)
                    {
                        contentNovel += string.Format(@"<dd><a href=""{0}{1}/{2}"">{3}</a></dd>", SubDirPath, df.Key.Name, df.Value.Name, df.Key.Name);
                        Console.WriteLine(string.Format("{0} {1}", DateTime.Now.ToString("HH:MM:ss:fff"), df.Value.Name));
                    }

                    foreach (string file in files)
                    {
                        FileInfo fi = new FileInfo(file);

                        contentZIP += string.Format(@"<dd><a href=""{0}{1}"">{2}</a></dd>", SubDirPath, fi.Name, fi.Name);
                        Console.WriteLine(string.Format("{0} {1}", DateTime.Now.ToString("HH:MM:ss:fff"), fi.Name));
                    }

                    content = TEMP.Replace("[CONTENT_NOVEL]", contentNovel);
                    content = content.Replace("[CONTENT_ZIP]", contentZIP);
                }

                Console.WriteLine(string.Format("{0} Writting", DateTime.Now.ToString("HH:MM:ss:fff")));
                StreamWriter sw = new StreamWriter(new FileStream(tempFile, FileMode.CreateNew, FileAccess.Write), Encoding.GetEncoding("gbk"));
                sw.Write(content);
                sw.Flush();
                sw.Close();

                Console.WriteLine(string.Format("{0} Done", DateTime.Now.ToString("HH:MM:ss:fff")));
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("{0} Error", DateTime.Now.ToString("HH:MM:ss:fff")));
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        public static int Comparison(KeyValuePair<DirectoryInfo, FileInfo> x, KeyValuePair<DirectoryInfo, FileInfo> y)
        {
            return x.Key.LastWriteTime.CompareTo(y.Key.LastWriteTime);
        }

    }
}
