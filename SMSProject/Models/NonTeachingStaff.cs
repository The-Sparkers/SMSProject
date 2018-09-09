using SMSProject.Models.HelperModels;
using SMSProject.Models.ModelEnums;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace SMSProject.Models
{
    public class NonTeachingStaff : Staff
    {
        string jobType;
        public NonTeachingStaff(int id, string connectionString)
        {
            con = new SqlConnection(connectionString);
            this.id = id;
            SetValues();
            try
            {
                query = "SELECT JobType FROM [NON-TEACHING_STAFF] WHERE StaffId=" + id;
                cmd = new SqlCommand(query, con);
                con.Open();
                jobType = (string)cmd.ExecuteScalar();
                con.Close();
            }
            catch (SqlException ex)
            {
                throw new Exception("Database processing error. CodeIndex:272.", ex);
            }

        }
        public NonTeachingStaff(string name, string cnic, string address, MobileNumber number, decimal salary, Genders gender, string jobType, string connectionString)
        {
            con = new SqlConnection(connectionString);
            try
            {
                query = "AddNonStaff";
                cmd = new SqlCommand(query, con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@name", System.Data.SqlDbType.VarChar)).Value = name;
                cmd.Parameters.Add(new SqlParameter("@CNIC", System.Data.SqlDbType.NChar)).Value = cnic;
                cmd.Parameters.Add(new SqlParameter("@address", System.Data.SqlDbType.VarChar)).Value = address;
                cmd.Parameters.Add(new SqlParameter("@mCountryCode", System.Data.SqlDbType.NChar)).Value = number.CountryCode;
                cmd.Parameters.Add(new SqlParameter("@mCompanyCode", System.Data.SqlDbType.NChar)).Value = number.CompanyCode;
                cmd.Parameters.Add(new SqlParameter("@mNumber", System.Data.SqlDbType.NChar)).Value = number.Number;
                cmd.Parameters.Add(new SqlParameter("@salary", System.Data.SqlDbType.Money)).Value = salary;
                cmd.Parameters.Add(new SqlParameter("@jobType", System.Data.SqlDbType.VarChar)).Value = jobType;
                cmd.Parameters.Add(new SqlParameter("@gender", System.Data.SqlDbType.Bit)).Value = (int)gender;
                cmd.Parameters.Add(new SqlParameter("@joiningDate", System.Data.SqlDbType.Date)).Value = DateTime.Now;
                con.Open();
                id = (int)cmd.ExecuteScalar();
                con.Close();
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2601)
                {
                    throw new Exception("Field CNIC exists. CodeIndex:143", ex);
                }
                Exception e = new Exception("Error Occured in Database processing. CodeIndex:237", ex);
                throw e;
            }
            this.name = name;
            this.number = number;
            this.salary = salary;
            this.address = address;
            this.cnic = cnic;
            this.jobType = jobType;
        }
        public string JobType
        {
            get
            {
                return jobType;
            }
            set
            {
                try
                {
                    query = "UPDATE [NON-TEACHING_STAFF] SET JobType='" + value + "' WHERE StaffId=" + id;
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
                    Exception e = new Exception("Error Occured in Database processing. CodeIndex:238", ex);
                    throw e;
                }
                jobType = value;
            }
        }
        public static List<NonTeachingStaff> GetAllNonTeachingStaff(string connectionString, string matchName)
        {
            List<NonTeachingStaff> lst = new List<NonTeachingStaff>();
            try
            {
                SqlConnection con = new SqlConnection(connectionString);
                string query = "SELECT s.StaffId FROM STAFF s , [NON-TEACHING_STAFF] ns WHERE s.StaffId=ns.StaffId AND s.Name LIKE '%" + matchName + "%'";
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lst.Add(new NonTeachingStaff((int)reader[0], con.ConnectionString));
                }
                con.Close();
            }
            catch (SqlException ex)
            {
                Exception e = new Exception("Error Occured in Database processing. CodeIndex:239", ex);
                throw e;
            }
            return lst;
        }
    }
}