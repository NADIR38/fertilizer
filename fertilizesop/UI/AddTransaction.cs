using System;
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
using FontAwesome.Sharp;
using Mysqlx.Crud;

namespace fertilizesop.UI
{
    public partial class AddTransaction : Form
    {
        TransactionBL bl = new TransactionBL();
        public AddTransaction()
        {
            InitializeComponent();
            dtpDate.Value = DateTime.Now;
        }

        private void btnsave_Click(object sender, EventArgs e)
        {

            if (!ValidateTransactionInputs()) return;

            Transaction t = new Transaction
            {
                TransactionType = cmbTransactionType.Text,
                Amount = decimal.Parse(txtAmount.Text),
                TransactionDate = dtpDate.Value.Date,
                Description = txtDescription.Text
            };

            if (bl.AddTransaction(t))
            {
                MessageBox.Show("Transaction saved successfully!");
                this.Close();
              
            }
            else
            {
                MessageBox.Show("Failed to save transaction.");
                this .Close();
            }

        }

        private void btncancle1_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            

            if (keyData == (Keys.Control | Keys.S))
            {
                btnsave.PerformClick();
                return true;
            }

            else if (keyData == Keys.Escape)
            {
                btncancle1.PerformClick();
                return true;
            }

            

            return base.ProcessCmdKey(ref msg, keyData); // Allow default behavior
        }

        private bool ValidateTransactionInputs()
        {
            // Validate Transaction Type
            if (string.IsNullOrWhiteSpace(cmbTransactionType.Text))
            {
                MessageBox.Show("Please select a transaction type (Deposit or Withdraw).", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbTransactionType.Focus();
                return false;
            }

            // Validate Amount
            if (string.IsNullOrWhiteSpace(txtAmount.Text) || !decimal.TryParse(txtAmount.Text, out decimal amount) || amount <= 0)
            {
                MessageBox.Show("Please enter a valid positive amount.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtAmount.Focus();
                return false;
            }

            // Validate Date (optional, assuming all dates allowed)
            if (dtpDate.Value.Date > DateTime.Now.Date)
            {
                MessageBox.Show("Future dates are not allowed.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtpDate.Focus();
                return false;
            }

            // Description is optional — but if you want at least something:
            if (string.IsNullOrWhiteSpace(txtDescription.Text))
            {
                MessageBox.Show("Please enter a description.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDescription.Focus();
                return false;
            }

            return true;
        }

        private void AddTransaction_Load(object sender, EventArgs e)
        {

        }
    }
}
