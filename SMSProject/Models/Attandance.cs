using System;
using System.Data.SqlClient;

namespace SMSProject.Models
{
    /// <summary>
    /// Attendance is the state of going regularly to or being present at a the school.
    /// Will be marked for every staff member and student.
    /// </summary>
    public class Attendance
    {
        bool isAbsent;
        DateTime date;
        long id;
        SqlConnection con;
        SqlCommand cmd;
        string query;
        public Attendance(long id, string connectionString)
        {
            con = new SqlConnection(connectionString);
            this.id = id;
            SetValues();
        }
        private void SetValues()
        {
            try
            {
                query = "select * from ATTANDANCES where AttandanceId=" + id;
                cmd = new SqlCommand(query, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    date = (DateTime)reader[1];
                    isAbsent = (bool)reader[2];
                }
                con.Close();
            }
            catch (SqlException ex)
            {
                Exception e = new Exception("Error Occured in Database processing. CodeIndex:142", ex);
                throw e;
            }
        }
        public long AttendanceId
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
        public bool IsAbsent
        {
            get
            {
                return isAbsent;
            }
            set
            {
                try
                {
                    query = "update ATTANDANCES set IsAbsent='" + value + "' where AttandanceId=" + id;
                    cmd = new SqlCommand(query, con);
                    con.Open();
                    if (cmd.ExecuteNonQuery() == 0)
                    {
                        throw new Exception("Nothing has updated in the database. CodeIndex:108");
                    }
                    con.Close();
                    isAbsent = value;
                }
                catch (SqlException ex)
                {
                    Exception e = new Exception("Error Occured in Database processing. CodeIndex:145", ex);
                    throw e;
                }
            }
        }
    }
}