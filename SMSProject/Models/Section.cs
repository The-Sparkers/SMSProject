using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace SMSProject.Models
{
    public class Section
    {
        int id, classId;
        string name;
        SqlConnection con;
        SqlCommand cmd;
        string query;
        public Section(int id, string connectionString)
        {
            con = new SqlConnection(connectionString);
            this.id = id;
            SetValues();
        }
        public Section(string name, int classId, string connectionString)
        {
            con = new SqlConnection(connectionString);
            try
            {
                query = "INSERT INTO [dbo].[SECTIONS] ([Name] ,[ClassId]) VALUES ('" + name + "' ," + classId + "); SELECT MAX(SectionId) FROM SECTIONS;";
                cmd = new SqlCommand(query, con);
                con.Open();
                id = (int)cmd.ExecuteScalar();
                con.Close();
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2601)
                {
                    throw new Exception("Section with the same name already exists. CodeIndex:143", ex);
                }
                Exception e = new Exception("Error Occured in Database processing. CodeIndex:211", ex);
                throw e;
            }
            this.name = name;
            this.classId = classId;
        }
        public int SectionId
        {
            get
            {
                return id;
            }
        }
        public string Name
        {
            get
            {
                return name;
            }
        }
        public Class Class
        {
            get
            {
                return new Class(classId, con.ConnectionString);
            }
        }
        public int Strength
        {
            get
            {
                int count = 0;
                try
                {
                    query = "SELECT COUNT(*) FROM STUDENTS WHERE SectionId=" + id;
                    cmd = new SqlCommand(query, con);
                    con.Open();
                    count = (int)cmd.ExecuteScalar();
                    con.Close();
                }
                catch (SqlException ex)
                {
                    Exception e = new Exception("Error Occured in Database processing. CodeIndex:213", ex);
                    throw e;
                }
                return count;
            }
        }
        public void Delete()
        {
            try
            {
                query = "DELETE SECTIONS WHERE SectionId=" + id;
                cmd = new SqlCommand(query, con);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
            catch (SqlException ex)
            {
                Exception e = new Exception("Error Occured in Database processing. CodeIndex:217", ex);
                throw e;
            }
        }
        public List<Student> GetStudents()
        {
            List<Student> lst = new List<Student>();
            try
            {
                query = "SELECT StudentId FROM STUDENTS WHERE SectionId=" + id;
                cmd = new SqlCommand(query, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lst.Add(new Student((int)reader[0], con.ConnectionString));
                }
                con.Close();
            }
            catch (SqlException ex)
            {
                Exception e = new Exception("Error Occured in Database processing. CodeIndex:212", ex);
                throw e;
            }
            return lst;
        }
        private void SetValues()
        {
            try
            {
                query = "SELECT * FROM SECTIONS WHERE SectionId=" + id;
                cmd = new SqlCommand(query, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    name = (string)reader[1];
                    classId = (int)reader[2];
                }
                con.Close();
            }
            catch (SqlException ex)
            {
                Exception e = new Exception("Error Occured in Database processing. CodeIndex:210", ex);
                throw e;
            }
        }
    }
}