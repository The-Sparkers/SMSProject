using SMSProject.Models.HelperModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SMSProject.Models
{
    public class Examination
    {
        Month month;
        string examName;
        bool isPublished;
        int id;
        SqlConnection con;
        SqlCommand cmd;
        string query;
        public Examination(int id, string connectionString)
        {
            con = new SqlConnection(connectionString);
            this.id = id;
            SetValues();
        }
        public Examination(string examName, Month month, string connectionString)
        {
            con = new SqlConnection(connectionString);
            try
            {
                query = "INSERT INTO [EXAMINATIONS] ([ExamName] ,[Month] ,[Year]) VALUES ('" + examName + "' ," + month.Number + " ," + month.Year + ");SELECT MAX(ExamId) FROM EXAMINATIONS;";
                cmd = new SqlCommand(query, con);
                con.Open();
                id = (int)cmd.ExecuteScalar();
                con.Close();
            }
            catch (SqlException ex)
            {
                Exception e = new Exception("Error Occured in Database processing. CodeIndex:176", ex);
                throw e;
            }
            this.examName = examName;
            this.month = month;
        }
        public int ExamId
        {
            get
            {
                return id;
            }
        }
        public string ExamName
        {
            get
            {
                return examName;
            }
            set
            {
                try
                {
                    query = "UPDATE EXAMINATIONS SET ExamName='" + value + "' WHERE ExamId=" + id;
                    cmd = new SqlCommand(query, con);
                    con.Open();
                    if (cmd.ExecuteNonQuery() == 0)
                    {
                        throw new Exception("Nothing has updated in the database. CodeIndex:108");
                    }
                    con.Close();
                    examName = value;
                }
                catch (SqlException ex)
                {
                    Exception e = new Exception("Error Occured in Database processing. CodeIndex:178", ex);
                    throw e;
                }
            }
        }
        public Month Month
        {
            get
            {
                return month;
            }
        }
        public bool IsPublished
        {
            get
            {
                return isPublished;
            }
            set
            {
                try
                {
                    query = "UPDATE EXAMINATIONS SET IsPublished='" + value + "' WHERE ExamId=" + id;
                    cmd = new SqlCommand(query, con);
                    con.Open();
                    if (cmd.ExecuteNonQuery() == 0)
                    {
                        throw new Exception("Nothing has updated in the database. CodeIndex:108");
                    }
                    con.Close();
                    isPublished = value;
                }
                catch (SqlException ex)
                {
                    Exception e = new Exception("Error Occured in Database processing. CodeIndex:177", ex);
                    throw e;
                }
            }
        }
        public void Delete()
        {
            try
            {
                query = "DELETE FROM EXAMINATIONS WHERE ExamId=" + id;
                cmd = new SqlCommand(query, con);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
            catch (SqlException ex)
            {
                Exception e = new Exception("Error Occured in Database processing. CodeIndex:179", ex);

            }
        }
        public void AddMarks(int studentId, int subjectId, decimal obtainedMarks, decimal totalMarks, string teacherRemarks)
        {
            try
            {
                query = "INSERT INTO [STUDENTS_GET_EXAM_RESULTS] ([StudentId] ,[SubjectId] ,[ExamId] ,[ObtainedMarks] ,[TotalMarks] ,[TeacherRemarks]) VALUES (" + studentId + " ," + subjectId + " ," + id + " ," + obtainedMarks + " ," + totalMarks + " ,'" + teacherRemarks + "')";
                cmd = new SqlCommand(query, con);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
            catch (SqlException ex)
            {
                Exception e = new Exception("Error Occured in Database processing. CodeIndex:265", ex);

            }
        }
        private void SetValues()
        {
            try
            {
                query = "SELECT * FROM EXAMINATIONS WHERE ExamId=" + id;
                cmd = new SqlCommand(query, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    examName = (string)reader[1];
                    month.Number = (int)reader[2];
                    month.Year = (int)reader[3];
                    isPublished = (bool)reader[4];
                }
                con.Close();
            }
            catch (SqlException ex)
            {
                Exception e = new Exception("Error Occured in Database processing. CodeIndex:175", ex);
                throw e;
            }
        }
        public static List<Examination> GetAllExamnations(string connectionString)
        {
            SqlConnection con = new SqlConnection(connectionString);
            List<Examination> lst = new List<Examination>();
            try
            {
                string query = "SELECT ExamId FROM EXAMINATIONS";
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lst.Add(new Examination((int)reader[0], con.ConnectionString));
                }
                con.Close();
            }
            catch (SqlException ex)
            {
                Exception e = new Exception("Error Occured in Database processing. CodeIndex:180", ex);
                throw e;
            }
            return lst;
        }
        public static List<Examination> GetAllPublishedExamnations(string connectionString)
        {
            SqlConnection con = new SqlConnection(connectionString);
            List<Examination> lst = new List<Examination>();
            try
            {
                string query = "SELECT ExamId FROM EXAMINATIONS WHERE IsPublished=1";
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lst.Add(new Examination((int)reader[0], con.ConnectionString));
                }
                con.Close();
            }
            catch (SqlException ex)
            {
                Exception e = new Exception("Error Occured in Database processing. CodeIndex:267", ex);
                throw e;
            }
            return lst;
        }
    }
}