using SMSProject.Models.ModelEnums;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace SMSProject.Models
{
    /// <summary>
    /// Application will be sent by the parent or student of will be reviewed by the admin.
    /// Can be sent more than one by a parent or a teacher.
    /// Have three types of statuses: Pending, Accepted and Rejected.
    /// </summary>
    public class Application
    {
        long id;
        int parentId;
        DateTime date;
        string body;
        SqlConnection con;
        SqlCommand cmd;
        string query;
        ApplicationStatuses status;
        public Application(long id, string connectionString)
        {
            con = new SqlConnection(connectionString);
            this.id = id;
            SetValues();
        }
        public Application(string body, DateTime date, Parent parent, string connectionString)
        {
            con = new SqlConnection(connectionString);
            try
            {
                query = "INSERT INTO [APPLICATIONS] ([Body] ,[Date] ,[ParentId]) VALUES ('" + body + "' ,'" + date + "' ," + parent.ParentId + ");SELECT MAX(ApplicationId) FROM APPLICATIONS";
                cmd = new SqlCommand(query, con);
                con.Open();
                id = (long)cmd.ExecuteScalar();
                con.Close();
            }
            catch (SqlException ex)
            {
                Exception e = new Exception("Error Occured in Database processing. CodeIndex:135", ex);
                throw e;
            }
            this.body = body;
            this.date = date;
            parentId = parent.ParentId;

        }
        public long ApplicationId
        {
            get
            {
                return id;
            }
        }
        public string Body
        {
            get
            {
                return body;
            }
        }
        public ApplicationStatuses Status
        {
            get
            {
                return status;
            }
            set
            {
                try
                {
                    query = "update APPLICATIONS set Status=" + (int)value + " where ApplicationId=" + id;
                    cmd = new SqlCommand(query, con);
                    con.Open();
                    if (cmd.ExecuteNonQuery() == 0)
                    {
                        throw new Exception("Nothing has updated in the database. CodeIndex:108");
                    }
                    con.Close();
                    status = value;
                }
                catch (SqlException ex)
                {
                    Exception e = new Exception("Error Occured in Database processing. CodeIndex:136", ex);
                    throw e;
                }
            }
        }
        public DateTime Date
        {
            get
            {
                return date;
            }
        }
        public Parent Parent
        {
            get
            {
                return new Parent(parentId, con.ConnectionString);
            }
        }

        private void SetValues()
        {
            try
            {
                query = "Select * from APPLICATIONS where ApplicationId=" + id;
                cmd = new SqlCommand(query, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    body = (string)reader[1];
                    date = (DateTime)reader[2];
                    status = (ApplicationStatuses)Convert.ToInt16(reader[3]);
                    parentId = (int)reader[4];
                }
                con.Close();
            }
            catch (SqlException ex)
            {
                Exception e = new Exception("Error Occured in Database processing. CodeIndex:137", ex);
                throw e;
            }
        }
        /// <summary>
        /// Returns a list of appliciations with Pending Status
        /// </summary>
        /// <param name="connectionString">Connection String of School DB</param>
        /// <returns></returns>
        public static List<Application> GetPendingApplications(string connectionString)
        {
            List<Application> lst = new List<Application>();
            SqlConnection con = new SqlConnection(connectionString);
            try
            {
                string query = "Select ApplicationId from APPLICATIONS where [status]=" + (int)ApplicationStatuses.Pending + " Order By Date DESC";
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lst.Add(new Application((long)reader[0], connectionString));
                }
                con.Close();
            }
            catch (SqlException ex)
            {
                Exception e = new Exception("Error Occured in Database processing. CodeIndex:138", ex);
                throw e;
            }
            return lst;
        }
        public static List<Application> GetAllApplications(string connectionString)
        {
            List<Application> lst = new List<Application>();
            SqlConnection con = new SqlConnection(connectionString);
            try
            {
                string query = "Select ApplicationId from APPLICATIONS Order By Date DESC";
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lst.Add(new Application((long)reader[0], connectionString));
                }
                con.Close();
            }
            catch (SqlException ex)
            {
                Exception e = new Exception("Error Occured in Database processing. CodeIndex:139", ex);
                throw e;
            }
            return lst;
        }
    }
}