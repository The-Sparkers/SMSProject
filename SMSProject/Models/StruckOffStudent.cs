using SMSProject.Models.ModelEnums;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace SMSProject.Models
{
    
    public class StruckOffStudent
    {
        int id;
        string fName, fCell, boardExam, bForm, lastClass, sName;
        Genders gender;
        DateTime dateOfStruck;
        SqlConnection con;
        SqlCommand cmd;
        string query;
        public StruckOffStudent(int id, string connectionString)
        {
            con = new SqlConnection(connectionString);
            this.id = id;
            SetValues();
        }
        private void SetValues()
        {
            try
            {
                query = "select * from [STRUCK-OFF_STUDENTS] where SStudentId=" + id;
                cmd = new SqlCommand(query, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    sName = (string)reader[1];
                    fName = (string)reader[2];
                    fCell = (string)reader[3];
                    boardExam = (string)reader[4];
                    lastClass = (string)reader[5];
                    dateOfStruck = (DateTime)reader[6];
                    bForm = (string)reader[7];
                    gender = (Genders)Convert.ToInt16(reader[8]);
                }
                con.Close();
            }
            catch (SqlException ex)
            {
                Exception e = new Exception("Error Occured in Database processing. CodeIndex:140", ex);
                throw e;
            }
        }

        public StruckOffStudent(Student student, string boardRollNo, string connectionString)
        {
            try
            {
                con = new SqlConnection(connectionString);
                query = "AddStruckOffStudent";
                cmd = new SqlCommand(query, con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@studentId", System.Data.SqlDbType.Int)).Value = student.StudentId;
                cmd.Parameters.Add(new SqlParameter("@dateOfStruck", System.Data.SqlDbType.Date)).Value = DateTime.Now;
                cmd.Parameters.Add(new SqlParameter("@boardRoll", System.Data.SqlDbType.NChar)).Value = boardRollNo;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
            catch (SqlException ex)
            {
                Exception e = new Exception("Error Occured in Database processing. CodeIndex:165", ex);
                throw e;
            }
        }
        public int Id
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
                return sName;
            }
        }
        public string FatherName
        {
            get
            {
                return fName;
            }
        }
        public string FatherCellNo
        {
            get
            {
                return fCell;
            }
        }
        public string BoardExamRollNumber
        {
            get
            {
                return boardExam;
            }
        }
        public string BFormNumber
        {
            get
            {
                return bForm;
            }
        }
        public Genders Gender
        {
            get
            {
                return gender;
            }
        }
        public DateTime DateOfStruck
        {
            get
            {
                return dateOfStruck;
            }
        }
        public string LastClass
        {
            get
            {
                return lastClass;
            }
        }
        public static List<StruckOffStudent> GetAllStruckedStudents(string searchName, string connectionString)
        {
            SqlConnection con = new SqlConnection(connectionString);
            List<StruckOffStudent> lst = new List<StruckOffStudent>();
            try
            {
                string query = "Select SStudentId from [STRUCK-OFF_STUDENTS] WHERE Name LIKE '" + searchName + "%' ORDER BY DateOfStruck";
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lst.Add(new StruckOffStudent((int)reader[0], con.ConnectionString));
                }
            }
            catch (SqlException ex)
            {
                Exception e = new Exception("Error Occured in Database processing. CodeIndex:141", ex);
                throw e;
            }
            return lst;
        }
    }
}