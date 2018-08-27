using System;
using System.Data.SqlClient;

namespace SMSProject.Models
{
    public class Downloadable
    {
        string name, fileName, fileExtention;
        DateTime dateUpload;
        int classId;
        long id;
        SqlConnection con;
        SqlCommand cmd;
        string query;
        public Downloadable(long id, string connectionString)
        {
            con = new SqlConnection(connectionString);
            this.id = id;
            SetValues();
        }
        public string Name
        {
            get
            {
                return name;
            }
        }
        public string FileName
        {
            get
            {
                return fileName;
            }
        }
        public string FileExtention
        {
            get
            {
                return fileExtention;
            }
        }
        public DateTime DateUpload
        {
            get
            {
                return dateUpload;
            }
        }
        public Class Class
        {
            get
            {
                return new Class(classId, con.ConnectionString);
            }
        }
        public string GetFullFileName()
        {
            return fileName+ fileExtention;
        }
        private void SetValues()
        {
            try
            {
                query = "SELECT * FROM DOWNLOADABLES WHERE DownloadableId=" + id;
                cmd = new SqlCommand(query, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    name = (string)reader[1];
                    fileExtention = (string)reader[2];
                    dateUpload = (DateTime)reader[3];
                    classId = (int)reader[4];
                    fileName = (string)reader[5];
                }
                con.Close();
            }
            catch (SqlException ex)
            {
                Exception e = new Exception("Error Occured in Database processing. CodeIndex:224", ex);
                throw e;
            }
        }
        public void Delete()
        {
            try
            {
                query = "DELETE FROM DOWNLOADABLES WHERE DownloadableId=" + id;
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
    }
}