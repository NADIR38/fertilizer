using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using fertilizesop.BL.Bl;
using fertilizesop.DL;

namespace fertilizesop.UI
{
    public partial class transactionView : Form
    {
        TransactionBL bl = new TransactionBL();
        int selectedRowIndex;
        public transactionView()
        {
            InitializeComponent();
            transactionData.AutoGenerateColumns = true;
            transactionData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            transactionData.EnableHeadersVisualStyles = false; // Important for custom styles
            transactionData.DefaultCellStyle.Font = new Font("Segoe UI", 12F, FontStyle.Regular);

            transactionData.ColumnHeadersDefaultCellStyle.BackColor = Color.DarkGreen;
            transactionData.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            transactionData.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            transactionData.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;


            LoadGrid();
        }

        private void LoadGrid()
        {
            TransactionBL bl = new TransactionBL();
            transactionData.DataSource = bl.ViewAllTransactions();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            transactionData.DataSource = bl.Search(txtSearch.Text.Trim());
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            LoadGrid();
        }

        private void iconButton9_Click(object sender, EventArgs e)
        {
            AddTransaction addTransaction = new AddTransaction();
            addTransaction.ShowDialog();
        }

        private void transactionView_Load(object sender, EventArgs e)
        {
            transactionData.Columns.Clear();

            transactionData.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "transaction_id",   // db column name
                HeaderText = "Transaction ID",         // your custom header
                Name = "colTransactionId"
            });

            transactionData.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "transaction_type",
                HeaderText = "Type",
                Name = "colType"
            });

            transactionData.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "amount",
                HeaderText = "Amount",
                Name = "colAmount"
            });

            transactionData.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "transaction_date",
                HeaderText = "Date",
                Name = "colDate"
            });

            transactionData.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "description",
                HeaderText = "Description",
                Name = "colDescription"
            });

            transactionData.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "remaining_balance",
                HeaderText = "Remaining Balance",
                Name = "colRemaining"
            });

            transactionData.Columns["colTransactionId"].Visible = false;
            transactionData.Focus();
        }
       
        // Keys logic here

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            
            if (keyData == (Keys.Control | Keys.A))
            {
                iconButton9.PerformClick();
                return true;
            }
            else if (keyData == (Keys.Control | Keys.R))
            {
                LoadGrid();
                return true;
            }
            else if (keyData == Keys.Up)
            {
                if (transactionData.Visible && selectedRowIndex > 0)
                {
                    selectedRowIndex--;
                    transactionData.ClearSelection();
                    transactionData.Rows[selectedRowIndex].Selected = true;
                    return true;
                }
            }
            else if (keyData == Keys.Down)
            {
                if (transactionData.Visible && selectedRowIndex < transactionData.Rows.Count - 1)
                {
                    selectedRowIndex++;
                    transactionData.ClearSelection();
                    transactionData.Rows[selectedRowIndex].Selected = true;
                    return true;
                }
            }

            return base.ProcessCmdKey(ref msg, keyData); // Allow default behavior
        }
    }
}
