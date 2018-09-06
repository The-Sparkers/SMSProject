using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SMSProject.ServiceModules
{
    public class SMSService
    {
        public static bool SendSMS(string message, string number)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SMSServiceConString"].ConnectionString);
            SqlCommand cmd;
            string query;
            try
            {
                query = "INSERT INTO [ServiceTable] ([Body] ,[RecieverNumber]) VALUES ('" + message + "' ,'" + number + "')";
                cmd = new SqlCommand(query, con);
                con.Open();
                if (cmd.ExecuteNonQuery() != 0)
                {
                    return true;
                }
                con.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return false;
        }
    }
}