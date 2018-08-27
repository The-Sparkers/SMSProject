using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace SMSProject.Models
{
    public class Test
    {
        long id;
        decimal totalMarks;
        DateTime heldDate;
        int subjectId;
        SqlConnection con;
        SqlCommand cmd;
        string query;
        public Test(long id, string connectionString)
        {
            con = new SqlConnection(connectionString);
            this.id = id;
            SetValues();
        }
        public Test(decimal totalMarks, DateTime heldDate, int subjectId, string connectionString)
        {
            con = new SqlConnection(connectionString);
            try
            {
                query = "INSERT INTO [TESTS] ([TotalMarks] ,[HeldDate] ,[SubjectId]) VALUES (" + totalMarks + " ,'" + heldDate + "' ," + subjectId + "); SELECT MAX(TestId) FROM TESTS;";
                cmd = new SqlCommand(query, con);
                con.Open();
                id = (long)cmd.ExecuteScalar();
                con.Close();
            }
            catch (SqlException ex)
            {
                Exception e = new Exception("Error Occured in Database processing. CodeIndex:204", ex);
                throw e;
            }
            this.totalMarks = totalMarks;
            this.heldDate = heldDate;
            this.subjectId = subjectId;
        }
        public long TestId
        {
            get
            {
                return id;
            }
        }
        public DateTime HeldDate
        {
            get
            {
                return heldDate;
            }
        }
        public decimal TotalMarks
        {
            get
            {
                return totalMarks;
            }
            set
            {
                try
                {
                    query = "UPDATE TESTS SET TotalMarks=" + value + " WHERE TestId=" + id;
                    cmd = new SqlCommand(query, con);
                    con.Open();
                    if (cmd.ExecuteNonQuery() == 0)
                    {
                        throw new Exception("Nothing has updated in the database. CodeIndex:108");
                    }
                    con.Close();
                    totalMarks = value;
                }
                catch (SqlException ex)
                {
                    Exception e = new Exception("Error Occured in Database processing. CodeIndex:205", ex);
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
        public bool AddTestMarks(int studentId, decimal marks, string remarks)
        {
            bool flag = false;
            try
            {
                query = "INSERT INTO [STUDENT_GET_TEST_RESULTS] ([StudentId] ,[TestId] ,[ObtainedMarks] ,[TeacherRemarks]) VALUES (" + studentId + " ," + id + " ," + marks + " ,'" + remarks + "')";
                cmd = new SqlCommand(query, con);
                con.Open();
                if (cmd.ExecuteNonQuery() != 0)
                {
                    flag = true;
                }
                con.Close();
            }
            catch (SqlException ex)
            {
                Exception e = new Exception("Error Occured in Database processing. CodeIndex:206", ex);
                throw e;
            }
            return flag;
        }
        private void SetValues()
        {
            try
            {
                query = "SELECT * FROM TESTS WHERE TestId=" + id;
                cmd = new SqlCommand(query, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    totalMarks = (decimal)reader[1];
                    heldDate = (DateTime)reader[2];
                    subjectId = (int)reader[3];
                }
                con.Close();
            }
            catch (SqlException ex)
            {
                Exception e = new Exception("Error Occured in Database processing. CodeIndex:203", ex);
                throw e;
            }
        }
        public static List<Test> GetAllTests(int subjectId, string connectionString)
        {
            SqlConnection con = new SqlConnection(connectionString);
            List<Test> lst = new List<Test>();
            try
            {
                string query = "SELECT TestId FROM TESTS WHERE SubjectId=" + subjectId + " ORDER BY HeldDate DESC";
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lst.Add(new Test((long)reader[0], con.ConnectionString));
                }
                con.Close();
            }
            catch (SqlException ex)
            {
                Exception e = new Exception("Error Occured in Database processing. CodeIndex:207", ex);
                throw e;
            }
            return lst;
        }
    }
}