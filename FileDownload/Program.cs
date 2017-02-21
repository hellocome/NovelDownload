using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Text.RegularExpressions;
using FileDownloadLib;

namespace FileDownload
{
    public class Program
    {
        static void Main(string[] args)
        {
            try
            {
                ConsoleLogger.Instance.Info("Init Console Log");
                Logger.Instance.Info("Init Log");
                Logger.Instance.Info("Start Processing");

                if (ParseArgs(args))
                {
                    Logger.Instance.Info("************************  Parameters  ****************************");
                    Logger.Instance.Info(string.Format("Host: {0}", Host));
                    Logger.Instance.Info(string.Format("Url: {0}", Url));
                    Logger.Instance.Info(string.Format("SaveTo: {0}", SaveTo));
                    Logger.Instance.Info(string.Format("NovelName: {0}", NovelName));
                    Logger.Instance.Info(string.Format("MaxPages: {0}", MaxPages));
                    Logger.Instance.Info(string.Format("UpdateMode: {0}", UpdateMode));
                    Logger.Instance.Info("******************************************************************");

                    string outNovelBaseDir = string.Empty;
                    IDownloadImagesText download = null;

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
                        
                  

                    if (download != null)
                    {
                        if (StartFromIndex > 0)
                        {
                            Logger.Instance.Info("Start From Index: " + StartFromIndex);
                            download.Argument.StartFromIndex = StartFromIndex;
                        }

                        string argumentsLine = string.Empty;
                        foreach (string argument in args)
                        {
                            if (argument.Equals("s", StringComparison.OrdinalIgnoreCase))
                            {
                                argumentsLine += argument + " ";
                            }
                        }

                        if (!string.IsNullOrEmpty(argumentsLine) && !selfExec)
                        {
                            download.SaveLastWorkingArguments(argumentsLine);
                        }

                        if (!selfExec)
                        {
                            download.CopyFiles();
                            download.SaveToConfig();
                        }

                        download.ProcessAllOneByOne();

                        Logger.Instance.Info("Finish Processing");
                        Console.Read();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }


        static bool UpdateMode = true;
        static string  Host = string.Empty;
        static string Url = string.Empty;
        static string SaveTo = string.Empty;
        static string NovelName = string.Empty;
        static int MaxPages = 1000;
        static int StartFromIndex = 0;

        static bool selfExec = false;

        static bool ParseArgs(string[] args)
        {
            string argStr = string.Empty;
            foreach (string arg in args)
            {
                argStr += arg + " ";
            }

            Logger.Instance.Info(argStr);

            string newArgsStr = string.Empty;
            if ((args == null || args.Length == 0 )&& !GetValidArgsConfig(out newArgsStr))
            {
                Logger.Instance.Info("Using Arguments Is NULL");

                ShowHelper();

                return false;
            }
            else if ((args == null || args.Length == 0) && GetValidArgsConfig(out newArgsStr))
            {
                Logger.Instance.Info("Using Arguments From Config File");
                Logger.Instance.Info(newArgsStr);

                string [] newArgs = newArgsStr.Split(new char[] { ' ' });

                if (newArgs != null)
                {
                    selfExec = true;
                    return ParseArgs(newArgs);
                }

                ShowHelper();

                return false;
            }
            else
            {

                Arguments arguments = new Arguments(args, true);

                List<string> validParameters = new List<string>();
                validParameters.Add("h");
                validParameters.Add("s");
                validParameters.Add("n");
                validParameters.Add("u");
                validParameters.Add("mp");
                validParameters.Add("new");
                validParameters.Add("help");
                validParameters.Add("si");

                #region Parse
                if (arguments.ContainsInvalidParameters(validParameters))
                {
                    Logger.Instance.Error("Invalid paramters found");
                    ShowHelper();

                    return false;
                }
                else
                {
                    try
                    {
                        bool help = !string.IsNullOrEmpty(arguments["help"]);
                        if (help)
                        {
                            ShowHelper();

                            return false;
                        }


                        string argH = arguments["h"];
                        if (!string.IsNullOrEmpty(argH))
                        {
                            Host = argH;
                        }
                        else
                        {
                            Logger.Instance.Error("-h is empty");
                            ShowHelper();

                            return false;
                        }

                        string argU = arguments["u"];
                        if (!string.IsNullOrEmpty(argU))
                        {
                            Url = argU;
                        }
                        else
                        {
                            Logger.Instance.Error("-u is empty");
                            ShowHelper();

                            return false;
                        }

                        string argSI = arguments["si"];
                        int startTemp = 0;
                        if (!string.IsNullOrEmpty(argSI) && int.TryParse(argSI, out startTemp))
                        {
                            StartFromIndex = startTemp;
                        }



                        string argS = arguments["s"];
                        if (!string.IsNullOrEmpty(argS))
                        {
                            SaveTo = argS;
                        }
                        else
                        {
                            SaveTo = AppDomain.CurrentDomain.BaseDirectory;

                            if (selfExec)
                            {
                                SaveTo = string.Format("{0}\\..", SaveTo);
                            }
                        }

                        if (System.IO.Directory.Exists(SaveTo))
                        {
                            System.IO.Directory.CreateDirectory(SaveTo);
                        }

                        bool GetMPOK = false;
                        string argMP = arguments["mp"];
                        if (!string.IsNullOrEmpty(argMP))
                        {
                            int k = 0;

                            if (int.TryParse(argMP, out k))
                            {
                                if (k > 0)
                                {
                                    MaxPages = k;
                                    GetMPOK = true;
                                }
                            }
                        }
                        else
                        {
                            GetMPOK = true;
                        }

                        if (!GetMPOK)
                        {
                            Logger.Instance.Error("-mp is invalid");
                            ShowHelper();
                            return false;
                        }

                        string argN = arguments["n"];
                        if (!string.IsNullOrEmpty(argN))
                        {
                            NovelName = argN;
                        }
                        else
                        {
                            NovelName = string.Format("Download_{0}", DateTime.Now.ToString("yyyymmdd_hhMMssfff"));
                        }


                        string argnew = arguments["new"];
                        if (!string.IsNullOrEmpty(argnew))
                        {
                            UpdateMode = false;
                        }
                        else
                        {
                            UpdateMode = true;
                        }


                        return true;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        Logger.Instance.Error(ex.ToString());
                        ShowHelper();
                        return false;
                    }
                }

                #endregion
            }
        }

        static bool ConfigExists(out string configPath)
        {
            configPath = string.Format("{0}\\config.xml", AppDomain.CurrentDomain.BaseDirectory);

            Logger.Instance.Info("Try to load: " + configPath);

            if (File.Exists(configPath))
            {
                Logger.Instance.Info("Find Config File");
                return true;
            }

            Logger.Instance.Info("Can't Find Config File");
            return false;
        }

        static bool GetValidArgsConfig(out string arguments)
        {
            arguments = string.Empty;
            try
            {
                string configPath = string.Empty;
                if (ConfigExists(out configPath))
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(configPath);

                    XmlNodeList nodeList = xmlDoc.SelectNodes("NovelConfig/config");

                    if (nodeList != null)
                    {
                        foreach (XmlNode node in nodeList)
                        {
                            XmlElement ele = node as XmlElement;

                            if (ele.GetAttribute("name").Equals("LastValidArgument", StringComparison.OrdinalIgnoreCase))
                            {
                                arguments = ele.GetAttribute("value");
                                return true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Error(ex.ToString());
            }

            return false;
        }

        static void ShowHelper()
        {
            Console.WriteLine("*********************************************************************************************************");
            Console.WriteLine("-h     [host name]      Required     - Example: -h \"biquge\"");
            Console.WriteLine("-u     [novel url]      Required     - Example: -u \"0_201\"");
            Console.WriteLine("-mp    [Max Pages]      Optional     - Example: -mp 123 - Default is 100, it need to be greater than 0");
            Console.WriteLine("-s     [save to Folder] Optional     - Example: -s \"c:\\test\" - Default is application folder");
            Console.WriteLine("-n     [novel name]     Optional     - Example: -n \"mohen\"   - Default is Download_DateTimeNow");
            Console.WriteLine("-new   [new download]   Optional     - Example: -new           - Deffault is in UpdateMode");
            Console.WriteLine("-help  [Show help]      Optional     - Example: -help          ");
            Console.WriteLine("*********************************************************************************************************");
        }
    }

    public class Arguments
    {
        // Variables
        private StringDictionary Parameters;
        private bool IgnoreCase;

        // Constructor
        public Arguments(string[] Args, bool ingoreCase)
        {
            IgnoreCase = ingoreCase;

            Parameters = new StringDictionary();
            Regex Spliter = new Regex(@"^-{1,2}",
                RegexOptions.IgnoreCase | RegexOptions.Compiled);

            Regex Remover = new Regex(@"^['""]?(.*?)['""]?$",
                RegexOptions.IgnoreCase | RegexOptions.Compiled);

            string Parameter = null;
            string[] Parts;

            // Valid parameters forms:
            // {-,/,--}param{ ,=,:}((",')value(",'))
            // Examples: 
            // -param1 value1 --param2 /param3:"Test-:-work" 
            //   /param4=happy -param5 '--=nice=--'
            foreach (string Txt in Args)
            {
                // Look for new parameters (-,/ or --) and a
                // possible enclosed value (=,:)
                Parts = Spliter.Split(Txt, 3);

                switch (Parts.Length)
                {
                    // Found a value (for the last parameter 
                    // found (space separator))
                    case 1:
                        if (Parameter != null)
                        {
                            if (!Parameters.ContainsKey(Parameter))
                            {
                                Parts[0] =
                                    Remover.Replace(Parts[0], "$1");

                                Parameters.Add(Parameter, Parts[0]);
                            }
                            Parameter = null;
                        }
                        // else Error: no parameter waiting for a value (skipped)
                        break;

                    // Found just a parameter
                    case 2:
                        // The last parameter is still waiting. 
                        // With no value, set it to true.
                        if (Parameter != null)
                        {
                            if (!Parameters.ContainsKey(Parameter))
                                Parameters.Add(Parameter, "true");
                        }
                        Parameter = Parts[1];
                        break;

                    // Parameter with enclosed value
                    case 3:
                        // The last parameter is still waiting. 
                        // With no value, set it to true.
                        if (Parameter != null)
                        {
                            if (!Parameters.ContainsKey(Parameter))
                                Parameters.Add(Parameter, "true");
                        }

                        Parameter = Parts[1];

                        // Remove possible enclosing characters (",')
                        if (!Parameters.ContainsKey(Parameter))
                        {
                            Parts[2] = Remover.Replace(Parts[2], "$1");
                            Parameters.Add(Parameter, Parts[2]);
                        }

                        Parameter = null;
                        break;
                }
            }
            // In case a parameter is still waiting
            if (Parameter != null)
            {
                if (!Parameters.ContainsKey(Parameter))
                    Parameters.Add(Parameter, "true");
            }
        }

        // Retrieve a parameter value if it exists 
        // (overriding C# indexer property)
        public string this[string Param]
        {
            get
            {
                return (Parameters[Param]);
            }

            set
            {
                if (Contains(Param))
                {
                    Parameters[Param] = value;
                }
                else
                {
                    Parameters.Add(Param, value);
                }
            }
        }

        public bool Contains(string Param)
        {
            if (Parameters.ContainsKey(Param))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool ContainsInvalidParameters(List<string> validParameters)
        {
            List<string> validParametersTemp = null;

            if (IgnoreCase)
            {
                validParametersTemp = new List<string>();

                foreach (string str in validParameters)
                {
                    validParametersTemp.Add(str.ToLower());
                }
            }
            else
            {
                validParametersTemp = validParameters;
            }

            if (Parameters != null)
            {
                foreach (string key in Parameters.Keys)
                {
                    if (!validParametersTemp.Contains(key))
                    {
                        Logger.Instance.Error("Invalid Parameter: " + key);
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
