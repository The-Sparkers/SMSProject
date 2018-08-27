using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace SMSProject.Models
{
    public class ParentFeeRecord
    {
        DateTime datePaid;
        long id;
        decimal concession, totalAmountDue, amountPaid;
        int parentId;
        SqlConnection con;
        SqlCommand cmd;
        string query;
        public ParentFeeRecord(long id, string connectionString)
        {
            con = new SqlConnection(connectionString);
            this.id = id;
            SetValues();
        }
        public ParentFeeRecord(DateTime datePaid, decimal amountDue, decimal concession, decimal amountPaid, Parent parent, string connectionString)
        {
            con = new SqlConnection(connectionString);
            try
            {
                query = "INSERT INTO [PARENT_MONTHLY_FEE] ([DatePaid] ,[AmountPaid] ,[Concession] ,[ParentId] ,[TotalAmountDue]) VALUES ('" + datePaid + "' ," + amountPaid + " ," + concession + " ," + parent.ParentId + " ," + amountDue + ");Select MAX(PFeeId) from PARENT_MONTHLY_FEE ";
                cmd = new SqlCommand(query, con);
                con.Open();
                id = (long)cmd.ExecuteScalar();
                con.Close();
            }
            catch (SqlException ex)
            {
                Exception e = new Exception("Error Occured in Database processing. CodeIndex:120", ex);
                throw e;
            }
            this.amountPaid = amountPaid;
            this.concession = concession;
            this.datePaid = datePaid;
            parentId = parent.ParentId;
            totalAmountDue = amountDue;
        }
        public long ParentFeeId
        {
            get
            {
                return id;
            }
        }
        public DateTime DatePaid
        {
            get
            {
                return datePaid;
            }
        }
        public decimal Concession
        {
            get
            {
                return concession;
            }
            set
            {
                try
                {
                    query = "update PARENT_MONTHLY_FEE set Concession=" + value + " where PFeeId=" + id; 
                    con.Open();
                    cmd = new SqlCommand(query, con);
                    if (cmd.ExecuteNonQuery() == 0)
                    {
                        throw new Exception("Nothing has updated in the database. CodeIndex:108");
                    }
                    con.Close();
                    concession = value;
                }
                catch (SqlException ex)
                {
                    Exception e = new Exception("Error Occured in Database processing. CodeIndex:121", ex);
                    throw e;
                }
            }
        }
        public decimal TotalAmountDue
        {
            get
            {
                return totalAmountDue;
            }
        }
        public decimal AmountPaid
        {
            get
            {
                return amountPaid;
            }
        }
        public decimal Balance
        {
            get
            {
                return -(totalAmountDue - amountPaid - concession);
            }
        }
        public Parent Parent
        {
            get
            {
                return new Parent(parentId, con.ConnectionString);
            }
        }
        private void SetValues()
        {
            try
            {
                query = "select * from PARENT_MONTHLY_FEE where PFeeId=" + id;
                cmd = new SqlCommand(query, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    datePaid = (DateTime)reader[1];
                    amountPaid = (decimal)reader[2];
                    concession = (decimal)reader[3];
                    parentId = (int)reader[4];
                    totalAmountDue = (decimal)reader[5];
                }
                con.Close();
            }
            catch (SqlException ex)
            {
                Exception e = new Exception("Error Occured in Database processing. CodeIndex:127", ex);
                throw e;
            }
        }
        public static decimal GetTotalCollection(DateTime month, string connectionString)
        {
            SqlConnection con = new SqlConnection(connectionString);
            decimal total=0;
            DateTime startDate = new DateTime(month.Year, month.Month, 1);
            DateTime endDate = startDate.AddMonths(1).AddDays(-1);
            try
            {
                string query = "Select SUM(AmountPaid) from PARENT_MONTHLY_FEE where DatePaid between '" + startDate + "' and '" + endDate + "'";
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                total = (decimal)cmd.ExecuteScalar();
                con.Close();
            }
            catch (SqlException ex)
            {
                Exception e = new Exception("Error Occured in Database processing. CodeIndex:122", ex);
                throw e;
            }
            return total;
        }
        public static decimal GetTotalDue(DateTime month, string connectionString)
        {
            SqlConnection con = new SqlConnection(connectionString);
            decimal total = 0;
            DateTime startDate = new DateTime(month.Year, month.Month, 1);
            DateTime endDate = startDate.AddMonths(1).AddDays(-1);
            try
            {
                string query = "GetTotalDue";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@monthStart", System.Data.SqlDbType.Date)).Value = startDate;
                cmd.Parameters.Add(new SqlParameter("@monthEnd", System.Data.SqlDbType.Date)).Value = endDate;
                con.Open();
                total = (decimal)cmd.ExecuteScalar();
                con.Close();
            }
            catch (SqlException ex)
            {
                Exception e = new Exception("Error Occured in Database processing. CodeIndex:123", ex);
                throw e;
            }
            return total;
        }
        public static List<ParentFeeRecord> GetAllMonthRecord(DateTime month, string connectionString)
        {
            List<ParentFeeRecord> lst = new List<ParentFeeRecord>();
            SqlConnection con = new SqlConnection(connectionString);
            DateTime startDate = new DateTime(month.Year, month.Month, 1);
            DateTime endDate = startDate.AddMonths(1).AddDays(-1);
            try
            {
                string query = "Select PFeeId from PARENT_MONTHLY_FEE where DatePaid between '" + startDate + "' and '" + endDate + "'";
                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataReader reader = cmd.ExecuteReader();
                con.Open();
                lst.Add(new ParentFeeRecord((long)reader[0], connectionString));
                con.Close();
            }
            catch (SqlException ex)
            {
                Exception e = new Exception("Error Occured in Database processing. CodeIndex:124", ex);
                throw e;
            }
            return lst;
        }
    }
}