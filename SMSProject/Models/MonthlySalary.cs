using SMSProject.Models.HelperModels;
using System;
using System.Data.SqlClient;

namespace SMSProject.Models
{
    /// <summary>
    /// Monthly Salary register of a staff member
    /// </summary>
    public class MonthlySalary
    {
        int staffId;
        Month month;
        decimal perAbsent, salary;
        long id;
        SqlConnection con;
        SqlCommand cmd;
        string query;
        public MonthlySalary(long id, string connectionString)
        {
            con = new SqlConnection(connectionString);
            this.id = id;
            SetValues();
        }
        public long SalaryId
        {
            get
            {
                return id;
            }
        }
        public Month SalaryMonth
        {
            get
            {
                return month;
            }
        }
        public decimal Salary
        {
            get
            {
                return salary;
            }
        }
        /// <summary>
        /// Number of absents in the month of registering the salary
        /// </summary>
        public int Absents
        {
            get
            {
                int count = 0;
                DateTime startDate = new DateTime(month.Year, month.Number, 1);
                DateTime endDate = startDate.AddMonths(1).AddDays(-1);
                try
                {
                    query = "SELECT COUNT(a.AttandanceId) FROM ATTANDANCES a, STAFF_GIVES_DAILY_ATTANDANCE st WHERE a.AttandanceId=st.AttandanceId AND a.IsAbsent=1 AND (a.[Date] BETWEEN '" + startDate + "' AND '" + endDate + "') AND st.StaffId=" + staffId;
                    cmd = new SqlCommand(query, con);
                    con.Open();
                    count = (int)cmd.ExecuteScalar();
                    con.Close();
                }
                catch (SqlException ex)
                {
                    Exception e = new Exception("Error Occured in Database processing. CodeIndex:257", ex);
                    throw e;
                }
                return count;
            }
        }
        public decimal TotalSalary
        {
            get
            {
                return decimal.Subtract(Salary, decimal.Multiply(Absents, perAbsent));
            }
        }
        /// <summary>
        /// Deduction of amount on per absent
        /// </summary>
        public decimal PerAbsentDeduction
        {
            get
            {
                return perAbsent;
            }
            set
            {
                try
                {
                    query = "UPDATE MONTHLY_SALARIES SET PerAbsentDeduction=" + value + " WHERE MSId=" + id;
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
                    Exception e = new Exception("Error Occured in Database processing. CodeIndex:256", ex);
                    throw e;
                }
                perAbsent = value;
            }
        }
        private void SetValues()
        {
            try
            {
                query = "SELECT * FROM MONTHLY_SALARIES WHERE MSId=" + id;
                cmd = new SqlCommand(query, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    month = new Month()
                    {
                        Number = (int)reader[1],
                        Year = (int)reader[2]
                    };
                    staffId = (int)reader[3];
                    perAbsent = (decimal)reader[4];
                    salary = (decimal)reader[5];
                }
                con.Close();
            }
            catch (SqlException ex)
            {
                Exception e = new Exception("Error Occured in Database processing. CodeIndex:255", ex);
                throw e;
            }
        }
    }
}