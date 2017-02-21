using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;

namespace BuildIndexWeb
{
    public class MsSqlFprBuildIndexWeb
    {
        private string mLog = string.Empty;
        public string Log
        {
            get
            {
                return mLog;
            }
        }

        private const string connectString = "Data Source=hellocome.db.10418620.hostedresource.com; Initial Catalog=hellocome; User ID=hellocome; Password='Zhengnan1!';Connection Timeout=10";
        //private const string connectString = "Data Source=haijiexpvm; Initial Catalog=hellocome; User ID=TestUSR; Password='haijie';Connection Timeout=10";

        public bool SetIndexContent(string content)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectString))
                {
                    conn.Open();

                    string sqlStr = "Update TempData Set ItemValue=@ItemValue WHERE ItemName=@ItemName";

                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = sqlStr;
                    cmd.Parameters.AddWithValue("@ItemName", "IndexContent");
                    cmd.Parameters.AddWithValue("@ItemValue", content);

                    int dr = cmd.ExecuteNonQuery();

                    if (dr == 1)
                    {
                        return true;
                    }

                    mLog += "Affect row is not 1: " + dr;

                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                mLog += ex.Message;
            }

            return false;
        }

        public bool GetIndexContent(out string content)
        {
            content = string.Empty;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectString))
                {
                    conn.Open();

                    string sqlStr = "SELECT ItemValue FROM TempData WHERE ItemName=@ItemName";

                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = sqlStr;
                    cmd.Parameters.AddWithValue("@ItemName", "IndexContent");

                    SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.Read())
                    {
                        content = dr["ItemValue"].ToString();
                        return true;
                    }

                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                mLog += ex.Message;
            }

            mLog += "Nothing Read";
            return false;
        }
    }
}