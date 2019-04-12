using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace SMSProject.Models
{
    /// <summary>
    /// Student's Daily Diary on which homework tasks are written.
    /// </summary>
    public class DailyDairy
    {
        DateTime date;
        string content;
        int subjectId;
        long id;
        SqlConnection con;
        SqlCommand cmd;
        string query;
        public DailyDairy(long id, string connectionString)
        {
            con = new SqlConnection(connectionString);
            this.id = id;
            SetValues();
        }
        public DailyDairy(string content, int subjectId, string connectionString)
        {
            con = new SqlConnection(connectionString);
            try
            {
                query = "INSERT INTO [dbo].[DAILY_DAIRIES] ([Date] ,[Content] ,[SubjectId]) VALUES ('" + DateTime.Now + "' ,'" + content + "' ," + subjectId + ");SELECT MAX(DairyId) FROM DAILY_DAIRIES;";
                cmd = new SqlCommand(query, con);
                con.Open();
                id = (long)cmd.ExecuteScalar();
                con.Close();
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2601)
                {
                    throw new Exception("Record already exists. CodeIndex:143", ex);
                }
                Exception e = new Exception("Error Occured in Database processing. CodeIndex:168", ex);
                throw e;
            }
        }
        public long DailyDairyId
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
        public string Content
        {
            get
            {
                return content;
            }
            set
            {
                try
                {
                    query = "UPDATE DAILY_DAIRIES SET Content='" + value + "' WHERE DairyId=" + id;
                    cmd = new SqlCommand(query, con);
                    con.Open();
                    if (cmd.ExecuteNonQuery() == 0)
                    {
                        throw new Exception("Nothing has updated in the database. CodeIndex:108");
                    }
                    con.Close();
                    content = value;
                }
                catch (SqlException ex)
                {
                    Exception e = new Exception("Error Occured in Database processing. CodeIndex:169", ex);
                    throw e;
                }
            }
        }
        public Subject Subject
        {
            get
            {
                return new Subject(subjectId, con.ConnectionString);
            }
        }
        public void Delete()
        {
            try
            {
                query = "DELETE FROM DAILY_DAIRIES WHERE DairyId=" + id;
                cmd = new SqlCommand(query, con);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
            catch (SqlException ex)
            {
                Exception e = new Exception("Error Occured in Database processing. CodeIndex:172", ex);
                throw e;
            }
        }
        private void SetValues()
        {
            try
            {
                query = "SELECT * FROM DAILY_DAIRIES WHERE DairyId=" + id;
                cmd = new SqlCommand(query, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    date = (DateTime)reader[1];
                    content = (string)reader[2];
                    subjectId = (int)reader[3];
                }
                con.Close();
            }
            catch (SqlException ex)
            {
                Exception e = new Exception("Error Occured in Database processing. CodeIndex:167", ex);
                throw e;
            }
        }
        /// <summary>
        /// Gets a diary of a specific subject for a specific date.
        /// </summary>
        /// <param name="subjectId">Unique Id of Subject</param>
        /// <param name="date">Date</param>
        /// <param name="connectionString">Connection to the DB of School</param>
        /// <returns></returns>
        public static DailyDairy GetDairyOfSubjectOnDate(int subjectId, DateTime date, string connectionString)
        {
            SqlConnection con = new SqlConnection(connectionString);
            long dId;
            try
            {
                string query = "SELECT DairyId FROM DAILY_DAIRIES WHERE [Date]='" + date + "' AND SubjectId=" + subjectId;
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                dId = (long)cmd.ExecuteScalar();
                con.Close();
            }
            catch (SqlException ex)
            {
                Exception e = new Exception("Error Occured in Database processing. CodeIndex:170", ex);
                throw e;
            }
            return new DailyDairy(dId, con.ConnectionString);
        }
        /// <summary>
        /// Gets a list of All Diaries for a subject.
        /// </summary>
        /// <param name="subjectId">unique id of the subject</param>
        /// <param name="connectionString">connection to the DB of school</param>
        /// <returns></returns>
        public static List<DailyDairy> GetDairiesOfSubject(int subjectId, string connectionString)
        {
            SqlConnection con = new SqlConnection(connectionString);
            List<DailyDairy> lst = new List<DailyDairy>();
            try
            {
                string query = "SELECT DairyId FROM DAILY_DAIRIES WHERE SubjectId=" + subjectId;
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lst.Add(new DailyDairy((long)reader[0], connectionString));
                }
                con.Close();
            }
            catch (SqlException ex)
            {
                Exception e = new Exception("Error Occured in Database processing. CodeIndex:171", ex);
                throw e;
            }
            return lst;
        }
    }
}