using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace BuildIndexWeb
{
    public partial class BuildIndex : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void BtnExecute_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.TextBoxCMD.Text.Equals("BuildIndex", StringComparison.OrdinalIgnoreCase))
                {
                    ProcessIndex pi = new ProcessIndex();
                    MsSqlFprBuildIndexWeb MSSQL = new MsSqlFprBuildIndexWeb();
                    pi.Process(AppDomain.CurrentDomain.BaseDirectory, "Downloads/", MSSQL);
                    this.LiteralViewContent.Text = pi.Log;
                }
                else
                {
                    this.LiteralViewContent.Text = "No command was Executed";
                }
            }
            catch (Exception ex)
            {
                this.LiteralViewContent.Text = ex.Message;
            }
        }
    }
}