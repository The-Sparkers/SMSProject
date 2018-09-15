using SMSProject.Models.HelperModels;
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
        public int GetAbsentStudents(DateTime date)
        {
            int count = 0;
            try
            {
                query = "SELECT COUNT(s.StudentId) FROM ATTANDANCES a , STUDENT_GIVES_DAILY_ATTANDANCE sta, STUDENTS s WHERE a.AttandanceId = sta.AttandanceId AND sta.StudentId = s.StudentId AND s.SectionId = " + id + " AND a.IsAbsent = 1 AND a.Date = '" + date + "'";
                cmd = new SqlCommand(query, con);
                con.Open();
                count = (int)cmd.ExecuteScalar();
                con.Close();
            }
            catch (SqlException ex)
            {
                Exception e = new Exception("Error Occured in Database processing. CodeIndex:276", ex);
                throw e;
            }
            return count;
        }
        public int GetPresentStudents(DateTime date)
        {
            int count = 0;
            try
            {
                count = Strength - GetAbsentStudents(date);            }
            catch (SqlException ex)
            {
                Exception e = new Exception("Error Occured in Database processing. CodeIndex:277", ex);
                throw e;
            }
            return count;
        }
        public MonthlyProgress GetMonthlyProgress(Month month)
        {
            DateTime monthStart = new DateTime(month.Year, month.Number, 1);
            DateTime monthEnd = monthStart.AddMonths(1).AddDays(-1);
            MonthlyProgress progress = new MonthlyProgress { MarksObtained = 0, TotalMarks = 0 };
            try
            {
                query = "GetMonthlyProgress";
                cmd = new SqlCommand(query, con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@monthStart", System.Data.SqlDbType.Date)).Value = monthStart;
                cmd.Parameters.Add(new SqlParameter("@monthEnd", System.Data.SqlDbType.Date)).Value = monthEnd;
                cmd.Parameters.Add(new SqlParameter("@month", System.Data.SqlDbType.Int)).Value = month.Number;
                cmd.Parameters.Add(new SqlParameter("@year", System.Data.SqlDbType.Int)).Value = month.Year;
                cmd.Parameters.Add(new SqlParameter("@sectionId", System.Data.SqlDbType.Int)).Value = id;
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    try
                    {
                        progress.MarksObtained = (decimal)reader[0];
                    }
                    catch (NullReferenceException)
                    {
                        progress.MarksObtained = 0;
                    }
                    try
                    {
                        progress.TotalMarks = (decimal)reader[1];
                    }
                    catch (NullReferenceException)
                    {
                        progress.TotalMarks = 0;
                    }
                }
                con.Close();
            }
            catch (SqlException ex)
            {
                Exception e = new Exception("Error Occured in Database processing. CodeIndex:278", ex);
                throw e;
            }
            return progress;
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
        public List<AssignedTeacher> GetAssingedTeachers()
        {
            List<AssignedTeacher> lst = new List<AssignedTeacher>();
            try
            {
                query = "SELECT StaffId,SubjectId FROM TEACHER_TEACHES_SUBJECT_OF_A_SECTION WHERE SectionId=" + id;
                cmd = new SqlCommand(query, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lst.Add(new AssignedTeacher
                    {
                        Subject = new Subject((int)reader[1], con.ConnectionString),
                        Teacher = new Teacher((int)reader[0], con.ConnectionString)
                    });
                }
                con.Close();
            }
            catch (SqlException ex)
            {
                Exception e = new Exception("Error Occured in Database processing. CodeIndex:280", ex);
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