using fertilizesop.BL.Models;
using fertilizesop.DL;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace fertilizesop.UI
{
    public partial class BulkCustomerPaymentForm : Form
    {
        private readonly Customerbilldl _customerBillDl;
        private int _selectedCustomerId = 0;

        public BulkCustomerPaymentForm()
        {
            InitializeComponent();
            _customerBillDl = new Customerbilldl();
            StyleForm();
            LoadCustomerSummaries();
        }

        private void StyleForm()
        {
            // Modern color scheme
            this.BackColor = Color.FromArgb(240, 242, 245);

            // Style DataGridView
            dgvCustomers.BorderStyle = BorderStyle.None;
            dgvCustomers.BackgroundColor = Color.White;
            dgvCustomers.EnableHeadersVisualStyles = false;
            dgvCustomers.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(0, 122, 204);
            dgvCustomers.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvCustomers.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvCustomers.ColumnHeadersDefaultCellStyle.Padding = new Padding(5);
            dgvCustomers.ColumnHeadersHeight = 40;
            dgvCustomers.DefaultCellStyle.SelectionBackColor = Color.FromArgb(230, 240, 250);
            dgvCustomers.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvCustomers.DefaultCellStyle.Font = new Font("Segoe UI", 9);
            dgvCustomers.RowTemplate.Height = 35;
            dgvCustomers.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(250, 250, 250);
            dgvCustomers.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvCustomers.GridColor = Color.FromArgb(230, 230, 230);

            // Style search textbox
            txtSearch.Font = new Font("Segoe UI", 11);
            txtSearch.BorderStyle = BorderStyle.FixedSingle;

            // Style payment group
            grpPayment.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            grpPayment.ForeColor = Color.FromArgb(50, 50, 50);

            // Style labels
            lblSelectedCustomer.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            lblTotalPending.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            lblTotalPaid.Font = new Font("Segoe UI", 10, FontStyle.Bold);

            // Style input textboxes
            txtPaymentAmount.Font = new Font("Segoe UI", 11);
            txtPaymentAmount.BorderStyle = BorderStyle.FixedSingle;
            txtRemarks.Font = new Font("Segoe UI", 9);
            txtRemarks.BorderStyle = BorderStyle.FixedSingle;

            // Style buttons
            btnProcessPayment.FlatStyle = FlatStyle.Flat;
            btnProcessPayment.FlatAppearance.BorderSize = 0;
            btnProcessPayment.BackColor = Color.FromArgb(76, 175, 80);
            btnProcessPayment.ForeColor = Color.White;
            btnProcessPayment.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            btnProcessPayment.Cursor = Cursors.Hand;

            btnRefresh.FlatStyle = FlatStyle.Flat;
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.BackColor = Color.FromArgb(0, 122, 204);
            btnRefresh.ForeColor = Color.White;
            btnRefresh.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            btnRefresh.Cursor = Cursors.Hand;

            // Add hover effects
            AddButtonHoverEffects(btnProcessPayment, Color.FromArgb(76, 175, 80), Color.FromArgb(56, 155, 60));
            AddButtonHoverEffects(btnRefresh, Color.FromArgb(0, 122, 204), Color.FromArgb(0, 102, 184));
        }

        private void AddButtonHoverEffects(Button btn, Color normalColor, Color hoverColor)
        {
            btn.MouseEnter += (s, e) => btn.BackColor = hoverColor;
            btn.MouseLeave += (s, e) => btn.BackColor = normalColor;
        }

        private void LoadCustomerSummaries(string searchText = "")
        {
            try
            {
                // Show loading state
                dgvCustomers.Cursor = Cursors.WaitCursor;

                var summaries = _customerBillDl.GetCustomerPaymentSummary(searchText);
                dgvCustomers.DataSource = summaries;

                // Customize column headers and formatting
                if (dgvCustomers.Columns.Contains("CustomerId"))
                    dgvCustomers.Columns["CustomerId"].Visible = false;

                if (dgvCustomers.Columns.Contains("CustomerName"))
                {
                    dgvCustomers.Columns["CustomerName"].HeaderText = "Customer Name";
                    dgvCustomers.Columns["CustomerName"].Width = 200;
                }

                if (dgvCustomers.Columns.Contains("CustomerPhone"))
                {
                    dgvCustomers.Columns["CustomerPhone"].HeaderText = "Phone";
                    dgvCustomers.Columns["CustomerPhone"].Width = 120;
                }

                if (dgvCustomers.Columns.Contains("TotalPending"))
                {
                    dgvCustomers.Columns["TotalPending"].HeaderText = "Total Pending";
                    dgvCustomers.Columns["TotalPending"].DefaultCellStyle.Format = "N2";
                    dgvCustomers.Columns["TotalPending"].DefaultCellStyle.ForeColor = Color.FromArgb(244, 67, 54);
                    dgvCustomers.Columns["TotalPending"].DefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
                    dgvCustomers.Columns["TotalPending"].Width = 120;
                }

                if (dgvCustomers.Columns.Contains("TotalPaid"))
                {
                    dgvCustomers.Columns["TotalPaid"].HeaderText = "Total Paid";
                    dgvCustomers.Columns["TotalPaid"].DefaultCellStyle.Format = "N2";
                    dgvCustomers.Columns["TotalPaid"].DefaultCellStyle.ForeColor = Color.FromArgb(76, 175, 80);
                    dgvCustomers.Columns["TotalPaid"].DefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
                    dgvCustomers.Columns["TotalPaid"].Width = 120;
                }

                if (dgvCustomers.Columns.Contains("PendingBillCount"))
                {
                    dgvCustomers.Columns["PendingBillCount"].HeaderText = "Pending Bills";
                    dgvCustomers.Columns["PendingBillCount"].Width = 100;
                }

                dgvCustomers.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                dgvCustomers.Cursor = Cursors.Default;
                MessageBox.Show("Error loading customer summaries: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DgvCustomers_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvCustomers.SelectedRows.Count > 0)
            {
                var row = dgvCustomers.SelectedRows[0];
                _selectedCustomerId = Convert.ToInt32(row.Cells["CustomerId"].Value);
                string customerName = row.Cells["CustomerName"].Value.ToString();
                decimal totalPending = Convert.ToDecimal(row.Cells["TotalPending"].Value);
                decimal totalPaid = Convert.ToDecimal(row.Cells["TotalPaid"].Value);
                int billCount = Convert.ToInt32(row.Cells["PendingBillCount"].Value);

                lblSelectedCustomer.Text = $"📋 {customerName}";
                lblTotalPending.Text = $"Pending: Rs. {totalPending:N2} ({billCount} bills)";
                lblTotalPaid.Text = $"Paid: Rs. {totalPaid:N2}";

                // Enable payment section
                grpPayment.Enabled = true;
            }
            else
            {
                _selectedCustomerId = 0;
                lblSelectedCustomer.Text = "Select a customer from the list above";
                lblTotalPending.Text = "Total Pending: -";
                lblTotalPaid.Text = "Total Paid: -";
                grpPayment.Enabled = false;
            }
        }

        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadCustomerSummaries(txtSearch.Text.Trim());
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
            LoadCustomerSummaries();
            MessageBox.Show("Customer list refreshed successfully!", "Refresh",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnProcessPayment_Click(object sender, EventArgs e)
        {
            if (_selectedCustomerId == 0)
            {
                MessageBox.Show("Please select a customer from the list.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPaymentAmount.Text))
            {
                MessageBox.Show("Please enter payment amount.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPaymentAmount.Focus();
                return;
            }

            if (!decimal.TryParse(txtPaymentAmount.Text, out decimal paymentAmount) || paymentAmount <= 0)
            {
                MessageBox.Show("Please enter a valid payment amount greater than zero.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPaymentAmount.Focus();
                return;
            }

            var confirmResult = MessageBox.Show(
                $"💳 Process bulk payment of Rs. {paymentAmount:N2}?\n\n" +
                $"📋 Customer: {lblSelectedCustomer.Text}\n" +
                $"📊 The payment will be distributed proportionally across ALL pending bills.\n\n" +
                $"✔️ Continue with payment?",
                "Confirm Bulk Payment",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (confirmResult != DialogResult.Yes)
                return;

            try
            {
                // Disable form during processing
                this.Enabled = false;
                this.Cursor = Cursors.WaitCursor;
                btnProcessPayment.Text = "Processing...";
                Application.DoEvents();

                bool success = _customerBillDl.ProcessBulkPayment(_selectedCustomerId, paymentAmount, txtRemarks.Text.Trim());

                this.Enabled = true;
                this.Cursor = Cursors.Default;
                btnProcessPayment.Text = "Process Payment";

                if (success)
                {
                    MessageBox.Show(
                        $"✅ Payment Successful!\n\n" +
                        $"💰 Amount: Rs. {paymentAmount:N2}\n" +
                        $"📋 Customer: {lblSelectedCustomer.Text}\n\n" +
                        $"Payment has been distributed proportionally across all pending bills.",
                        "Success",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );

                    // Clear inputs and refresh
                    txtPaymentAmount.Clear();
                    txtRemarks.Clear();
                    LoadCustomerSummaries(txtSearch.Text.Trim());
                }
                else
                {
                    MessageBox.Show("❌ Failed to process payment. Please try again.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                this.Enabled = true;
                this.Cursor = Cursors.Default;
                btnProcessPayment.Text = "Process Payment";

                MessageBox.Show("Error processing payment: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Handle Enter key for quick payment
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Enter && txtPaymentAmount.Focused)
            {
                txtRemarks.Focus();
                return true;
            }
            else if (keyData == Keys.Enter && txtRemarks.Focused)
            {
                btnProcessPayment.PerformClick();
                return true;
            }
            else if (keyData == (Keys.Control | Keys.R))
            {
                btnRefresh.PerformClick();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}