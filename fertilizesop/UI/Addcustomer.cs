
﻿using fertilizesop.BL.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using fertilizesop.Interfaces.BLInterfaces;
namespace fertilizesop.UI
{
    public partial class AddCustomer : Form
    {
        private readonly ICustomerBl ibl;
        public AddCustomer(ICustomerBl ibl)
        {
            InitializeComponent();
            this.ibl = ibl;
        }

        private void btnsave_Click(object sender, EventArgs e)
        {
            string fname = txtname.Text.Trim();
            string last_name = txtlname.Text.Trim();
            string address = txtaddress.Text.Trim();
            string phone_number = txtcontact.Text.Trim();
            try
            {
                var customers = new Customers(0, fname, phone_number, address, last_name);
                var result = ibl.Addcustomer(customers);
                if (result)
                {
                    MessageBox.Show("Customer added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtname.Clear();
                    txtaddress.Clear();
                    txtcontact.Clear();
                    txtlname.Clear();
                }
                else
                {
                    MessageBox.Show("Failed to add customer. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (ArgumentNullException ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Database error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An unexpected error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}