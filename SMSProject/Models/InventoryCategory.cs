using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace SMSProject.Models
{
    public class InventoryCategory
    {
        int id;
        string name;
        SqlConnection con;
        SqlCommand cmd;
        string query;
        public InventoryCategory(int id, string connectionString)
        {
            con = new SqlConnection(connectionString);
            this.id = id;
            SetValues();
        }
        public InventoryCategory(string name, string connectionString)
        {
            con = new SqlConnection(connectionString);
            try
            {
                query = "INSERT INTO [dbo].[INVENTORY_CATEGORIES] ([Name]) VALUES ('" + name + "'); SELECT MAX(CategoryId) FROM INVENTORY_CATEGORIES;";
                cmd = new SqlCommand(query, con);
                con.Open();
                id = (int)cmd.ExecuteScalar();
                con.Close();
            }
            catch (SqlException ex)
            {
                Exception e = new Exception("Error Occured in Database processing. CodeIndex:188", ex);
                throw e;
            }
            this.name = name;
        }
        public int CategoryId
        {
            get
            {
                return id;
            }
        }
        public string CategoryName
        {
            get
            {
                return name;
            }
            set
            {
                try
                {
                    query = "UPDATE INVENTORY_CATEGORIES SET [Name]='" + value + "' WHERE CategoryId=" + id;
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
                    Exception e = new Exception("Error Occured in Database processing. CodeIndex:191", ex);
                    throw e;
                }
            }
        }
        public List<InventoryItem> GetItems()
        {
            List<InventoryItem> lst = new List<InventoryItem>();
            try
            {
                query = "SELECT ItemId FROM INVENTORY_ITEMS WHERE CategoryId=" + id;
                cmd = new SqlCommand(query, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lst.Add(new InventoryItem((int)reader[0], con.ConnectionString));
                }
                con.Close();
            }
            catch (SqlException ex)
            {
                Exception e = new Exception("Error Occured in Database processing. CodeIndex:189", ex);
                throw e;
            }
            return lst;
        }
        private void SetValues()
        {
            try
            {
                query = "SELECT [Name] FROM INVENTORY_CATEGORIES WHERE CategoryId=" + id;
                cmd = new SqlCommand(query, con);
                con.Open();
                name = (string)cmd.ExecuteScalar();
                con.Close();
            }
            catch (SqlException ex)
            {
                Exception e = new Exception("Error Occured in Database processing. CodeIndex:187", ex);
                throw e;
            }
        }
        public static List<InventoryCategory> GetAllCategories(string connectionString)
        {
            SqlConnection con = new SqlConnection(connectionString);
            List<InventoryCategory> lst = new List<InventoryCategory>();
            try
            {
                string query = "SELECT CategoryId FROM INVENTORY_CATEGORIES";
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lst.Add(new InventoryCategory((int)reader[0], con.ConnectionString));
                }
                con.Close();
            }
            catch (SqlException ex)
            {
                Exception e = new Exception("Error Occured in Database processing. CodeIndex:190", ex);
                throw e;
            }
            return lst;
        }
    }
}