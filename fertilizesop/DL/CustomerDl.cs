using fertilizesop.BL.Models;
using KIMS;
using MySql.Data.MySqlClient;
using Mysqlx.Crud;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace fertilizesop.DL
{
    public class CustomerDl : ICustomerDl
    {
        public bool Addcustomer(Customers s)
        {
            try
            {
                using (var conn = DatabaseHelper.Instance.GetConnection())
                {
                    conn.Open();
                    string query = "insert into customers(first_name,last_name,phone,address)values(@first_name,@last_name,@phone,@address);";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@first_name", s.first_Name);
                        cmd.Parameters.AddWithValue("@last_name", s.last_name);
                        cmd.Parameters.AddWithValue("@phone", s.phonenumber);
                        cmd.Parameters.AddWithValue("@address", s.Address);
                        int rowsaffected = cmd.ExecuteNonQuery();
                        return rowsaffected > 0;
                    }



                }

            }
            catch (Exception ex)
            {
                throw new Exception("error in dl " + ex.Message);
            }
        }
        public List<Isupplier> getcustomers()
        {
            var customers = new List<Isupplier>();
            try
            {
                using (var conn = DatabaseHelper.Instance.GetConnection())
                {

                    conn.Open();
                    string query = "SELECT * FROM customers;";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {

                                int id = reader.GetInt32("customer_id");
                                string address = reader.IsDBNull(reader.GetOrdinal("address")) ? "" : reader.GetString("address");

                                string first_name = reader.GetString("first_name");
                                string last = reader.GetString("last_name");

                                string phone = reader.IsDBNull(reader.GetOrdinal("phone")) ? "" : reader.GetString("phone");
                                Customers c = new Customers(id, first_name, phone, address, last);

                                customers.Add(c);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving customers: " + ex.Message, ex);
            }
            return customers;
        }
        public bool update(Customers c)
        {
            try
            {
                using (var conn = DatabaseHelper.Instance.GetConnection())
                {
                    conn.Open();
                    string query = "UPDATE customers SET first_name = @first_name, last_name = @last_name, phone = @phone, address = @address WHERE customer_id = @id;";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@first_name", c.first_Name);
                        cmd.Parameters.AddWithValue("@last_name", c.last_name);
                        cmd.Parameters.AddWithValue("@phone", c.phonenumber);
                        cmd.Parameters.AddWithValue("@address", string.IsNullOrEmpty(c.Address) ? (object)DBNull.Value : c.Address);
                        cmd.Parameters.AddWithValue("@id", c.Id);
                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (MySqlException ex)
            {
                throw new Exception("Database error occurred while updating customer: " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating customer: " + ex.Message, ex);
            }
        }
        public List<Isupplier> searchcustomers(string text)
        {
            var customers = new List<Isupplier>();
            try
            {
                using (var conn = DatabaseHelper.Instance.GetConnection())
                {
                    conn.Open();
                    string query = @"
                SELECT * FROM customers 
                WHERE first_name LIKE @keyword 
                   OR last_name LIKE @keyword 
or phone like @keyword;
                   
            ";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@keyword", $"{text}%");
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {

                                int id = reader.GetInt32("customer_id");
                                string address = reader.IsDBNull(reader.GetOrdinal("address")) ? "" : reader.GetString("address");

                                string first_name = reader.GetString("first_name");
                                string last = reader.GetString("last_name");

                                string phone = reader.IsDBNull(reader.GetOrdinal("phone")) ? "" : reader.GetString("phone");
                                Customers c = new Customers(id, first_name, phone, address, last);

                                customers.Add(c);
                            }
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                throw new Exception(ex.Message, ex);
            }
            return customers;
        }
    }
}
