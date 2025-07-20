using Microsoft.Extensions.DependencyInjection;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using TechStore.BL.BL;
using TechStore.BL.Models;
using TechStore.BL.Models.Person;
using TechStore.Interfaces;
using TechStore.Interfaces.BLInterfaces;
using TechStore.UI;

namespace TechStore.UI
{
    public partial class Customerform : Form
    {
        private readonly ICustomerBL _customerBL;
        private int selectedCustomerId;

        public Customerform(ICustomerBL customerBL)
        {
            InitializeComponent();
            this._customerBL = customerBL;
            paneledit.Visible = false;
            this.textBox1.TextChanged += textBox1_TextChanged;
            UIHelper.StyleGridView(dataGridView2);
            this.KeyPreview = true;
            this.KeyDown += Customerform_KeyDown;
            dataGridView2.CellContentClick += dataGridView2_CellContentClick;
            dataGridView2.KeyDown += dataGridView2_KeyDown;

            UIHelper.ApplyButtonStyles(dataGridView2);
        }
        private void Customerform_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S && paneledit.Visible)
            {
                btnsave.PerformClick(); // save edited customer
                e.SuppressKeyPress = true;
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down && dataGridView2.Rows.Count > 0)
            {
                dataGridView2.Focus();
                dataGridView2.CurrentCell = dataGridView2.Rows[0].Cells[1];
            }
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape && paneledit.Visible)
            {
                paneledit.Visible = false;
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void dataGridView2_KeyDown(object sender, KeyEventArgs e)
        {
            if (dataGridView2.CurrentRow == null) return;

            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                dataGridView2_CellContentClick(sender,
                    new DataGridViewCellEventArgs(dataGridView2.Columns["Edit"].Index, dataGridView2.CurrentRow.Index));
            }

            if (e.KeyCode == Keys.Delete)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                dataGridView2_CellContentClick(sender,
                    new DataGridViewCellEventArgs(dataGridView2.Columns["Delete"].Index, dataGridView2.CurrentRow.Index));
            }
        }

        private void Customerform_Load(object sender, EventArgs e)
        {
            LoadCustomers();
            dataGridView2.Focus();
        }

        private void LoadCustomers()
        {
            List<Customer> list = _customerBL.GetCustomers().OfType<Customer>().ToList();
            dataGridView2.Columns.Clear();
            dataGridView2.DataSource = list;

            dataGridView2.Columns["Id"].Visible = false;
            dataGridView2       .Columns["_firstname"].HeaderText = "First Name";
                    dataGridView2.Columns["_lastname"].HeaderText = "Last Name";
            dataGridView2.Columns["phone"].HeaderText = "Contact Number";
            dataGridView2.Columns["_type"].HeaderText = "Type";
            dataGridView2.Columns["email"].HeaderText = "Email";
            dataGridView2.Columns["address"].HeaderText = "Address";
                
            if (dataGridView2.Columns.Contains("_name"))
                dataGridView2.Columns["_name"].Visible = false;
         
            dataGridView2 .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // 👇 ADD BUTTONS BEFORE SETTING DISPLAY ORDER
            UIHelper.AddButtonColumn(dataGridView2, "Edit", "Edit", "Edit");
            UIHelper.AddButtonColumn(dataGridView2, "Delete", "Delete", "Delete");

            // ✅ NOW SET ORDER
       
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            var columnName = dataGridView2.Columns[e.ColumnIndex].Name;
            var row = dataGridView2.Rows[e.RowIndex];
            selectedCustomerId = Convert.ToInt32(row.Cells["id"].Value);

            if (columnName == "Edit")
            {
               
                    txtname.Text = row.Cells["_firstname"].Value?.ToString();
                    txtlname.Text = row.Cells["_lastname"].Value?.ToString();
                    txtcontact.Text = row.Cells["phone"].Value?.ToString();
                    string type = row.Cells["_type"].Value?.ToString() ?? "Walk_in";
                    comboBox1.Text = type;

                    if (type == "Regular")
                    {
                        txtemail.Text = row.Cells["email"].Value?.ToString();
                        txtaddress.Text = row.Cells["address"].Value?.ToString();
                        ToggleRegularFields(true);
                    }
                    else
                    {
                        txtemail.Text = "";
                        txtaddress.Text = "";
                        ToggleRegularFields(false);
                    }

                    UIHelper.RoundPanelCorners(paneledit, 20);
                    UIHelper.ShowCenteredPanel(this, paneledit);
                
                
            }
            else if (columnName == "Delete")
            {
                try
                {
                    var confirm = MessageBox.Show("Are you sure you want to delete this customer?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (confirm == DialogResult.Yes)
                    {
                        bool result = _customerBL.DeleteCustomer(selectedCustomerId);
                        MessageBox.Show(result ? "Customer deleted successfully." : "Failed to delete customer.", result ? "Deleted" : "Error",
                            MessageBoxButtons.OK, result ? MessageBoxIcon.Information : MessageBoxIcon.Error);

                        if (result) LoadCustomers();
                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Database error occurred while Deleting: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (ArgumentException ex)
                {
                    MessageBox.Show("Validation error: " + ex.Message, "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void ToggleRegularFields(bool visible)
        {
            lblEmail.Visible = txtemail.Visible = visible;
            lblAddress.Visible = txtaddress.Visible = visible;

            if (!visible)
            {
                txtemail.Text = "";
                txtaddress.Text = "";
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ToggleRegularFields(comboBox1.Text == "Regular");
        }

        private void btnsave1_Click(object sender, EventArgs e)
        {
            string name = txtname.Text.Trim();
            string lname = txtlname.Text.Trim();
            string email = txtemail.Text.Trim();
            string phone = txtcontact.Text.Trim();
            string address = txtaddress.Text.Trim();
            string type = comboBox1.Text;

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(lname) || string.IsNullOrWhiteSpace(type))
            {
                MessageBox.Show("First name, Last name, Type, ", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (type == "Regular" && (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(address)))
            {
                MessageBox.Show("Email and Address are required for regular customers.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                Ipersons customer = new Customer(selectedCustomerId, email, address, name, lname, type, phone);
                bool result = _customerBL.UpdateCustomer(customer);

                MessageBox.Show(result ? "Customer updated successfully." : "Failed to update customer.", result ? "Success" : "Error",
                    MessageBoxButtons.OK, result ? MessageBoxIcon.Information : MessageBoxIcon.Error);

                if (result)
                {
                    ClearForm();
                    paneledit.Visible = false;
                    LoadCustomers();
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Database error occurred while Updating: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show("Validation error: " + ex.Message, "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while updating : " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearForm()
        {
            txtname.Clear();
            txtlname.Clear();
            txtcontact.Clear();
            txtemail.Clear();
            txtaddress.Clear();
            comboBox1.SelectedIndex = -1;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string searchText = textBox1.Text.Trim();

            if (string.IsNullOrEmpty(searchText))
            {
                LoadCustomers();
                return;
            }

            List<Customer> list = _customerBL.SearchCustomers(searchText).OfType<Customer>().ToList();
            MessageBox.Show("Search Results: " + list.Count.ToString());
            dataGridView2.Columns.Clear();
            dataGridView2.DataSource = list;

            dataGridView2.Columns["Id"].Visible = false;
            dataGridView2.Columns["_firstname"].HeaderText = "First Name";
            dataGridView2.Columns["_lastname"].HeaderText = "Last Name";
            dataGridView2.Columns["phone"].HeaderText = "Contact Number";
            dataGridView2.Columns["_type"].HeaderText = "Type";
            dataGridView2.Columns["email"].HeaderText = "Email";
            dataGridView2.Columns["address"].HeaderText = "Address";


            if (dataGridView2.Columns.Contains("_name"))
                dataGridView2.Columns["_name"].Visible = false;



            dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            UIHelper.AddButtonColumn(dataGridView2, "Edit", "Edit", "Edit");
            UIHelper.AddButtonColumn(dataGridView2, "Delete", "Delete", "Delete");
       

        }

        private void btnadd_Click(object sender, EventArgs e)
        {
            var f = Program.ServiceProvider.GetRequiredService<AddCustomerform>();
            f.ShowDialog(this);
        }

        private void pictureBox10_Click(object sender, EventArgs e)
        {
            LoadCustomers();
        }

        private void btncancle1_Click(object sender, EventArgs e)
        {
            paneledit.Visible = false;
        }

        private void iconButton3_Click(object sender, EventArgs e)
        {
            var f=Program.ServiceProvider.GetService<CustomerBill_SpecificProducts>();
        }

        private void iconButton2_Click(object sender, EventArgs e)
        {
            var f = Program.ServiceProvider.GetService<BillingRecordsOverview>();
            Dashboard.Instance.LoadFormIntoPanel(f);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string searchText = textBox1.Text.Trim();

            if (string.IsNullOrEmpty(searchText))
            {
                LoadCustomers();
                return;
            }

            List<Customer> list = _customerBL.SearchCustomers(searchText).OfType<Customer>().ToList();
            MessageBox.Show("Search Results: " + list.Count.ToString());
            MessageBox.Show("TextChanged event triggered!");

            dataGridView2.Columns.Clear();
            dataGridView2.DataSource = list;

            dataGridView2.Columns["Id"].Visible = false;
            dataGridView2.Columns["_firstname"].HeaderText = "First Name";
            dataGridView2.Columns["_lastname"].HeaderText = "Last Name";
            dataGridView2.Columns["phone"].HeaderText = "Contact Number";
            dataGridView2.Columns["_type"].HeaderText = "Type";
            dataGridView2.Columns["email"].HeaderText = "Email";
            dataGridView2.Columns["address"].HeaderText = "Address";

            if (dataGridView2.Columns.Contains("_name"))
                dataGridView2.Columns["_name"].Visible = false;




            dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            UIHelper.AddButtonColumn(dataGridView2, "Edit", "Edit", "Edit");
            UIHelper.AddButtonColumn(dataGridView2, "Delete", "Delete", "Delete");

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            string searchText = textBox5.Text.Trim();

            if (string.IsNullOrEmpty(searchText))
            {
                LoadCustomers();
                return;
            }

            List<Customer> list = _customerBL.SearchCustomers(searchText).OfType<Customer>().ToList();


            dataGridView2.Columns.Clear();
            dataGridView2.DataSource = list;

            dataGridView2.Columns["Id"].Visible = false;
            dataGridView2.Columns["_firstname"].HeaderText = "First Name";
            dataGridView2.Columns["_lastname"].HeaderText = "Last Name";
            dataGridView2.Columns["phone"].HeaderText = "Contact Number";
            dataGridView2.Columns["_type"].HeaderText = "Type";
            dataGridView2.Columns["email"].HeaderText = "Email";
            dataGridView2.Columns["address"].HeaderText = "Address";

            if (dataGridView2.Columns.Contains("_name"))
                dataGridView2.Columns["_name"].Visible = false;




            dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            UIHelper.AddButtonColumn(dataGridView2, "Edit", "Edit", "Edit");
            UIHelper.AddButtonColumn(dataGridView2, "Delete", "Delete", "Delete");
        }
    }
}
