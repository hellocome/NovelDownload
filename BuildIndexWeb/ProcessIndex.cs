using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace BuildIndexWeb
{
    public class ProcessIndex
    {
        private string mLog = string.Empty;

        public string[] DefaultPages = new string[] { "index.html", "index.htm", "default.html", "default.htm", "index.asp", "index.aspx" };

        public string Log
        {
            get
            {
                return mLog;
            }
        }

        void WriteLine(string line)
        {
            mLog += line + "\r\t";
        }

        public bool Process(string currDir, string SubDirPath, MsSqlFprBuildIndexWeb MSSQL)
        {
            try
            {
                mLog = string.Empty;

                WriteLine(string.Format("{0} Start", DateTime.Now.ToString("HH:MM:ss:fff")));

                string contentNovel = string.Empty;

                string scanDir = currDir + System.IO.Path.DirectorySeparatorChar + SubDirPath;

                string[] files = Directory.GetFiles(scanDir);
                string[] dirs = Directory.GetDirectories(scanDir);

                int count = 0;


                contentNovel += "<Table class=\"list\">";

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
                                if (count % 4 == 0)
                                {
                                    contentNovel += "<tr>";
                                }
                                count++;

                                contentNovel += string.Format(@"<td width=""25%""><a href=""{0}{1}/{2}"">{3}</a></td>", SubDirPath, DI.Name, fi.Name, DI.Name);
                                WriteLine(string.Format("{0} {1}", DateTime.Now.ToString("HH:MM:ss:fff"), fi.Name));

                                if (count % 4 == 0)
                                {
                                    contentNovel += "</tr>";
                                }
                            }
                        }
                    }
                }

                if (count == 0)
                {
                    contentNovel += string.Format(@"<tr><td width=""100%"">NO CONTENT IN THIS FILE</td></tr>");
                }
                else
                {
                    while (count % 3 != 0)
                    {
                        contentNovel += string.Format(@"<td width=""25%""></td>");
                        count++;
                    }

                    contentNovel += "</tr>";
                }

                contentNovel += "</Table>";

                WriteLine(string.Format("{0} Writting", DateTime.Now.ToString("HH:MM:ss:fff")));
                WriteLine("In: "+ contentNovel);

                if (!MSSQL.SetIndexContent(contentNovel))
                {
                    WriteLine("Log: " + MSSQL.Log);
                }

                string getContent = string.Empty;
                if (MSSQL.GetIndexContent(out getContent))
                {
                    WriteLine("Out: " + getContent);
                }

                WriteLine("Log: " + MSSQL.Log);

                WriteLine(string.Format("{0} Done", DateTime.Now.ToString("HH:MM:ss:fff")));
                return true;
            }
            catch (Exception ex)
            {
                WriteLine(string.Format("{0} Error", DateTime.Now.ToString("HH:MM:ss:fff")));
                WriteLine(ex.ToString());
                return false;
            }
        }
    }
}