using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Text.RegularExpressions;

namespace FileDownload
{
    public class TextToImage
    {
        private System.Drawing.Bitmap CreateCheckCodeImage(string checkCode)
        {
            if (checkCode == null || checkCode.Trim() == String.Empty)
            {
                return null;
            }

            System.Drawing.Bitmap image = new System.Drawing.Bitmap((int)Math.Ceiling((checkCode.Length * 27.0)), 25);
            Graphics g = Graphics.FromImage(image);

            try
            {
                g.Clear(Color.White);

                //Font font = new System.Drawing.Font("Arial", 16, (System.Drawing.FontStyle.Bold 　 System.Drawing.FontStyle.Italic)); 
                System.Drawing.Font font = new System.Drawing.Font("楷体_GB2312", 16, (System.Drawing.FontStyle.Bold));
                System.Drawing.Drawing2D.LinearGradientBrush brush = new System.Drawing.Drawing2D.LinearGradientBrush(new System.Drawing.Rectangle(0, 0, image.Width, image.Height), Color.Blue, Color.DarkRed, 1.2f, true);
                g.DrawString(checkCode, font, brush, 2, 2);

                g.DrawRectangle(new Pen(Color.Silver), 0, 0, image.Width - 1, image.Height - 1);

                return image;
            }
            finally
            {
                //g.Dispose();
                //image.Dispose();
            }
        }


        public string FormatHTMLStringToNormal(string htmlString)
        {
            htmlString = Regex.Replace(htmlString, @"<br\s*/>", "\n", RegexOptions.IgnoreCase);

            return htmlString;
        }
    }
}
