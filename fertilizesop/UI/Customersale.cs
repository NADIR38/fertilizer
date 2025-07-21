using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using fertilizesop.DL;

namespace fertilizesop.UI
{
    public partial class Customersale : Form
    {
        private DataGridView dgvproductsearch = new DataGridView();
        Customersaledl _customersaledl = new Customersaledl();
        private DataGridViewRow row;
        private int selectedRowIndex = -1;
        public Customersale()
        {
            InitializeComponent();
            dataGridView1.CellEndEdit += dataGridView1_CellEndEdit;
            dataGridView1.CurrentCellDirtyStateChanged += dataGridView1_CurrentCellDirtyStateChanged;
            dataGridView1.CellValueChanged += dataGridView1_CellValueChanged;

            setupproductsearch();
            txtproductsearch.TextChanged += txtproductsearch_TextChanged;
        }

        private void dataGridView1_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dataGridView1.IsCurrentCellDirty)
            {
                dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            string columnName = dataGridView1.Columns[e.ColumnIndex].Name;

            if (columnName == "quantity" || columnName == "discount")
            {
                CalculateRowTotal(e.RowIndex);
            }
        }


        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if(keyData == Keys.Enter)
            {
                if(dgvproductsearch.Visible )
                {
                    button1.PerformClick();
                    return true;
                       
                }
            }

            else if (keyData == Keys.Right)
            {
                if (dataGridView1.CurrentCell != null)
                {
                    int rowIndex = dataGridView1.CurrentCell.RowIndex;
                    int colIndex = dataGridView1.CurrentCell.ColumnIndex;

                    if (colIndex < dataGridView1.Columns.Count - 1)
                    {
                        dataGridView1.CurrentCell = dataGridView1.Rows[rowIndex].Cells[colIndex + 1];
                    }

                    return true;
                }
            }

            else if(keyData == Keys.Up)
            {
                if(dgvproductsearch.Visible && txtproductsearch.Focused && selectedRowIndex > 0)
                {
                    selectedRowIndex--;
                    dgvproductsearch.ClearSelection();
                    dgvproductsearch.Rows[selectedRowIndex].Selected = true;
                }
                
                else if(dataGridView1.Focused && selectedRowIndex > 0)
                {
                    selectedRowIndex--;
                    dataGridView1.ClearSelection();
                    dataGridView1.Rows[selectedRowIndex ].Selected = true;
                }
            }

            else if(keyData == Keys.Down)
            {
                if(dgvproductsearch.Visible && txtproductsearch.Focused && selectedRowIndex < dgvproductsearch.Rows.Count -1)
                {
                    selectedRowIndex++;
                    dgvproductsearch.ClearSelection();
                    dgvproductsearch.Rows[selectedRowIndex ].Selected = true;
                }
                else if(dataGridView1.Focused && selectedRowIndex < dataGridView1.Rows.Count -1)
                {
                    selectedRowIndex++;
                    dataGridView1.ClearSelection();
                    dataGridView1.Rows[selectedRowIndex].Selected = true;
                }
            }

            else if (keyData == Keys.Left)
            {
                if (dataGridView1.CurrentCell != null)
                {
                    int rowindex = dataGridView1.CurrentCell.RowIndex;
                    int colindex = dataGridView1.CurrentCell.ColumnIndex;
                    if (colindex > 0)
                    {
                        dataGridView1.CurrentCell = dataGridView1.Rows[rowindex].Cells[colindex - 1];
                    }
                }
            }

            else if (keyData == Keys.Delete)
            {
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    int row = dataGridView1.SelectedCells[0].RowIndex;
                    if (row >= 0 && row < dataGridView1.Columns.Count - 1)
                    {
                        DialogResult result = MessageBox.Show("Are you sure you want to delete this product from the list?", "Confirm deletion", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                        if (result == DialogResult.Yes)
                        {
                            dataGridView1.Rows.RemoveAt(row);
                        }
                    }
                }
            }


                return base.ProcessCmdKey(ref msg, keyData);

        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            // Get column name being edited
            string columnName = dataGridView1.Columns[e.ColumnIndex].Name;

