using fertilizesop.BL.Models;
using fertilizesop.Interfaces.BLInterfaces;
using Microsoft.Extensions.DependencyInjection;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace fertilizesop.UI
{
    public partial class CustomerForm : Form
    {
        private int selectedCustomerId = -1;
        private bool isEditMode = false; // Track operation mode
        private readonly ICustomerBl ibl;

        public CustomerForm(ICustomerBl ibl)
        {
            InitializeComponent();
            this.ibl = ibl;
            UIHelper.StyleGridView(dataGridView2);
            paneledit.Visible = false;
            this.textBox1.TextChanged += textBox1_TextChanged;
            UIHelper.StyleGridView(dataGridView2);
            this.KeyPreview = true;
            this.KeyDown += CustomerForm_KeyDown;
            dataGridView2.CellContentClick += dataGridView2_CellContentClick;

            UIHelper.ApplyButtonStyles(dataGridView2);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if(keyData == Keys.Enter)
            {
                if(txtname.Focused)
                {
                    txtlname.Focus();
                    return true;
                }
                else if (txtlname.Focused)
                {
                    txtcontact.Focus();
                    return true;
                }
                else if (txtcontact.Focused)
                {
                    txtaddress.Focus();
                    return true;
                }
                else if (txtaddress.Focused)
                {
                    btnsave.PerformClick();
                    return true;
                }
            }
            else if (keyData == Keys.Escape)
            {
                btncancle1.PerformClick();
                return true;
            }
                return base.ProcessCmdKey(ref msg, keyData);
        }

        private void CustomerForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.R)
            {
                pictureBox1_Click(sender, e);
                e.Handled = true;
            }
            else if (e.Control && e.KeyCode == Keys.S)
            {
                if (paneledit.Visible)
                {
                    btnsave.PerformClick();
                    e.Handled = true;
                }
            }
            else if (e.Control && e.KeyCode == Keys.A)
            {
                iconButton9.PerformClick(); // Add New
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Escape)
            {
                if (paneledit.Visible)
                {
                    paneledit.Visible = false;
                    e.Handled = true;
                }
            }
            else if (e.KeyCode == Keys.Enter)
            {
                if (!paneledit.Visible && dataGridView2.Focused)
                {
                    editSelectedCustomer();
                    e.Handled = true;
                }
            }
        }
        private void editSelectedCustomer()
        {
            if (dataGridView2.CurrentRow != null)
            {
                isEditMode = true; // Set to Edit Mode
                var row = dataGridView2.CurrentRow;
                selectedCustomerId = Convert.ToInt32(row.Cells["Id"].Value);
                txtname.Text = row.Cells["first_Name"].Value?.ToString();
                txtlname.Text = row.Cells["last_name"].Value?.ToString();
                txtcontact.Text = row.Cells["phonenumber"].Value?.ToString();
                txtaddress.Text = row.Cells["Address"].Value?.ToString();

                UIHelper.RoundPanelCorners(paneledit, 20);
                UIHelper.ShowCenteredPanel(this, paneledit);
            }
        }

        private void iconButton9_Click(object sender, EventArgs e)
        {
            // Open Panel for ADDING
            isEditMode = false;
            selectedCustomerId = -1;
            ClearPanelFields();
            
            UIHelper.RoundPanelCorners(paneledit, 20);
            UIHelper.ShowCenteredPanel(this, paneledit);
            txtname.Focus();
        }

        private void ClearPanelFields()
        {
            txtname.Clear();
            txtlname.Clear();
            txtcontact.Clear();
            txtaddress.Clear();
        }

        private void load()
        {
            var list = ibl.getcustomers();
            dataGridView2.Columns.Clear();
            dataGridView2.DataSource = list.OfType<Customers>().Select(c => new { c.first_Name, c.last_name, c.phonenumber, c.Address, c.Id }).ToList();
            dataGridView2.Columns["Id"].Visible = false;
            UIHelper.AddButtonColumn(dataGridView2, "Edit", "Edit", "Edit");
        }
        private void CustomerForm_Load(object sender, EventArgs e)
        {
            load();
            dataGridView2.Focus();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string text = textBox1.Text.Trim();
            if (string.IsNullOrEmpty(text))
            {
                load();
                dataGridView2.Focus();
                return;
            }
            var list = ibl.searchcustomers(text);
            dataGridView2.Columns.Clear();
            dataGridView2.DataSource = list.OfType<Customers>().Select(c => new { c.first_Name, c.last_name, c.phonenumber, c.Address, c.Id }).ToList();
            dataGridView2.Columns["Id"].Visible = false;
            UIHelper.AddButtonColumn(dataGridView2, "Edit", "Edit", "Edit");
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            var columnName = dataGridView2.Columns[e.ColumnIndex].Name;
            var row = dataGridView2.Rows[e.RowIndex];
            selectedCustomerId = Convert.ToInt32(row.Cells["id"].Value);

            if (columnName == "Edit")
            {
                isEditMode = true;
                txtname.Text = row.Cells["first_name"].Value?.ToString();
                txtlname.Text = row.Cells["last_name"].Value?.ToString();
                txtcontact.Text = row.Cells["phonenumber"].Value?.ToString();
                txtaddress.Text = row.Cells["Address"].Value?.ToString();

                UIHelper.RoundPanelCorners(paneledit, 20);
                UIHelper.ShowCenteredPanel(this, paneledit);
            }
        }

        private void btnsave_Click(object sender, EventArgs e)
        {
            string name = txtname.Text.Trim();
            string lname = txtlname.Text.Trim();
            string phone = txtcontact.Text.Trim();
            string address = txtaddress.Text.Trim();

            if(string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("First name is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var customer = new Customers(selectedCustomerId, name, phone, address, lname);
                bool result = false;

                if (isEditMode)
                {
                    // UPDATE Logic
                    result = ibl.update(customer);
                    if(result) MessageBox.Show("Customer updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else MessageBox.Show("Failed to update customer.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    // ADD Logic
                    result = ibl.Addcustomer(customer);
                    if (result) MessageBox.Show("Customer added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else MessageBox.Show("Failed to add customer.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                if (result)
                {
                    ClearPanelFields();
                    paneledit.Visible = false;
                    load();
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Database error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            load();
            dataGridView2.Focus();
        }
        private void btncancle1_Click(object sender, EventArgs e)
        {
            paneledit.Visible = false;
        }

        private void paneledit_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
