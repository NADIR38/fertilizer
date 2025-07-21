using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KIMS;
using MySql.Data.MySqlClient;

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
    }
}
