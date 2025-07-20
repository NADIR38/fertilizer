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
using fertilizesop.Interfaces.BLInterfaces;

namespace fertilizesop.UI
{
    public partial class Addsupplier : Form
    {
        private readonly Isupplierbl _supplierbl;
        public Addsupplier(Isupplierbl supplierbl)
        {
            InitializeComponent();
            _supplierbl = supplierbl;
        }

        private void btnsave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtfirstname.Text))
            {
                MessageBox.Show("Pease enter a name first");
                return;
            }
            string fname = txtfirstname.Text;
            string contact = txtcontact.Text;
            string address = txtaddress.Text;
            try
            {
                var supplier = new Suppliers(fname, contact, address);
                bool result = _supplierbl.addsupplier(supplier);
                if (result)
                {
                    MessageBox.Show("supplier saved successfully", "success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    clearfields();
                }
                else
                {
                    MessageBox.Show("Invalid data entered");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving the supplier " + ex.Message);
            }
        }

        private void clearfields()
        {
            txtaddress.Clear();
            txtcontact.Clear();
            txtfirstname.Clear();
        }

    }
}