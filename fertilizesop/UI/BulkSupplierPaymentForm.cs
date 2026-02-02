using fertilizesop.BL.Models;
using fertilizesop.DL;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace fertilizesop.UI
{
    public partial class BulkSupplierPaymentForm : Form
    {
        private readonly SupplierbillDl _supplierBillDl;
        private int _selectedSupplierId = 0;

        public BulkSupplierPaymentForm()
        {
            InitializeComponent();
            _supplierBillDl = new SupplierbillDl();
            StyleForm();
            LoadSupplierSummaries();
        }

        private void StyleForm()
        {
            // Modern color scheme
            this.BackColor = Color.FromArgb(240, 242, 245);

            // Style DataGridView
            dgvSuppliers.BorderStyle = BorderStyle.None;
            dgvSuppliers.BackgroundColor = Color.White;
            dgvSuppliers.EnableHeadersVisualStyles = false;
            dgvSuppliers.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(255, 87, 34);
            dgvSuppliers.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvSuppliers.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvSuppliers.ColumnHeadersDefaultCellStyle.Padding = new Padding(5);
            dgvSuppliers.ColumnHeadersHeight = 40;
            dgvSuppliers.DefaultCellStyle.SelectionBackColor = Color.FromArgb(255, 243, 224);
            dgvSuppliers.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvSuppliers.DefaultCellStyle.Font = new Font("Segoe UI", 9);
            dgvSuppliers.RowTemplate.Height = 35;
            dgvSuppliers.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(250, 250, 250);
            dgvSuppliers.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvSuppliers.GridColor = Color.FromArgb(230, 230, 230);

            // Style search textbox
            txtSearch.Font = new Font("Segoe UI", 11);
            txtSearch.BorderStyle = BorderStyle.FixedSingle;

            // Style payment group
            grpPayment.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            grpPayment.ForeColor = Color.FromArgb(50, 50, 50);

            // Style labels
            lblSelectedSupplier.Font = new Font("Segoe UI", 11, FontStyle.Bold);
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
            btnRefresh.BackColor = Color.FromArgb(255, 87, 34);
            btnRefresh.ForeColor = Color.White;
            btnRefresh.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            btnRefresh.Cursor = Cursors.Hand;

            // Add hover effects
            AddButtonHoverEffects(btnProcessPayment, Color.FromArgb(76, 175, 80), Color.FromArgb(56, 155, 60));
            AddButtonHoverEffects(btnRefresh, Color.FromArgb(255, 87, 34), Color.FromArgb(235, 67, 14));
        }

        private void AddButtonHoverEffects(Button btn, Color normalColor, Color hoverColor)
        {
            btn.MouseEnter += (s, e) => btn.BackColor = hoverColor;
            btn.MouseLeave += (s, e) => btn.BackColor = normalColor;
        }

        private void LoadSupplierSummaries(string searchText = "")
        {
            try
            {
                // Show loading state
                dgvSuppliers.Cursor = Cursors.WaitCursor;

                var summaries = _supplierBillDl.GetSupplierPaymentSummary(searchText);
                dgvSuppliers.DataSource = summaries;

                // FIXED: Check if columns exist before accessing them
                if (dgvSuppliers.Columns.Contains("SupplierId"))
                    dgvSuppliers.Columns["SupplierId"].Visible = false;

                if (dgvSuppliers.Columns.Contains("SupplierName") && dgvSuppliers.Columns["SupplierName"] != null)
                {
                    dgvSuppliers.Columns["SupplierName"].HeaderText = "Supplier Name";
                    dgvSuppliers.Columns["SupplierName"].Width = 200;
                }

                if (dgvSuppliers.Columns.Contains("SupplierPhone") && dgvSuppliers.Columns["SupplierPhone"] != null)
                {
                    dgvSuppliers.Columns["SupplierPhone"].HeaderText = "Phone";
                    dgvSuppliers.Columns["SupplierPhone"].Width = 120;
                }

                if (dgvSuppliers.Columns.Contains("TotalPending") && dgvSuppliers.Columns["TotalPending"] != null)
                {
                    dgvSuppliers.Columns["TotalPending"].HeaderText = "Total Pending";
                    dgvSuppliers.Columns["TotalPending"].DefaultCellStyle.Format = "N2";
                    dgvSuppliers.Columns["TotalPending"].DefaultCellStyle.ForeColor = Color.FromArgb(244, 67, 54);
                    dgvSuppliers.Columns["TotalPending"].DefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
                    dgvSuppliers.Columns["TotalPending"].Width = 120;
                }

                if (dgvSuppliers.Columns.Contains("TotalPaid") && dgvSuppliers.Columns["TotalPaid"] != null)
                {
                    dgvSuppliers.Columns["TotalPaid"].HeaderText = "Total Paid";
                    dgvSuppliers.Columns["TotalPaid"].DefaultCellStyle.Format = "N2";
                    dgvSuppliers.Columns["TotalPaid"].DefaultCellStyle.ForeColor = Color.FromArgb(76, 175, 80);
                    dgvSuppliers.Columns["TotalPaid"].DefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
                    dgvSuppliers.Columns["TotalPaid"].Width = 120;
                }

                if (dgvSuppliers.Columns.Contains("PendingBillCount") && dgvSuppliers.Columns["PendingBillCount"] != null)
                {
                    dgvSuppliers.Columns["PendingBillCount"].HeaderText = "Pending Bills";
                    dgvSuppliers.Columns["PendingBillCount"].Width = 100;
                }

                dgvSuppliers.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                dgvSuppliers.Cursor = Cursors.Default;
                MessageBox.Show("Error loading supplier summaries: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DgvSuppliers_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvSuppliers.SelectedRows.Count > 0)
            {
                var row = dgvSuppliers.SelectedRows[0];
                _selectedSupplierId = Convert.ToInt32(row.Cells["SupplierId"].Value);
                string supplierName = row.Cells["SupplierName"].Value.ToString();
                decimal totalPending = Convert.ToDecimal(row.Cells["TotalPending"].Value);
                decimal totalPaid = Convert.ToDecimal(row.Cells["TotalPaid"].Value);
                int billCount = Convert.ToInt32(row.Cells["PendingBillCount"].Value);

                lblSelectedSupplier.Text = $"📦 {supplierName}";
                lblTotalPending.Text = $"Pending: Rs. {totalPending:N2} ({billCount} bills)";
                lblTotalPaid.Text = $"Paid: Rs. {totalPaid:N2}";

                // Enable payment section
                grpPayment.Enabled = true;
            }
            else
            {
                _selectedSupplierId = 0;
                lblSelectedSupplier.Text = "Select a supplier from the list above";
                lblTotalPending.Text = "Total Pending: -";
                lblTotalPaid.Text = "Total Paid: -";
                grpPayment.Enabled = false;
            }
        }

        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadSupplierSummaries(txtSearch.Text.Trim());
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
            LoadSupplierSummaries();
            MessageBox.Show("Supplier list refreshed successfully!", "Refresh",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnProcessPayment_Click(object sender, EventArgs e)
        {
            if (_selectedSupplierId == 0)
            {
                MessageBox.Show("Please select a supplier from the list.", "Validation Error",
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
                $"📦 Supplier: {lblSelectedSupplier.Text}\n" +
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

                bool success = _supplierBillDl.ProcessBulkPayment(_selectedSupplierId, paymentAmount, txtRemarks.Text.Trim());

                this.Enabled = true;
                this.Cursor = Cursors.Default;
                btnProcessPayment.Text = "Process Payment";

                if (success)
                {
                    MessageBox.Show(
                        $"✅ Payment Successful!\n\n" +
                        $"💰 Amount: Rs. {paymentAmount:N2}\n" +
                        $"📦 Supplier: {lblSelectedSupplier.Text}\n\n" +
                        $"Payment has been distributed proportionally across all pending bills.",
                        "Success",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );

                    // Clear inputs and refresh
                    txtPaymentAmount.Clear();
                    txtRemarks.Clear();
                    LoadSupplierSummaries(txtSearch.Text.Trim());
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