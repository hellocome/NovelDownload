using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace FileDownloadWeb
{
    public partial class ShowLog : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string file = Request.QueryString["File"];

                file = string.Format("{0}\\{1}", AppDomain.CurrentDomain.BaseDirectory, file);

                StreamReader sw = new StreamReader(new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite));

                this.TextBoxLog.Text = sw.ReadToEnd();

                sw.Close();
            }
            catch (Exception ex)
            {
                this.TextBoxLog.Text = ex.ToString();
            }
        }
    }
}