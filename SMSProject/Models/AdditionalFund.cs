using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace SMSProject.Models
{
    public class AdditionalFund
    {
        int id;
        string name;
        decimal amount;
        SqlConnection con;
        SqlCommand cmd;
        string query;
        public AdditionalFund(int id, string connectionString)
        {
            con = new SqlConnection(connectionString);
            this.id = id;
            SetValues();
        }
        public AdditionalFund(string name, decimal amount, string connectionString)
        {
            con = new SqlConnection(connectionString);
            try
            {
                query = "INSERT INTO [dbo].[ADDITIONAL_FUNDS] ([Name] ,[Amount]) VALUES ('" + name + "' ," + amount + "); SELECT MAX([FundId]) FROM ADDITIONAL_FUNDS;";
                cmd = new SqlCommand(query, con);
                con.Open();
                id = (int)cmd.ExecuteScalar();
                con.Close();
                this.name = name;
                this.amount = amount;
            }
            catch (SqlException ex)
            {
                Exception e = new Exception("Error Occured in Database processing. CodeIndex:199", ex);
                throw e;
            }
            this.name = name;
            this.amount = amount;
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
                    query = "UPDATE ADDITIONAL_FUNDS SET [Name]='" + value + "' WHERE FundId=" + id;
                    cmd = new SqlCommand(query, con);
                    con.Open();
                    if (cmd.ExecuteNonQuery() == 0)
                    {
                        throw new Exception("Nothing has updated in the database. CodeIndex:108");
                    }
                    con.Close();
                    name = value;
                }
                catch (SqlException ex)
                {
                    Exception e = new Exception("Error Occured in Database processing. CodeIndex:200", ex);
                    throw e;
                }
            }
        }
        public decimal Amount
        {
            get
            {
                return amount;
            }
            set
            {
                try
                {
                    query = "UPDATE ADDITIONAL_FUNDS SET [Amount]='" + value + "' WHERE FundId=" + id;
                    cmd = new SqlCommand(query, con);
                    con.Open();
                    if (cmd.ExecuteNonQuery() == 0)
                    {
                        throw new Exception("Nothing has updated in the database. CodeIndex:108");
                    }
                    con.Close();
                    amount = value;
                }
                catch (SqlException ex)
                {
                    Exception e = new Exception("Error Occured in Database processing. CodeIndex:201", ex);
                    throw e;
                }
            }
        }
        private void SetValues()
        {
            try
            {
                query = "SELECT * FROM ADDITIONAL_FUNDS WHERE FundId=" + id;
                cmd = new SqlCommand(query, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    name = (string)reader[1];
                    amount = (decimal)reader[2];
                }
                con.Close();
            }
            catch (SqlException ex)
            {
                Exception e = new Exception("Error Occured in Database processing. CodeIndex:201", ex);
                throw e;
            }
        }
        public static List<AdditionalFund> GetAllFunds(string connectionString)
        {
            SqlConnection con = new SqlConnection(connectionString);
            List<AdditionalFund> lst = new List<AdditionalFund>();
            try
            {
               string query = "SELECT FundId FROM ADDITIONAL_FUNDS";
               SqlCommand cmd = new SqlCommand(query, con);
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
                Exception e = new Exception("Error Occured in Database processing. CodeIndex:202", ex);
                throw e;
            }
            return lst;
        }
    }
}