using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SMSProject.Models
{
    public class InventoryItem
    {
        decimal price;
        string name;
        int id, quantity, categoryId;
        SqlConnection con;
        SqlCommand cmd;
        string query;
        public InventoryItem(int id, string connectionString)
        {
            con = new SqlConnection(connectionString);
            this.id = id;
            SetValues();
        }
        public InventoryItem(string name, int quantity, decimal price, int categoryId, string connectionString)
        {
            con = new SqlConnection(connectionString);
            try
            {
                query = "INSERT INTO [dbo].[INVENTORY_ITEMS] ([Quantity] ,[Name] ,[Price] ,[CategoryId]) VALUES (" + quantity + " ,'" + name + "' ," + price + " ," + categoryId + ");SELECT MAX(ItemId) FROM INVENTORY_ITEMS;";
                cmd = new SqlCommand(query, con);
                con.Open();
                id = (int)cmd.ExecuteScalar();
                con.Close();
            }
            catch (SqlException ex)
            {
                Exception e = new Exception("Error Occured in Database processing. CodeIndex:182", ex);
                throw e;
            }
            this.name = name;
            this.price = price;
            this.categoryId = categoryId;
            this.quantity = quantity;
        }
        public int ItemId
        {
            get
            {
                return id;
            }
        }
        public InventoryCategory Category
        {
            get
            {
                return new InventoryCategory(categoryId, con.ConnectionString);
            }
        }
        public string Name
        {
            get
            {
                return name;
            }
        }
        public int Quantity
        {
            get
            {
                return quantity;
            }
            set
            {
                try
                {
                    query = "UPDATE INVENTORY_ITEMS SET Quantity=" + value + " WHERE ItemId=" + id;
                    cmd = new SqlCommand(query, con);
                    con.Open();
                    if (cmd.ExecuteNonQuery() == 0)
                    {
                        throw new Exception("Nothing has updated in the database. CodeIndex:108");
                    }
                    con.Close();
                    quantity = value;
                }
                catch (SqlException ex)
                {
                    Exception e = new Exception("Error Occured in Database processing. CodeIndex:183", ex);
                    throw e;
                }
            }
        }
        public decimal Price
        {
            get
            {
                return price;
            }
            set
            {
                try
                {
                    query = "UPDATE INVENTORY_ITEMS SET Price=" + value + " WHERE ItemId=" + id;
                    cmd = new SqlCommand(query, con);
                    con.Open();
                    if (cmd.ExecuteNonQuery() == 0)
                    {
                        throw new Exception("Nothing has updated in the database. CodeIndex:108");
                    }
                    con.Close();
                    price = value;
                }
                catch (SqlException ex)
                {
                    Exception e = new Exception("Error Occured in Database processing. CodeIndex:184", ex);
                    throw e;
                }
            }
        }
        public bool ItemSell(int studentId, int quantity)
        {
            bool flag = false;
            try
            {
                query = "SellItem";
                cmd = new SqlCommand(query, con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@itemId", System.Data.SqlDbType.Int)).Value = id;
                cmd.Parameters.Add(new SqlParameter("@studentId", System.Data.SqlDbType.Int)).Value = studentId;
                cmd.Parameters.Add(new SqlParameter("@quantity", System.Data.SqlDbType.Int)).Value = quantity;
                cmd.Parameters.Add(new SqlParameter("@date", System.Data.SqlDbType.DateTime)).Value = DateTime.Now;
                con.Open();
                flag = Convert.ToBoolean(cmd.ExecuteScalar());
                con.Close();
            }
            catch (SqlException ex)
            {
                Exception e = new Exception("Error Occured in Database processing. CodeIndex:185", ex);
                throw e;
            }
            return flag;
        }
        private void SetValues()
        {
            try
            {
                query = "SELECT * FROM INVENTORY_ITEMS WHERE ItemId=" + id;
                cmd = new SqlCommand(query, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    quantity = (int)reader[1];
                    name = (string)reader[2];
                    price = (decimal)reader[3];
                    categoryId = (int)reader[4];
                }
                con.Close();
            }
            catch (SqlException ex)
            {
                Exception e = new Exception("Error Occured in Database processing. CodeIndex:181", ex);
                throw e;
            }
        }
        public static List<InventoryItem> GetAllItems(string connectionString)
        {
            SqlConnection con = new SqlConnection(connectionString);
            List<InventoryItem> lst = new List<InventoryItem>();
            try
            {
                string query = "SELECT ItemId FROM INVENTORY_ITEMS";
                SqlCommand cmd = new SqlCommand(query, con);
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
                Exception e = new Exception("Error Occured in Database processing. CodeIndex:186", ex);
                throw e;
            }
            return lst;
        }
    }
}