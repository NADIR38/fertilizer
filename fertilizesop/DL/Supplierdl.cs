using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using fertilizesop.BL.Models;
using fertilizesop.Interfaces.DLinterfaces;
using KIMS;
using MySql.Data.MySqlClient;

namespace fertilizesop.DL
{
    internal class Supplierdl : Isupplierdl
    {
        public bool addsupplier(Suppliers s)
        {
            try
            {
                using(var con = DatabaseHelper.Instance.GetConnection())
                {
                    con.Open();
                    string query = "insert into suppliers (name , phone, address) values (@name, @phone, @address)";
                    using(var cmd = new MySqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("name", s.first_Name);
                        cmd.Parameters.AddWithValue("phone", s.phonenumber);
                        cmd.Parameters.AddWithValue("address", s.Address);
                        int id = cmd.ExecuteNonQuery();
                        return id > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding supplier " + ex.Message);
            }
        }

        public bool deletesupplier(Suppliers s)
        {
            throw new NotImplementedException();
        }

        public List<Suppliers> getsupplier()
        {
            throw new NotImplementedException();
        }

        public List<Suppliers> searchsupplier(string text)
        {
            throw new NotImplementedException();
        }

        public bool updatesupplier(Suppliers s)
        {
            throw new NotImplementedException();
        }
    }
}
