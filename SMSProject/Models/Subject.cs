using System;
using System.Data.SqlClient;

namespace SMSProject.Models
{
    public class Subject
    {
        int id;
        string name;
        int classId;
        SqlConnection con;
        SqlCommand cmd;
        string query;
        public Subject(int id, string connectionString)
        {
            con = new SqlConnection(connectionString);
            this.id = id;
            SetValues();
        }
        public int SubjectId
        {
            get
            {
                return id;
            }
        }
        public Class Class
        {
            get
            {
                return new Class(classId, con.ConnectionString);
            }
        }
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                try
                {
                    query = "UPDATE SUBJECTS SET [Name]='" + value + "' WHERE SubjectId=" + id;
                    cmd = new SqlCommand(query, con);
                    con.Open();
                    if (cmd.ExecuteNonQuery() == 0)
                    {
                        throw new Exception("Nothing has updated in the database. CodeIndex:108");
                    }
                    con.Close();
                    name = value;
                }
                catch (SqlException ex)
                {
                    Exception e = new Exception("Error Occured in Database processing. CodeIndex:209", ex);
                    throw e;
                }
            }
        }
        public void Delete()
        {
            try
            {
                query = "DELETE SUBJECTS WHERE SubjectId=" + id;
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
        private void SetValues()
        {
            try
            {
                query = "SELECT * FROM SUBJECTS WHERE SubjectId=" + id;
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
                Exception e = new Exception("Error Occured in Database processing. CodeIndex:208", ex);
                throw e;
            }
        }
    }
}