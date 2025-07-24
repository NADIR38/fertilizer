﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using fertilizesop.BL.Bl;
using fertilizesop.BL.Models;
using fertilizesop.DL;
using FontAwesome.Sharp;

namespace fertilizesop.UI
{
    public partial class customer_bills : Form
    {
        private readonly Customerbillbl _billingBL;
        public customer_bills()
        {
            InitializeComponent();
            _billingBL = new Customerbillbl();
            this.Load += Customerbill_Load;
            paneledit.Visible = false;
            UIHelper.StyleGridView(dataGridView2);
        }

        private void Customerbill_Load(object sender, EventArgs e)
        {

            load();
            LoadBillingRecords();
            dataGridView2.Focus();
        }

        private void OpenBillDetailsForm(int billId)
        {
            try
            {
                var billDetailsForm = new CustomerBill_SpecificProducts(billId);
                billDetailsForm.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening bill details: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    var clickedColumn = dataGridView2.Columns[e.ColumnIndex];

                    if (clickedColumn.Name == "btnDetails")
                    {
                        var billIdCell = dataGridView2.Rows[e.RowIndex].Cells["BillID"];
                        if (billIdCell.Value != null && int.TryParse(billIdCell.Value.ToString(), out int billId))
                            OpenBillDetailsForm(billId);
                    }
                    else if (clickedColumn.Name == "btnPayment")
                    {
                        var row = dataGridView2.Rows[e.RowIndex];

                        // Fetch values
                        txtname1.Text = row.Cells["CustomerName"].Value?.ToString();
                        txtbill.Text = row.Cells["BillID"].Value?.ToString();
                        txtamount.Text = row.Cells["DueAmount"].Value?.ToString();
                        txtpayment.Clear();
                        txtremarks.Clear();
                        txtdate.Text = DateTime.Now.ToString();

                        paneledit.Visible = true;
                    }
                }
            }
            catch(Exception ex) 
            {
                MessageBox.Show("Error in buttons " + ex.Message);
            }
        }

        private void load()
        {
            try
            {
                var list = _billingBL.getbill();
                MessageBox.Show($"Fetched {list.Count} bills from DB");

                var filteredList = list.Where(b => b.total_price != 0.00m).ToList();

                if (filteredList.Count == 0)
                {
                    MessageBox.Show("No bills with non-zero total_price were found.");
                }

                dataGridView2.Columns.Clear();
                dataGridView2.DataSource = filteredList;
                dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                if (dataGridView2.Columns.Contains("customer_id"))
                {
                    dataGridView2.Columns["customer_id"].Visible = false;
                }

                if (dataGridView2.Columns.Contains("batch_name"))
                {
                    dataGridView2.Columns["batch_name"].Visible = false;
                }

                UIHelper.AddButtonColumn(dataGridView2, "Edit", "View Details", "Details");
                UIHelper.AddButtonColumn(dataGridView2, "Delete", "Add payment", "payment");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in fetching the customer bills: " + ex.Message);
            }
        }

        private void AddDetailsButtonColumn()
        {
            if (dataGridView2.Columns.Contains("btnDetails"))
                dataGridView2.Columns.Remove("btnDetails");
            if (dataGridView2.Columns.Contains("btnPayment"))
                dataGridView2.Columns.Remove("btnPayment");

            // View Details Button
            var btnDetails = new DataGridViewButtonColumn
            {
                Name = "btnDetails",
                Text = "View Details",
                UseColumnTextForButtonValue = true,
                HeaderText = "Actions",
                FlatStyle = FlatStyle.Flat,
                Width = 100,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    BackColor = Color.FromArgb(0, 126, 250),
                    ForeColor = Color.White,
                    Font = new Font("Segoe UI", 9, FontStyle.Bold)
                }
            };

            // Payment Button
            var btnPayment = new DataGridViewButtonColumn
            {
                Name = "btnPayment",
                Text = "Payment",
                UseColumnTextForButtonValue = true,
                HeaderText = "",
                FlatStyle = FlatStyle.Flat,
                Width = 100,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    BackColor = Color.FromArgb(46, 204, 113),
                    ForeColor = Color.White,
                    Font = new Font("Segoe UI", 9, FontStyle.Bold)
                }
            };

            dataGridView2.Columns.Add(btnDetails);
            dataGridView2.Columns.Add(btnPayment);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Enter)
            {
                if (paneledit.Visible)
                {
                    btnsave1.PerformClick();
                    return true;
                }

            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void LoadBillingRecords(string searchTerm = "")
        {
            try
            {
                DataTable dt = _billingBL.GetBillingRecords(searchTerm);
                dataGridView2.DataSource = dt;

                if (dt.Rows.Count > 0)
                {
                    dataGridView2.ClearSelection();
                    AddDetailsButtonColumn();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading billing records: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



       

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void txtdate_TextChanged(object sender, EventArgs e)
        {

        }

        private void toplbl_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void txtamount_TextChanged(object sender, EventArgs e)
        {

        }

        private void button9_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void txtname1_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox10_Click(object sender, EventArgs e)
        {
            LoadBillingRecords();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void txtpayment_TextChanged(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void btnsave1_Click(object sender, EventArgs e)
        {
            // Validate payment input
            if (!decimal.TryParse(txtpayment.Text, out decimal payment) || payment <= 0)
            {
                MessageBox.Show("Enter a valid payment amount.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Extract other fields
            string customerName = txtname1.Text.Trim();
            if (!int.TryParse(txtbill.Text, out int billId))
            {
                MessageBox.Show("Invalid Bill ID.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string remarks = txtremarks.Text.Trim();

            if (!DateTime.TryParse(txtdate.Text, out DateTime date))
            {
                MessageBox.Show("Enter a valid date.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Create record object
                var record = new Customerrecord
                (0,
                    customerName,
                    payment,
                    date,
                    billId,
                    remarks
                );

                // Call DL method
                bool result = BillingRecordsOverviewDL.AddRecord(record);

                if (result)
                {
                    MessageBox.Show("Payment recorded successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadBillingRecords(); // Reload updated data
                }
                else
                {
                    MessageBox.Show("Failed to record payment.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving payment: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void txtbill_TextChanged(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btncancle1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(textBox1.Text))
                {
                    return;
                }
                var billdeta = _billingBL.searchbill(textBox1.Text);
                dataGridView2.DataSource = billdeta;
                dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                if (dataGridView2.Columns.Contains("customer_id"))
                {
                    dataGridView2.Columns["customer_id"].Visible = false;
                }

                if (dataGridView2.Columns.Contains("batch_name"))
                {
                    dataGridView2.Columns["batch_name"].Visible = false;
                }

                UIHelper.AddButtonColumn(dataGridView2, "Edit", "View Details", "Details");
                UIHelper.AddButtonColumn(dataGridView2, "Delete", "Add payment", "payment");
            }catch (Exception ex)
            {
                MessageBox.Show("error in searching " + ex.Message );
            }
        }

        private void txtremarks_TextChanged(object sender, EventArgs e)
        {

        }

        private void paneledit_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
