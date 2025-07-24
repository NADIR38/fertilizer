using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KIMS;
using MySql.Data.MySqlClient;
using fertilizesop.BL.Models;

namespace fertilizesop.DL
{
    public class TransactionDAL
    {
        private MySqlConnection conn => DatabaseHelper.Instance.GetConnection();

        public bool InsertTransaction(Transaction t)
        {
            using (var connection = conn)
            {
                string query = @"INSERT INTO transaction_history (transaction_type, amount, transaction_date, description, remaining_balance)
                             VALUES (@type, @amount, @date, @desc, @balance)";

                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@type", t.TransactionType);
                cmd.Parameters.AddWithValue("@amount", t.Amount);
                cmd.Parameters.AddWithValue("@date", t.TransactionDate);
                cmd.Parameters.AddWithValue("@desc", t.Description);
                cmd.Parameters.AddWithValue("@balance", t.RemainingBalance);

                connection.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public decimal GetLatestBalance()
        {
            using (var connection = conn)
            {
                string query = "SELECT remaining_balance FROM transaction_history ORDER BY transaction_id DESC LIMIT 1";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                connection.Open();
                var result = cmd.ExecuteScalar();
                return result != null ? Convert.ToDecimal(result) : 0;
            }
        }

        public DataTable GetAllTransactions()
        {
            using (var connection = conn)
            {
                string query = "SELECT * FROM transaction_history ORDER BY transaction_date DESC";
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }

        public DataTable SearchTransactions(string keyword)
        {
            using (var connection = conn)
            {
                string query = @"SELECT * FROM transaction_history 
                             WHERE transaction_type LIKE @kw OR 
                                   description LIKE @kw OR 
                                   amount LIKE @kw OR 
                                   remaining_balance LIKE @kw";

                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@kw", "%" + keyword + "%");

                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }
    }

}
