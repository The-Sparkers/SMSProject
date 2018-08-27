using System;
using System.Configuration;
using System.Data.SqlClient;

namespace SMSProject.Models
{
    public class TeacherAccount
    {
        string id, username;
        int teacherId;
        bool isActive;
        SqlConnection con;
        SqlCommand cmd;
        string query, modelConString;
        public TeacherAccount(string id, string connectionString)
        {
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["AspIdentityConection"].ConnectionString);
            modelConString = connectionString;
            this.id = id;
            SetValues();
        }
        public TeacherAccount(int teacherId, string connectionString)
        {
            try
            {
                SqlConnection modelConnection = new SqlConnection(connectionString);
                query = "select AccountId from TEACHER_HAS_ACCOUNT where StaffId=" + teacherId;
                cmd = new SqlCommand(query, modelConnection);
                modelConnection.Open();
                id = (string)cmd.ExecuteScalar();
                modelConnection.Close();
            }
            catch (SqlException ex)
            {
                throw new Exception("Error Occured in Database processing. CodeIndex:261", ex);
            }
        }
        public string AccountId
        {
            get
            {
                return id;
            }
        }
        public Parent Parent
        {
            get
            {
                return new Parent(teacherId, modelConString);
            }
        }
        public string Username
        {
            get
            {
                return username;
            }
        }
        public bool IsActive
        {
            get
            {
                return isActive;
            }
            set
            {
                try
                {
                    query = "UPDATE TEACHER_HAS_ACCOUNT SET IsActive=" + value + " WHERE AccountId='" + id + "' AND StaffId=" + teacherId;
                    SqlConnection modelConnection = new SqlConnection(modelConString);
                    cmd = new SqlCommand(query, modelConnection);
                    modelConnection.Open();
                    cmd.ExecuteNonQuery();
                    modelConnection.Close();
                    isActive = value;
                }
                catch (SqlException ex)
                {
                    throw new Exception("Error Occured in Database processing. CodeIndex:263", ex);
                }
            }
        }
        private void SetValues()
        {
            try
            {
                query = "Select UserName from AspNetUsers where Id='" + id + "'";
                cmd = new SqlCommand(query, con);
                con.Open();
                username = (string)cmd.ExecuteScalar();
                con.Close();
            }
            catch (SqlException ex)
            {
                throw new Exception("Error Occured in Database processing. CodeIndex:258", ex);
            }
            try
            {
                query = "Select StaffId, IsActive from TEACHER_HAS_ACCOUNT where AccountId='" + id + "'";
                SqlConnection modelConnection = new SqlConnection(modelConString);
                cmd = new SqlCommand(query, modelConnection);
                modelConnection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    teacherId = (int)reader[0];
                    isActive = (bool)reader[1];
                }
                modelConnection.Close();
            }
            catch (SqlException ex)
            {
                throw new Exception("Error Occured in Database processing. CodeIndex:259", ex);
            }
        }
    }
}