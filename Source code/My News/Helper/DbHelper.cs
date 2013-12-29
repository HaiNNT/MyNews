using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace My_News
{
    /// <summary>
    /// How to use:
    /// SqlCommand cmd = new SqlCommand("YOUR SQL QUERY STRING @YOUR_PARAMETER", DbHelper.connection);
    /// cmd.Parameters.AddWithValue("@YOUR_PARAMETER", "YOUR VALUE");
    /// DbHelper.Open();
    /// cmd.Execute...
    /// DbHelper.Close();
    /// </summary>
    static class DbHelper
    {
        public static SqlConnection connection = new SqlConnection(GetConnectionString());

        public static bool NoServerMode { get; set; }

        public static string GetConnectionString()
        {
            if (Properties.Settings.Default.db_use_trusted)
            {
                return @"Server=" + Properties.Settings.Default.db_server + ";Database=" + Properties.Settings.Default.db_database + ";Trusted_Connection=True;";
            }
            else
            {
                return @"Server=" + Properties.Settings.Default.db_server + ";Database=" + Properties.Settings.Default.db_database + ";User Id=" + Properties.Settings.Default.db_username + ";Password=" + Properties.Settings.Default.db_password + ";";
            }
        }

        public static void Open()
        {
            connection.Open();
        }
        public static void Close()
        {
            connection.Close();
        }

        public static bool IsConnected()
        {
            try
            {
                connection = new SqlConnection(GetConnectionString());
                DbHelper.Open();
                DbHelper.Close();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}