            // Only run calculation if quantity or discount was edited
            if (columnName == "quantity" || columnName == "discount")
            {
                CalculateRowTotal(e.RowIndex);
            }
        }

        private void CalculateRowTotal(int rowIndex)
        {
            try
            {
                DataGridViewRow row = dataGridView1.Rows[rowIndex];

                // Get cell values
                decimal salePrice = Convert.ToDecimal(row.Cells["sale_price"].Value ?? 0);
                decimal discount = Convert.ToDecimal(row.Cells["discount"].Value ?? 0);
                int quantity = Convert.ToInt32(row.Cells["quantity"].Value ?? 0);

                // Validation
                if (discount < 0 || discount > salePrice)
                {
                    MessageBox.Show("Discount cannot be negative or greater than sale price.", "Invalid Discount", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    row.Cells["discount"].Value = 0;
                    discount = 0;
                }

                // Calculate total
                decimal discountedPrice = salePrice - discount;
                decimal total = discountedPrice * quantity;

                // Set total cell value
                row.Cells["total"].Value = total;
            }
            catch
            {
                // Optional: handle conversion errors or nulls
                dataGridView1.Rows[rowIndex].Cells["total"].Value = 0;
            }
        }


        private void setupproductsearch()
        {
            dgvproductsearch.Visible = false;
            dgvproductsearch.ReadOnly = false;
            dgvproductsearch.AutoGenerateColumns = true;
            dgvproductsearch.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvproductsearch.AllowUserToAddRows = false;
            dgvproductsearch.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvproductsearch.BackgroundColor = SystemColors.Control;
            //dgvproductsearch.Columns.Add("Product", "name");
            //dgvproductsearch.Columns.Add("Description", "description");
            //dgvproductsearch.Columns.Add("Sale Price", "sale_Price");
            //dgvproductsearch.Columns.Add("Quantity_in_stock", "quantity");
            this.Controls.Add(dgvproductsearch); // Add this inside setupproductsearch()
            dgvproductsearch.Location = new System.Drawing.Point(50, 400); // ✅ Correct
            dgvproductsearch.Size = new System.Drawing.Size(dataGridView1.Width, dataGridView1.Height / 2);
            dgvproductsearch.BringToFront();
            dgvproductsearch.CellClick += dgvproductsearch_CellCliick;
        }

        private void dgvproductsearch_CellCliick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                // Access selected row
                DataGridViewRow selectedRow = dgvproductsearch.Rows[e.RowIndex];

                // Get values from the row
                string name = selectedRow.Cells["name"].Value.ToString();
                string description = selectedRow.Cells["description"].Value.ToString();

            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void txtproductsearch_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtproductsearch.Text))
            {
                clearfields();
                return;
            }
            if (dgvproductsearch.Columns.Contains("product_id"))
            {
                dgvproductsearch.Columns["product_id"].Visible = false;
            }
            dgvproductsearch.Visible = true;
            DataTable dt = new DataTable();
            dt = _customersaledl.getproductthings(txtproductsearch.Text);
            dgvproductsearch.DataSource = dt;
        }
        private void clearfields()
        {
            txtcustsearch.Text = string.Empty;
            txtproductsearch.Text = string.Empty; txtproductsearch.Focus();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dgvproductsearch.SelectedRows.Count > 0)
            {
                // Access selected row
                DataGridViewRow selectedRow = dgvproductsearch.SelectedRows[0];

                // Get values from the row
                string name = selectedRow.Cells["name"].Value.ToString();
                string description = selectedRow.Cells["description"].Value.ToString();
                int saleprice =Convert.ToInt32( selectedRow.Cells["sale_price"].Value.ToString());

                dataGridView1.Rows.Add(name,description, saleprice);
                dgvproductsearch.Visible = false;
                clearfields();
            }
        }

        private void delete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                {
                    dataGridView1.Rows.Remove(row);
                }
            }
            else
            {
                MessageBox.Show("Selection Required", "Please select items to remove");
            }
        }

        
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
