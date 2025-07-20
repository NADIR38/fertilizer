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
using Microsoft.Extensions.DependencyInjection;

namespace fertilizesop.UI
{
    public partial class supplierform : Form
    {
        private readonly Isupplierbl _customerbl;
        private int customerid = -1;
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
                var supplier = new Suppliers(customerid, fname, contact, address);
                bool result = _customerbl.updatesupplier(supplier);
                if (result)
                {
                    MessageBox.Show("supplier updated successfully", "success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    clearfields();
                    editpanel.Visible = false;
                    load();
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving the supplier " + ex.Message);
            }
        }

        private void editpanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Addbutton_Click(object sender, EventArgs e)
        {
            var form = Program.ServiceProvider.GetRequiredService<Addsupplier>();
            form.ShowDialog();
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
            if(e.ColumnIndex <0 || e.RowIndex < 0)
            {
                return;
            }
            var columnname = dataGridView1.Columns[e.ColumnIndex].Name;
            var rowname = dataGridView1.Rows[e.RowIndex];
            customerid = Convert.ToInt32(rowname.Cells["Id"].Value);
            editpanel.Visible = true;
            txtaddress.Text = rowname.Cells["address"].Value.ToString();
            txtfirstname.Text = rowname.Cells["first_name"].Value.ToString();
            txtcontact.Text = rowname.Cells["phonenumber"].Value.ToString();
            UIHelper.RoundPanelCorners(editpanel, 20);
            UIHelper.ShowCenteredPanel(this, editpanel);
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
