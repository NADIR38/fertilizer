using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using KIMS;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Common;
using Newtonsoft.Json;

namespace fertilizesop.DL
{
    public class Customersaledl
    {
        public DataTable getproductthings(string text)
        {
            DataTable dt = new DataTable();
            using (var con = DatabaseHelper.Instance.GetConnection())
            {
                con.Open();
                string query = "Select * from products where name like @text";
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@text" , "%" + text + "%");
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                    {

                        adapter.Fill(dt);
                    }
                }
            }
            return dt;
        }

        public DataTable getallcustomer(string text)
        {
            DataTable dt = new DataTable();
            using(var con = DatabaseHelper.Instance.GetConnection())
            {
                con.Open();
                string query = "SELECT CONCAT(first_name, ' ', last_name) as name, address, phone FROM customers WHERE CONCAT(first_name, ' ', last_name) LIKE @text";

                using (MySqlCommand cmd = new MySqlCommand( query, con))
                {
                    cmd.Parameters.AddWithValue("@text" , "%" + text + "%");
                    using(MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                }
            }
            return dt;
        }

        public int getcustomerid(string text)
        {
            try
            {
                using(var con = DatabaseHelper.Instance.GetConnection())
                {
                    con.Open();
                    string query = "select customer_id from customers where concat(first_name,' ',last_name) = @text";
                    using(MySqlCommand cmd = new MySqlCommand(query , con))
                    {
                        cmd.Parameters.AddWithValue("@text", text);
                        object result = cmd.ExecuteScalar();
                        return result != null ? Convert.ToInt32(result) : -1;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to recieve customer_id " + ex);
            }
        }

        public bool SaveDataToDatabase(int? id, DateTime? date, int? total_amount, int? paid_amount, DataGridView d)
        {
            using (var con = DatabaseHelper.Instance.GetConnection())
            {
                        con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                        string query = @"INSERT INTO customerbills (CustomerID, SaleDate, total_price, paid_amount) 
                        VALUES (@id, @date, @total_amount, @paid_amount);
                        SELECT LAST_INSERT_ID();";
                        int billid;
                        using (MySqlCommand cmd = new MySqlCommand(query, con, tran))
                        {
                            cmd.Parameters.AddWithValue("@id", id ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@date", date ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@total_amount", total_amount ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@paid_amount", paid_amount ?? (object)DBNull.Value);
                            object result = cmd.ExecuteScalar();
                            billid = result != null && int.TryParse(result.ToString(), out int tempBillId) ? tempBillId : -1;
                        }

                        string query2 = "insert into customerpricerecord (customer_id ,BillID , date, payment) values (@c_id , @b_id, @date, @payment)";
                        using (MySqlCommand cmd2 = new MySqlCommand(query2, con, tran))
                        {
                            cmd2.Parameters.AddWithValue("@c_id", id ?? (object)DBNull.Value);
                            cmd2.Parameters.AddWithValue("@b_id", billid);
                            cmd2.Parameters.AddWithValue("@date", date ?? (object)DBNull.Value);
                            cmd2.Parameters.AddWithValue("@payment", paid_amount ?? (object)DBNull.Value);
                            cmd2.ExecuteNonQuery();
                        }

                        foreach (DataGridViewRow row in d.Rows)
                        {
                            int productid;
                            string name = row.Cells["name"]?.Value?.ToString()?.Trim();
                            string description = row.Cells["description"]?.Value?.ToString()?.Trim();

                            string productidquery = "select product_id from products where name = @name and description = @description";
                            using (MySqlCommand command2 = new MySqlCommand(productidquery, con, tran))
                            {
                                command2.Parameters.AddWithValue("@name", name ?? (object)DBNull.Value);
                                command2.Parameters.AddWithValue("@description", description ?? (object)DBNull.Value);
                                object result = command2.ExecuteScalar();
                                productid = result != null && int.TryParse(result.ToString(), out int tempBillId) ? tempBillId : -1;
                            }

                            if(productid < 0)
                            {
                                throw new Exception("product id not found");
                            }
                            string detailquery = "insert into customer_bill_details (Bill_id, product_id, quantity, discount) values (@bill_iid, @product_id, @quantity, @discount)";
                            int billdetailid;
                            using(MySqlCommand command = new MySqlCommand(detailquery, con, tran))
                            {
                                command.Parameters.AddWithValue("@bill_iid", billid);
                                command.Parameters.AddWithValue("@product_id", productid);
                                if (!int.TryParse(row.Cells["quantity"].Value?.ToString(), out int qty))
                                    throw new Exception("Invalid quantity for product.");
                                command.Parameters.AddWithValue("@quantity", qty);
                                int.TryParse(row.Cells["Discount"].Value?.ToString(), out int discount);
                                command.Parameters.AddWithValue("@discount", discount);
                                object result = command.ExecuteScalar();
                                billdetailid = result != null && int.TryParse(result.ToString(), out int tempId) ? tempId : -1;                               
                            }

                            string queryupdatequantity = "UPDATE products SET quantity = quantity - @quantitysold WHERE product_id = @product_id AND quantity >= @quantitysold";
                            using(MySqlCommand comma = new MySqlCommand(queryupdatequantity, con, tran))
                            {
                                comma.Parameters.AddWithValue("@product_id", productid);
                                int quantity = Convert.ToInt32( row.Cells["quantity"]?.Value?.ToString());
                                comma.Parameters.AddWithValue("@quantitysold", quantity);
                                comma.ExecuteNonQuery();
                            }

                            string q = "insert into inventory_log (product_id, change_type, quantity_change, log_date) values (@p_id, @type, @quantity_changed, @date)";
                            using(MySqlCommand com = new MySqlCommand(q, con, tran))
                            {
                                com.Parameters.AddWithValue("@p_id", productid);
                                com.Parameters.AddWithValue("@type", "sale");
                                if (!int.TryParse(row.Cells["quantity"].Value?.ToString(), out int qty))
                                    throw new Exception("Invalid quantity for product.");
                                com.Parameters.AddWithValue("@quantity_changed", qty);
                                com.Parameters.AddWithValue("@date", date ?? (object)DBNull.Value);
                                com.ExecuteNonQuery();

                            }
                        }
                        tran.Commit();  
                        return true;
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("Error in saving data to database: " + e.Message);
                        tran.Rollback();
                        return false;
                    }
                }
            } 
        }
    }
}
