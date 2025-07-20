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
    public partial class supplierform : Form
    {
        private readonly Isupplierbl _customerbl;
        public supplierform(Isupplierbl customerbl)
        {
            InitializeComponent();
            _customerbl = customerbl;
            editpanel.Visible = false;
        }

        private void load()
        {
            var supplier = _customerbl.getsupplier();
            dataGridView1.Columns.Clear();
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.DataSource = supplier.OfType<Suppliers>().Select(c=> new { c.Id, c.first_Name, c.Address, c.phonenumber }).ToList();
            dataGridView1.Columns["Id"].Visible = false;
            UIHelper.AddButtonColumn(dataGridView1, "Edit", "Edit", "Edit");
            
        }
        
        

        private void txtfirstname_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtlastname_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtcontact_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtaddress_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnsave_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(txtfirstname.Text))
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
                bool result = _customerbl.addsupplier(supplier);
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
                MessageBox.Show("Error saving the supplier " +  ex.Message);
            }
        }

        private void editpanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Addbutton_Click(object sender, EventArgs e)
        {
            editpanel.Visible = true;
        }

        private void btncancel_Click(object sender, EventArgs e)
        {
            editpanel.Visible = false;
        }
        private void clearfields()
        {
            txtaddress.Clear();
            txtcontact.Clear();
            txtfirstname.Clear();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void supplierform_Load(object sender, EventArgs e)
        {
            load();
        }

        private void txtsearch_TextChanged(object sender, EventArgs e)
        {
            string text = txtsearch.Text;
            if(string.IsNullOrEmpty(text))
            {
                load();
                return;
            }
            var sup = _customerbl.searchsupplier(text);
            dataGridView1.Columns.Clear();
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; 
            dataGridView1.DataSource = sup.OfType<Suppliers>().Select(c => new { c.Id, c.first_Name, c.Address, c.phonenumber }).ToList();
            dataGridView1.Columns["Id"].Visible = false;
            UIHelper.AddButtonColumn(dataGridView1, "Edit", "Edit", "Edit");
        }
    }
}
