using SMSProject.Models.HelperModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace SMSProject.Models
{
    /// <summary>
    /// Record of monthly fee for student
    /// </summary>
    public class StudentMonthlyFee
    {
        long id;
        Month feeMonth;
        decimal fine;
        int studentId;
        SqlConnection con;
        SqlCommand cmd;
        string query;
        public StudentMonthlyFee(long id, string connectionString)
        {
            con = new SqlConnection(connectionString);
            this.id = id;
            SetValues();
        }
        public StudentMonthlyFee(decimal fine, DateTime month, int studentId, string connectionString)
        {
            con = new SqlConnection(connectionString);
            try
            {
                query = "INSERT INTO [dbo].[STUDENT_MONTHLY_FEE] ([Fine] ,[Month] ,[Year] ,[StudentId]) VALUES (" + fine + " ," + month.Month + " ," + month.Year + " ," + studentId + "); SELECT MAX(SFeeId) FROM STUDENT_MONTHLY_FEE;";
                cmd = new SqlCommand(query, con);
                con.Open();
                id = (long)cmd.ExecuteScalar();
                con.Close();
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2601)
                {
                    throw new Exception("Record already exists. CodeIndex:143", ex);
                }
                Exception e = new Exception("Error Occured in Database processing. CodeIndex:193", ex);
                throw e;
            }
            this.fine = fine;
            feeMonth = new Month()
            {
                Year = month.Year,
                Number = month.Month
            };
            this.studentId = studentId;
        }
        public long FeeId
        {
            get
            {
                return id;
            }
        }

        public Student Student
        {
            get
            {
                return new Student(studentId, con.ConnectionString);
            }
        }
        public Month FeeMonth
        {
            get
            {
                return feeMonth;
            }
        }
        public decimal Fine
        {
            get
            {
                return fine;
            }
            set
            {
                try
                {
                    query = "UPDATE STUDENT_MONTHLY_FEE SET Fine=" + value + " WHERE SFeeId=" + id;
                    cmd = new SqlCommand(query, con);
                    con.Open();
                    if (cmd.ExecuteNonQuery() == 0)
                    {
                        throw new Exception("Nothing has updated in the database. CodeIndex:108");
                    }
                    con.Close();
                    fine = value;
                }
                catch (SqlException ex)
                {
                    Exception e = new Exception("Error Occured in Database processing. CodeIndex:194", ex);
                    throw e;
                }
            }
        }
        public decimal ItemsPurchasedAmount
        {
            get
            {
                decimal total = 0;
                DateTime startDate = new DateTime(feeMonth.Year, feeMonth.Number, 1);
                DateTime endDate = startDate.AddMonths(1).AddDays(-1);
                try
                {
                    query = "SELECT SUM(sp.PurchasedPrice*sp.Quantity) FROM STUDENT_PURCHASES_ITEMS sp WHERE sp.StudentId=" + studentId + " AND (sp.[Date] BETWEEN '" + startDate + "' AND '" + endDate + "')";
                    cmd = new SqlCommand(query, con);
                    con.Open();
                    total = (decimal)cmd.ExecuteScalar();
                    con.Close();
                }
                catch (SqlException ex)
                {
                    Exception e = new Exception("Error Occured in Database processing. CodeIndex:196", ex);
                    throw e;
                }
                return total;
            }
        }
        public decimal AdditionalFundsTotal
        {
            get
            {
                decimal total = 0;
                DateTime startDate = new DateTime(feeMonth.Year, feeMonth.Number, 1);
                DateTime endDate = startDate.AddMonths(1).AddDays(-1);
                try
                {
                    query = "SELECT SUM(a.Amount)  FROM ADDITIONAL_FUNDS a, ADDITIONAL_FUND_ADDS_INTO_MONTHLY_FEE af WHERE a.FundId=af.FundId AND af.SFeeId=" + id;
                    cmd = new SqlCommand(query, con);
                    con.Open();
                    try
                    {
                        total = (decimal)cmd.ExecuteScalar();
                    }
                    catch (InvalidCastException)
                    {
                        total = 0;
                    }
                    con.Close();
                }
                catch (SqlException ex)
                {
                    Exception e = new Exception("Error Occured in Database processing. CodeIndex:269", ex);
                    throw e;
                }
                return total;
            }
        }
        public decimal TuitionFee
        {
            get
            {
                return Student.MonthlyFee;
            }
        }
        public decimal TotalFee
        {
            get
            {
                return ItemsPurchasedAmount + TuitionFee + Fine + AdditionalFundsTotal;
            }
        }
        public void AddAdditionalFunds(int additionalFundId)
        {
            try
            {
                query = "INSERT INTO [dbo].[ADDITIONAL_FUND_ADDS_INTO_MONTHLY_FEE] ([FundId] ,[SFeeId]) VALUES (" + additionalFundId + " ," + id + ")";
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
                if (ex.Number == 2627)
                {
                    throw new Exception("Additional fund already added. CodeIndex:143");
                }
                Exception e = new Exception("Error Occured in Database processing. CodeIndex:195", ex);
                throw e;
            }
        }
        public List<AdditionalFund> GetAdditionalFunds()
        {
            List<AdditionalFund> lst = new List<AdditionalFund>();
            try
            {
                query = "SELECT FundId FROM ADDITIONAL_FUND_ADDS_INTO_MONTHLY_FEE WHERE SFeeId=" + id;
                cmd = new SqlCommand(query, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lst.Add(new AdditionalFund((int)reader[0], con.ConnectionString));
                }
                con.Close();
            }
            catch (SqlException ex)
            {
                Exception e = new Exception("Error Occured in Database processing. CodeIndex:197", ex);
                throw e;
            }
            return lst;
        }
        private void SetValues()
        {
            try
            {
                query = "SELECT * FROM STUDENT_MONTHLY_FEE WHERE SFeeId=" + id;
                cmd = new SqlCommand(query, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    fine = (decimal)reader[1];
                    feeMonth = new Month()
                    {
                        Number = (int)reader[2],
                        Year = (int)reader[3]
                    };
                    studentId = (int)reader[4];
                }
                con.Close();
            }
            catch (SqlException ex)
            {
                Exception e = new Exception("Error Occured in Database processing. CodeIndex:192", ex);
                throw e;
            }
        }
    }
}