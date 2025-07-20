using fertilizesop.BL.Bl;
using fertilizesop.BL.Models;
using KIMS;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace fertilizesop.DL
{
    public class ProductsDl : IProductsDl
    {
        public bool Addproduct(Products p)
        {
            try
            {
                using (var conn = DatabaseHelper.Instance.GetConnection())
                {
                    conn.Open();
                    string query = "insert into products(name,sale_price,description,quantity) values(@name,@sale,@descp,@quantity);";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@name", p.Name);
                        cmd.Parameters.AddWithValue("@sale", p.Price);
                        cmd.Parameters.AddWithValue("@descp", p.Description);
                        cmd.Parameters.AddWithValue("@quantity", p.quantity);
                        int rowsaffected = cmd.ExecuteNonQuery();
                        return rowsaffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding products " + ex.Message);
            }

        }
        public List<Products> GetProducts()
        {
            var list = new List<Products>();
            try
            {
                using (var conn = DatabaseHelper.Instance.GetConnection())
                {
                    conn.Open();
                    string query = "select * from products;";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {

                                int id = reader.GetInt32("product_id");
                                string descp = reader.IsDBNull(reader.GetOrdinal("description")) ? "" : reader.GetString("description");

                                string name = reader.GetString("name");
                                decimal sale_price = reader.GetDecimal("sale_price");
                                int quantity = reader.GetInt32("quantity");
                                var products = new Products(id, name, descp, sale_price, quantity);
                                list.Add(products);


                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting products " + ex.Message);
            }
            return list;
        }
        public List<Products> searchproducts(string text)
        {
            var list = new List<Products>();
            try
            {
                using (var conn = DatabaseHelper.Instance.GetConnection())
                {
                    conn.Open();
                    string query = "select * from products where name like @name;";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            cmd.Parameters.AddWithValue("@name", $"{text}%");

                            while (reader.Read())
                            {

                                int id = reader.GetInt32("product_id");
                                string descp = reader.IsDBNull(reader.GetOrdinal("description")) ? "" : reader.GetString("description");

                                string name = reader.GetString("name");
                                decimal sale_price = reader.GetDecimal("sale_price");
                                int quantity = reader.GetInt32("quantity");
                                var products = new Products(id, name, descp, sale_price, quantity);
                                list.Add(products);


                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting products " + ex.Message);
            }
            return list;
        }
        public bool update(Products c)
        {
            try
            {
                using (var conn = DatabaseHelper.Instance.GetConnection())
                {
                    conn.Open();
                    string query = "UPDATE products SET name = @name, description = @descp, sale_price = @price, quantity = @quantity WHERE product_id = @id;";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@name", c.Name);
                        cmd.Parameters.AddWithValue("@descp", c.Description);
                        cmd.Parameters.AddWithValue("@price", c.Price);
                        cmd.Parameters.AddWithValue("@quantity", c.quantity);
                        cmd.Parameters.AddWithValue("@id", c.Id);
                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (MySqlException ex)
            {
                throw new Exception("Database error occurred while updating Products: " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating products: " + ex.Message, ex);
            }
        }
    }
}
