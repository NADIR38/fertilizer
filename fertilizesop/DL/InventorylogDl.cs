using fertilizesop.BL.Models;
using KIMS;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fertilizesop.DL
{
    public class InventorylogDl : IInventorylogDl
    {
        public List<inventorylog> getlog()
        {
            try
            {
                using (var conn = DatabaseHelper.Instance.GetConnection())
                {
                    conn.Open();
                    string query = "select p.name,i.quantity_change,i.log_date,i.change_type,i.remarks from inventory_log i join products p on p.product_id=i.product_id;";
                    {
                        using (var cmd = new MySqlCommand(query, conn))
                        {
                            var list = new List<inventorylog>();
                            using (var reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    var logs = new inventorylog(
                                        reader.GetString("name"),
                                        reader.GetInt32("quantity_change"),
                                        reader.GetDateTime("log_date"),
                                        reader.GetString("change_type"),
                                        reader.GetString("remarks"));
                                    list.Add(logs);
                                }
                            }
                            return list;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("error" + ex.Message);
            }
        }
        public List<inventorylog> getlog(string searchTerm)
        {
            try
            {
                using (var conn = DatabaseHelper.Instance.GetConnection())
                {
                    conn.Open();
                    string query = @"
                SELECT p.name, i.quantity_change, i.log_date, i.change_type, i.remarks 
                FROM inventory_log i 
                JOIN products p ON p.product_id = i.product_id 
                WHERE 
                    p.name LIKE @search OR 
                    i.change_type LIKE @search OR 
                    DATE_FORMAT(i.log_date, '%Y-%m-%d') LIKE @search;";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@search", "%" + searchTerm + "%");

                        var list = new List<inventorylog>();
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var logs = new inventorylog(
                                    reader.GetString("name"),
                                    reader.GetInt32("quantity_change"),
                                    reader.GetDateTime("log_date"),
                                    reader.GetString("change_type"),
                                    reader.GetString("remarks"));
                                list.Add(logs);
                            }
                        }
                        return list;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
        }

    }
}
