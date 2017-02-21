using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace FileDownload
{
    public class ConsoleLogger
    {
        private static Dictionary<Color, ConsoleColor> ColorMap = new Dictionary<Color, ConsoleColor>();

        private static FileDownloadLib.Logger instance = null;
        public static FileDownloadLib.Logger Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FileDownloadLib.Logger.Instance;

                    if (!instance.inited)
                    {
                        FileDownloadLib.Logger.Instance.Init(WriteLog, Color.Red, Color.Green, Color.Yellow);
                    }
                }

                return FileDownloadLib.Logger.Instance;
            }
        }

        static ConsoleLogger()
        {
            foreach (string name in Enum.GetNames(typeof(ConsoleColor)))
            {
                try
                {
                    ColorMap.Add(Color.FromName(name), (ConsoleColor)(Enum.Parse(typeof(ConsoleColor), name)));
                }
                catch (Exception ex) { }
            }
        }

        private static ConsoleColor GetConsoleColor(Color color)
        {
            if (ColorMap.ContainsKey(color))
            {
                return ColorMap[color];
            }

            return ConsoleColor.White;
        }

        private static void WriteLog(Object color, string log)
        {
            try
            {
                Color newColor = (Color)color;
                Console.ForegroundColor = GetConsoleColor(newColor);
            }
            catch (Exception)
            { }
            Console.WriteLine(log);
        }

    }
}
