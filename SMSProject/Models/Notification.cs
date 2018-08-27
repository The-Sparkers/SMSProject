using SMSProject.Models.ModelEnums;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SMSProject.Models
{
    public class Notification
    {
        string message;
        DateTime date;
        bool isSms, isWeb, forParent, forTeacher;
        SqlConnection con;
        SqlCommand cmd;
        string query;
        long id;
        public Notification(long id, string connectionString)
        {
            con = new SqlConnection(connectionString);
            this.id = id;
            SetValues();
        }
        public Notification(string message, DateTime date, NotificationStatuses status, NotificationTypes type, string connectionString)
        {
            con = new SqlConnection(connectionString);
            try
            {
                if (status == NotificationStatuses.ForTeacher)
                {
                    if (type == NotificationTypes.SMS)
                    {
                        query = "INSERT INTO [NOTIFICATIONS] ([Message] ,[Date] ,[IsSMS] ,[TeacherFlag]) VALUES ('"+message+"' ,'"+date+"' ,1 ,1); Select MAX(NotificationId) from NOTIFICATIONS";
                    }
                    else if (type == NotificationTypes.Web)
                    {
                        query = "INSERT INTO [NOTIFICATIONS] ([Message] ,[Date] ,[IsWeb] ,[TeacherFlag]) VALUES ('"+message+"' ,'"+date+"' ,1 ,1); Select MAX(NotificationId) from NOTIFICATIONS";
                    }
                    else if (type == NotificationTypes.All)
                    {
                        query = "INSERT INTO [NOTIFICATIONS] ([Message] ,[Date] ,[IsWeb], [IsSMS] ,[TeacherFlag]) VALUES ('" + message + "' ,'" + date + "' ,1 ,1 ,1); Select MAX(NotificationId) from NOTIFICATIONS";
                    }
                }
                else if (status == NotificationStatuses.ForParent)
                {
                    if (type == NotificationTypes.SMS)
                    {
                        query = "INSERT INTO [NOTIFICATIONS] ([Message] ,[Date] ,[IsSMS] ,[ParentFlag]) VALUES ('" + message + "' ,'" + date + "' ,1 ,1); Select MAX(NotificationId) from NOTIFICATIONS";
                    }
                    else if (type == NotificationTypes.Web)
                    {
                        query = "INSERT INTO [NOTIFICATIONS] ([Message] ,[Date] ,[IsWeb] ,[ParentFlag]) VALUES ('" + message + "' ,'" + date + "' ,1 ,1); Select MAX(NotificationId) from NOTIFICATIONS";
                    }
                    else if (type == NotificationTypes.All)
                    {
                        query = "INSERT INTO [NOTIFICATIONS] ([Message] ,[Date] ,[IsWeb], [IsSMS] ,[ParentFlag]) VALUES ('" + message + "' ,'" + date + "' ,1 ,1 ,1); Select MAX(NotificationId) from NOTIFICATIONS";
                    }
                }
                else if (status == NotificationStatuses.ForAll)
                {
                    if (type == NotificationTypes.SMS)
                    {
                        query = "INSERT INTO [NOTIFICATIONS] ([Message] ,[Date] ,[IsSMS] ,[TeacherFlag] ,[ParentFlag]) VALUES ('" + message + "' ,'" + date + "' ,1 ,1, 1); Select MAX(NotificationId) from NOTIFICATIONS";
                    }
                    else if (type == NotificationTypes.Web)
                    {
                        query = "INSERT INTO [NOTIFICATIONS] ([Message] ,[Date] ,[IsWeb] ,[TeacherFlag] ,[ParentFlag]) VALUES ('" + message + "' ,'" + date + "' ,1 ,1, 1); Select MAX(NotificationId) from NOTIFICATIONS";
                    }
                    else if (type == NotificationTypes.All)
                    {
                        query = "INSERT INTO [NOTIFICATIONS] ([Message] ,[Date] ,[IsWeb], [IsSMS] ,[TeacherFlag] ,[ParentFlag]) VALUES ('" + message + "' ,'" + date + "' ,1 ,1 ,1, 1); Select MAX(NotificationId) from NOTIFICATIONS";
                    }
                }
                cmd = new SqlCommand(query, con);
                con.Open();
                id = (long)cmd.ExecuteScalar();
                con.Close();
                SetValues();
            }
            catch (SqlException ex)
            {
                Exception e = new Exception("Error Occured in Database processing. CodeIndex:130", ex);
                throw e;
            }
        }
        public long NotificationId
        {
            get
            {
                return id;
            }
        }
        public DateTime Date
        {
            get
            {
                return date;
            }
        }
        public bool IsSMS
        {
            get
            {
                return isSms;
            }
        }
        public bool IsWeb
        {
            get
            {
                return isWeb;
            }
        }
        public string Message
        {
            get
            {
                return message;
            }
            set
            {
                try
                {
                    query = "update NOTIFICATIONS set Message='" + value + "' where NotificationId=" + id;
                    cmd = new SqlCommand(query, con);
                    con.Open();
                    if (cmd.ExecuteNonQuery() == 0)
                    {
                        throw new Exception("Nothing has updated in the database. CodeIndex:108");
                    }
                    con.Close();
                    message = value;
                }
                catch (SqlException ex)
                {
                    Exception e = new Exception("Error Occured in Database processing. CodeIndex:129", ex);
                    throw e;
                }
            }
        }
        public bool ForParent
        {
            get
            {
                return forParent;
            }
        }
        public bool ForTeacher
        {
            get
            {
                return forTeacher;
            }
        }
        public void Delete()
        {
            try
            {
                query = "DELETE FROM NOTIFICATIONS WHERE NotificationId=" + id;
                cmd = new SqlCommand(query, con);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
            catch (SqlException ex)
            {
                Exception e = new Exception("Error Occured in Database processing. CodeIndex:173", ex);
                throw e;
            }
        }
        private void SetValues()
        {
            try
            {
                query = "Select * from NOTIFICATIONS where NotificationId=" + id;
                cmd = new SqlCommand(query, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    message = (string)reader[1];
                    date = (DateTime)reader[2];
                    isSms = (bool)reader[3];
                    isWeb = (bool)reader[4];
                    forParent = (bool)reader[5];
                    forTeacher = (bool)reader[6];
                }
                con.Close();
            }
            catch (SqlException ex)
            {
                Exception e = new Exception("Error Occured in Database processing. CodeIndex:128", ex);
                throw e;
            }
        }
        public static List<Notification> GetAllNotifications(string connectionString)
        {
            List<Notification> lst = new List<Notification>();
            SqlConnection con = new SqlConnection(connectionString);
            try
            {
                string query = "select NotificationId from NOTIFICATIONS order by [Date] DESC";
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lst.Add(new Notification((long)reader[0], connectionString));
                }
                con.Close();
            }
            catch (SqlException ex)
            {
                Exception e = new Exception("Error Occured in Database processing. CodeIndex:122", ex);
                throw e;
            }
            return lst;
        }
    }
}