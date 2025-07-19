using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using fertilizesop.BL.Models;
using fertilizesop.BL.Models.persons;
using fertilizesop.Interfaces.BLinterfaces;
using MySql.Data.MySqlClient;

namespace fertilizesop.UI
{
    public partial class Addcustomer : Form
    {
        private readonly Icustomerbl _customerbl;
        public Addcustomer(Icustomerbl customerbl)
        {
            InitializeComponent();
            _customerbl = customerbl;
        }

        private void iconButton1_Click(object sender, EventArgs e)
        {
            string firstname = txtname.Text;
            string lastname = txtlastname.Text;
            string type = comboBox1.Text.Trim();
            string address = txtaddress.Text;
            string email = txtemail.Text;
            string contact = txtcontact.Text;
            try
            {
                if(string.IsNullOrWhiteSpace(firstname))
                {
                    throw new ArgumentException("Name is required.");
                }
                if (string.IsNullOrWhiteSpace(type))
                    throw new ArgumentException("Customer type must be selected.");

                else if (string.IsNullOrWhiteSpace(address))
                {
                    throw new ArgumentException("Address is required");
                }

                if(comboBox1.Text == "Regular")
                {
                    if (string.IsNullOrWhiteSpace(email))
                        throw new ArgumentException("Email is required for Regular customers.");

                    if (string.IsNullOrWhiteSpace(address))
                        throw new ArgumentException("Address is required for Regular customers.");
                }
                Ipersons p = new Customer(firstname , lastname , type, address, email, contact);
                MessageBox.Show("Customer created with ID: " + ((Customer)p).id);
                bool result = _customerbl.addcustomer(p);
                if(result)
                {
                    MessageBox.Show("Customer added successfully", "success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Error savin customer","error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void txtemail_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
