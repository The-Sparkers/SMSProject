using SMSProject.Models.HelperModels;
using SMSProject.Models.ModelEnums;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace SMSProject.Models
{
    public class Staff
    {
        protected Genders gender;
        protected string name, cnic, address;
        protected decimal salary;
        protected MobileNumber number;
        protected int id;
        protected SqlConnection con;
        protected SqlCommand cmd;
        protected DateTime joiningDate;
        protected string query;
        public Staff(int id, string connectionString)
        {
            con = new SqlConnection(connectionString);
            this.id = id;
            SetValues();
        }
        protected Staff()
        {

        }
        public int StaffId
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
            set
            {
                try
                {
                    query = "UPDATE STAFF SET [Name]='" + value + "' WHERE StaffId=" + id;
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
                    Exception e = new Exception("Error Occured in Database processing. CodeIndex:226", ex);
                    throw e;
                }
                name = value;
            }
        }
        public string Address
        {
            get
            {
                return address;
            }
            set
            {
                try
                {
                    query = "UPDATE STAFF SET [Address]='" + value + "' WHERE StaffId=" + id;
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
                    Exception e = new Exception("Error Occured in Database processing. CodeIndex:227", ex);
                    throw e;
                }
                address = value;
            }
        }
        public string CNIC
        {
            get
            {
                return cnic;
            }
            set
            {
                try
                {
                    query = "UpdateStaffCNIC";
                    cmd = new SqlCommand(query, con);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@cnic", System.Data.SqlDbType.NChar)).Value = value;
                    cmd.Parameters.Add(new SqlParameter("@staffId", System.Data.SqlDbType.Int)).Value = id;
                    con.Open();
                    if (cmd.ExecuteNonQuery() == 0)
                    {
                        throw new Exception("Nothing has updated in the database, maybe the entered value is already present in the record. CodeIndex:108, 143");
                    }
                    con.Close();
                }
                catch (SqlException ex)
                {
                    Exception e = new Exception("Error Occured in Database processing. CodeIndex:229", ex);
                    throw e;
                }
                cnic = value;
            }
        }
        public MobileNumber PhoneNumber
        {
            get
            {
                return number;
            }
            set
            {
                try
                {
                    query = "UPDATE STAFF SET [MCountryCode]='" + value.CountryCode + "', [MCompanyCode]='" + value.CompanyCode + "', [MNumber]='" + value.Number + "' WHERE StaffId=" + id;
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
                    Exception e = new Exception("Error Occured in Database processing. CodeIndex:228", ex);
                    throw e;
                }
                number = value;
            }
        }
        public Genders Gender
        {
            get
            {
                return gender;
            }
        }
        public decimal Salary
        {
            get
            {
                return salary;
            }
            set
            {
                try
                {
                    query = "UPDATE STAFF SET [Salary]=" + value + " WHERE StaffId=" + id;
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
                    Exception e = new Exception("Error Occured in Database processing. CodeIndex:230", ex);
                    throw e;
                }
                salary = value;
            }
        }
        public DateTime Joiningdate
        {
            get
            {
                return joiningDate;
            }
        }
        public bool SetSalary(Month month, decimal perAbsentDeduction)
        {
            bool flag;
            try
            {
                query = "SetSalary";
                cmd = new SqlCommand(query, con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@month", System.Data.SqlDbType.Int)).Value = month.Number;
                cmd.Parameters.Add(new SqlParameter("@year", System.Data.SqlDbType.Int)).Value = month.Year;
                cmd.Parameters.Add(new SqlParameter("@staffId", System.Data.SqlDbType.Int)).Value = id;
                cmd.Parameters.Add(new SqlParameter("@perAbsent", System.Data.SqlDbType.Money)).Value = perAbsentDeduction;
                cmd.Parameters.Add(new SqlParameter("@salary", System.Data.SqlDbType.Money)).Value = salary;
                con.Open();
                flag = Convert.ToBoolean(cmd.ExecuteScalar());
                con.Close();
            }
            catch (SqlException ex)
            {
                Exception e = new Exception("Error Occured in Database processing. CodeIndex:231", ex);
                throw e;
            }
            return flag;
        }
        public void SetAttendance(DateTime date, bool isAbsent = false)
        {
            try
            {
                query = "SetStaffAttandance";
                cmd = new SqlCommand(query, con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@staffId", System.Data.SqlDbType.Int)).Value = id;
                cmd.Parameters.Add(new SqlParameter("@date", System.Data.SqlDbType.Date)).Value = date;
                cmd.Parameters.Add(new SqlParameter("@isAbsent", System.Data.SqlDbType.Bit)).Value = isAbsent;
                con.Open();
                if (cmd.ExecuteNonQuery() == 0)
                {
                    throw new Exception("Nothing has updated in the database. CodeIndex:108");
                }
                con.Close();
            }
            catch (SqlException ex)
            {
                Exception e = new Exception("Error Occured in Database processing. CodeIndex:234", ex);
                throw e;
            }
        }
        public List<Attendance> GetMonthAttendances(DateTime month)
        {
            List<Attendance> lst = new List<Attendance>();
            DateTime startDate = new DateTime(month.Year, month.Month, 1);
            DateTime endDate = startDate.AddMonths(1).AddDays(-1);
            try
            {
                query = "SELECT a.AttandanceId FROM ATTANDANCES a,STAFF_GIVES_DAILY_ATTANDANCE st WHERE st.AttandanceId=a.AttandanceId AND st.StaffId=" + id + " AND (a.[Date] BETWEEN '" + startDate + "' AND '" + endDate + "')";
                cmd = new SqlCommand(query, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lst.Add(new Attendance((long)reader[0], con.ConnectionString));
                }
                con.Close();
            }
            catch (SqlException ex)
            {
                Exception e = new Exception("Error Occured in Database processing. CodeIndex:160", ex);
                throw e;
            }
            return lst;
        }
        public int GetAbsents(DateTime month)
        {
            int absents = 0;
            DateTime startDate = new DateTime(month.Year, month.Month, 1);
            DateTime endDate = startDate.AddMonths(1).AddDays(-1);
            try
            {
                query = "SELECT COUNT(*) FROM STAFF_GIVES_DAILY_ATTANDANCE sa, ATTANDANCES a WHERE sa.AttandanceId=a.AttandanceId AND sa.StaffId=" + id + " AND (a.[Date] BETWEEN '" + startDate + "' AND '" + endDate + "') AND a.IsAbsent=1";
                cmd = new SqlCommand(query, con);
                con.Open();
                absents = (int)cmd.ExecuteScalar();
                con.Close();
            }
            catch (SqlException ex)
            {
                Exception e = new Exception("Error Occured in Database processing. CodeIndex:236", ex);
                throw e;
            }
            return absents;
        }
        public MonthlySalary GetMonthSalary(Month month)
        {
            MonthlySalary sal;
            try
            {
                query = "SELECT MSId FROM MONTHLY_SALARIES WHERE StaffId=" + id + " AND [Month]=" + month.Number + " AND [Year]=" + month.Year;
                cmd = new SqlCommand(query, con);
                con.Open();
                sal = new MonthlySalary((long)cmd.ExecuteScalar(), con.ConnectionString);
                con.Close();
            }
            catch (SqlException ex)
            {
                Exception e = new Exception("Error Occured in Database processing. CodeIndex:253", ex);
                throw e;
            }
            return sal;
        }
        public bool Delete()
        {
            bool flag = false;
            try
            {
                query = "DELETE FROM STAFF WHERE StaffId=" + id;
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
                Exception e = new Exception("Error Occured in Database processing. CodeIndex:244", ex);
                throw e;
            }

            return flag;
        }
        protected void SetValues()
        {
            try
            {
                query = "SELECT * FROM STAFF WHERE StaffId=" + id;
                cmd = new SqlCommand(query, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    name = (string)reader[1];
                    cnic = (string)reader[2];
                    address = (string)reader[3];
                    number = new MobileNumber((string)reader[4], (string)reader[5], (string)reader[6]);
                    salary = (decimal)reader[7];
                    gender = (Genders)Convert.ToInt16(reader[8]);
                    joiningDate = (DateTime)reader[9];
                }
                con.Close();
            }
            catch (SqlException ex)
            {
                Exception e = new Exception("Error Occured in Database processing. CodeIndex:225", ex);
                throw e;
            }
        }
        public static List<Staff> GetAllStaff(string connectionString)
        {
            List<Staff> lst = new List<Staff>();
            try
            {
                SqlConnection con = new SqlConnection(connectionString);
                string query = "SELECT StaffId FROM STAFF";
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lst.Add(new Staff((int)reader[0], con.ConnectionString));
                }
                con.Close();
            }
            catch (SqlException ex)
            {
                Exception e = new Exception("Error Occured in Database processing. CodeIndex:233", ex);
                throw e;
            }
            return lst;
        }
        public static List<Staff> GetAllUnsetSalaryStaff(Month month, string connectionString)
        {
            List<Staff> lst = new List<Staff>();
            try
            {
                SqlConnection con = new SqlConnection(connectionString);
                string query = "SELECT s.StaffId FROM STAFF s WHERE NOT EXISTS(SELECT ms.MSId FROM MONTHLY_SALARIES ms WHERE ms.StaffId=s.StaffId AND ms.[Month]=" + month.Number + " AND ms.[Year]=" + month.Year + " )";
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lst.Add(new Staff((int)reader[0], con.ConnectionString));
                }
                con.Close();
            }
            catch (SqlException ex)
            {
                Exception e = new Exception("Error Occured in Database processing. CodeIndex:274", ex);
                throw e;
            }
            return lst;
        }
        public static List<Staff> GetAllSetSalaryStaff(Month month, string connectionString)
        {
            List<Staff> lst = new List<Staff>();
            try
            {
                SqlConnection con = new SqlConnection(connectionString);
                string query = "SELECT StaffId FROM MONTHLY_SALARIES WHERE [Month]=" + month.Number + " AND [Year]=" + month.Year; 
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lst.Add(new Staff((int)reader[0], con.ConnectionString));
                }
                con.Close();
            }
            catch (SqlException ex)
            {
                Exception e = new Exception("Error Occured in Database processing. CodeIndex:275", ex);
                throw e;
            }
            return lst;
        }
    }
}