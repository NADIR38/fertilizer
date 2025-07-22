using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using KIMS;
using MySql.Data.MySqlClient;
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
            try
            {
                using (var con = DatabaseHelper.Instance.GetConnection())
                {
                    con.Open();
                    string query = @"INSERT INTO customerbills (CustomerID, SaleDate, total_price, paid_amount) 
                 VALUES (@id, @date, @total_amount, @paid_amount);
                 SELECT LAST_INSERT_ID();";
                    int billid;
                    using (MySqlCommand cmd = new MySqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@id", id ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@date", date ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@total_amount", total_amount ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@paid_amount", paid_amount ?? (object)DBNull.Value);
                        object result = cmd.ExecuteScalar();
                        billid = result != null && int.TryParse(result.ToString(), out int tempBillId) ? tempBillId : -1;
                    }

                    string query2 = "insert into customerpricerecord (customer_id ,BillID , date, payment) values (@c_id , @b_id, @date, @payment)";
                    using(MySqlCommand cmd2 = new MySqlCommand(query2, con))
                    {
                        cmd2.Parameters.AddWithValue("@c_id", id ?? (object)DBNull.Value);
                        cmd2.Parameters.AddWithValue("@b_id", billid );
                        cmd2.Parameters.AddWithValue("@date", date ?? (object)DBNull.Value);
                        cmd2.Parameters.AddWithValue("@payment", paid_amount ?? (object)DBNull.Value);
                        cmd2.ExecuteNonQuery();
                        return true;
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error in saving data to database: " + e.Message);
                return false;
            }
        }
    }
}
