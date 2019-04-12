using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace SMSProject.Models
{
    /// <summary>
    /// A Class is a combination of students of a specific Level.
    /// A Class has different sections.
    /// </summary>
    public class Class
    {
        int id, inchargeId, rollNoIndex;
        string name;
        SqlConnection con;
        SqlCommand cmd;
        string query;
        public Class(int id, string connectionString)
        {
            con = new SqlConnection(connectionString);
            this.id = id;
            SetValues();
        }
        public Class(string name, int rollNoIndex, int inchargeId, string connectionString)
        {
            con = new SqlConnection(connectionString);
            try
            {
                query = "INSERT INTO [dbo].[CLASSES] ([Name] ,[RollNoIndex] ,[InchargeId]) VALUES ('"+name+"' ,"+rollNoIndex+" ,"+inchargeId+ "); SELECT MAX(ClassId) FROM CLASSES";
                cmd = new SqlCommand(query, con);
                con.Open();
                id = (int)cmd.ExecuteScalar();
                con.Close();
            }
            catch (SqlException ex)
            {
                Exception e = new Exception("Error Occured in Database processing. CodeIndex:215", ex);
                throw e;
            }
            this.name = name;
            this.rollNoIndex = rollNoIndex;
            this.inchargeId = inchargeId;
        }
        public int ClassId
        {
            get
            {
                return id;
            }
        }
        /// <summary>
        /// Index from which the first Roll number of a class starts
        /// </summary>
        public int RollNoIndex
        {
            get
            {
                return rollNoIndex;
            }
        }
        public string Name
        {
            get
            {
                return name;
            }
        }
        /// <summary>
        /// Number of students in the class
        /// </summary>
        public int Strength
        {
            get
            {
                int count = 0;
                try
                {
                    query = "SELECT COUNT(st.StudentId) FROM SECTIONS s, CLASSES c, STUDENTS st WHERE s.ClassId=c.ClassId AND st.SectionId=s.SectionId AND c.ClassId=" + id;
                    cmd = new SqlCommand(query, con);
                    con.Open();
                    count = (int)cmd.ExecuteScalar();
                    con.Close();
                }
                catch (SqlException ex)
                {
                    Exception e = new Exception("Error Occured in Database processing. CodeIndex:223", ex);
                    throw e;
                }
                return count;
            }
        }
        public Teacher Incharge
        {
            get
            {
                return new Teacher(inchargeId, con.ConnectionString);
            }
            set
            {
                try
                {
                    query = "UPDATE CLASSES SET InchargeId=" + value.StaffId + " WHERE ClassId=" + id;
                    cmd = new SqlCommand(query, con);
                    con.Open();
                    if (cmd.ExecuteNonQuery() == 0)
                    {
                        throw new Exception("Nothing has updated in the database. CodeIndex:108");
                    }
                    con.Close();
                }
                catch (SqlException ex)
                {
                    Exception e = new Exception("Error Occured in Database processing. CodeIndex:216", ex);
                    throw e;
                }
                inchargeId = value.StaffId;
            }
        }
        public void AddSubjects(string subjectName)
        {
            try
            {
                query = "INSERT INTO [SUBJECTS] ([Name] ,[ClassId]) VALUES ('" + subjectName + "' ," + id + ")";
                cmd = new SqlCommand(query, con);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
            catch (SqlException ex)
            {
                Exception e = new Exception("Error Occured in Database processing. CodeIndex:219", ex);
                throw e;
            }
        }
        /// <summary>
        /// Adds Downloadable files for a class.
        /// Mostly Includes the learning content.
        /// </summary>
        /// <param name="name">Name of the Content</param>
        /// <param name="fileName">File Name</param>
        /// <param name="uploadDate">Date of Upload</param>
        /// <param name="extention">File Extention</param>
        public void AddDownloadable(string name, string fileName, DateTime uploadDate, string extention)
        {
            try
            {
                query = "INSERT INTO [DOWNLOADABLES] ([Filename] ,[FileExtention] ,[DateUpload] ,[ClassId] ,[DownloadableName]) VALUES ('" + fileName + "' ,'" + extention + "' ,'" + uploadDate + "' ," + ClassId + " ,'" + name + "')";
                cmd = new SqlCommand(query, con);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
            catch (SqlException ex)
            {
                Exception e = new Exception("Error Occured in Database processing. CodeIndex:220", ex);
                throw e;
            }
        }
        public List<Downloadable> GetDownloadables()
        {
            List<Downloadable> lst = new List<Downloadable>();
            try
            {
                query = "SELECT DownloadableId FROM DOWNLOADABLES WHERE ClassId=" + id+ "ORDER BY DateUpload DESC";
                cmd = new SqlCommand(query, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lst.Add(new Downloadable((long)reader[0], con.ConnectionString));
                }
                con.Close();
            }
            catch (SqlException ex)
            {
                Exception e = new Exception("Error Occured in Database processing. CodeIndex:221", ex);
                throw e;
            }
            return lst;
        }
        public List<Subject> GetSubjects()
        {
            List<Subject> lst = new List<Subject>();
            try
            {
                query = "SELECT SubjectId FROM SUBJECTS WHERE ClassId=" + id;
                cmd = new SqlCommand(query, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lst.Add(new Subject((int)reader[0], con.ConnectionString));
                }
                con.Close();
            }
            catch (SqlException ex)
            {
                Exception e = new Exception("Error Occured in Database processing. CodeIndex:222", ex);
                throw e;
            }
            return lst;
        }
        public List<Section> GetSections()
        {
            List<Section> lst = new List<Section>();
            try
            {
                query = "SELECT SectionId FROM SECTIONS WHERE ClassId=" + id;
                cmd = new SqlCommand(query, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lst.Add(new Section((int)reader[0], con.ConnectionString));
                }
                con.Close();
            }
            catch (SqlException ex)
            {
                Exception e = new Exception("Error Occured in Database processing. CodeIndex:271", ex);
                throw e;
            }
            return lst;
        }
        private void SetValues()
        {
            try
            {
                query = "SELECT * FROM CLASSES WHERE ClassId=" + id;
                cmd = new SqlCommand(query, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    name = (string)reader[1];
                    rollNoIndex = (int)reader[2];
                    inchargeId = (int)reader[3];
                }
                con.Close();
            }
            catch (SqlException ex)
            {
                Exception e = new Exception("Error Occured in Database processing. CodeIndex:214", ex);
                throw e;
            }
        }
        public static List<Class> GetAllClasses(string connectionString)
        {
            List<Class> lst = new List<Class>();
            try
            {
                SqlConnection con = new SqlConnection(connectionString);
                string query = "Select ClassId FROM CLASSES";
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lst.Add(new Class((int)reader[0], con.ConnectionString));
                }
                con.Close();
            }
            catch (SqlException ex)
            {
                Exception e = new Exception("Error Occured in Database processing. CodeIndex:232", ex);
                throw e;
            }
            return lst;
        }
    }
}