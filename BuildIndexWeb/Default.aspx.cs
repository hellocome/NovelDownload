using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
namespace BuildIndexWeb
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                MsSqlFprBuildIndexWeb MSSQL = new MsSqlFprBuildIndexWeb();

                string content = string.Empty;
                if (!MSSQL.GetIndexContent(out content))
                {
                    htmlDisplay.Text = "Error";

                    htmlDisplay.Text += MSSQL.Log;
                }
                else
                {
                    htmlDisplay.Text = "Content";

                    htmlDisplay.Text += content;
                }
            }
            catch (Exception ex)
            {
                this.htmlDisplay.Text = "Error Exception";

                this.htmlDisplay.Text += ex.ToString();
            }
        }
    }
}