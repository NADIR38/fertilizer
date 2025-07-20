using fertilizesop.BL.Bl;
using fertilizesop.Interfaces.BLInterfaces;
using KIMS;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace fertilizesop.UI
{
    public partial class Addbatchdetailsform : Form
    {
        private int selectedProductId;
        private string selectedProductName;
        private string selectedProductDescription;
        private readonly IbatchdetailsBl ibl;
        public Addbatchdetailsform(IbatchdetailsBl ibl)
        {

            InitializeComponent();
            UIHelper.StyleGridView(dataGridView2);
            this.txtBname.TextChanged += txtBname_TextChanged;
            this.ibl = ibl;
            this.dataGridView2.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView2_CellClick);
            this.txtBname.TextUpdate += txtBname_TextUpdate;
            this.KeyPreview = true; // Allows the form to receive key events before the controls
            
    this.KeyDown += Addbatchdetailsform_KeyDown;
        }
        private void Addbatchdetailsform_KeyDown(object sender, KeyEventArgs e)
        {
            // Ctrl + S for Save
            if (e.Control && e.KeyCode == Keys.S)
            {
                btnsave.PerformClick();
                e.Handled = true;
            }

            // Arrow key navigation in DataGridView
            if (dataGridView2.Focused)
            {
                if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down)
                {
                    int rowIndex = dataGridView2.CurrentCell.RowIndex;
                    int columnIndex = dataGridView2.CurrentCell.ColumnIndex;

                    if (e.KeyCode == Keys.Up && rowIndex > 0)
                    {
                        dataGridView2.CurrentCell = dataGridView2.Rows[rowIndex - 1].Cells[columnIndex];
                        e.Handled = true;
                    }
                    else if (e.KeyCode == Keys.Down && rowIndex < dataGridView2.Rows.Count - 2)
                    {
                        dataGridView2.CurrentCell = dataGridView2.Rows[rowIndex + 1].Cells[columnIndex];
                        e.Handled = true;
                    }
                }
            }
        }

        private void btnsave_Click(object sender, EventArgs e)
        {
            string batchname = txtBname.Text.Trim();
            string costText = txtprice.Text.Trim();
            string saleText = txtSprice.Text.Trim();
            string quantityText = txtquantity.Text.Trim();

            if (string.IsNullOrWhiteSpace(batchname) ||
                string.IsNullOrWhiteSpace(costText) ||
                string.IsNullOrWhiteSpace(saleText) ||
                string.IsNullOrWhiteSpace(quantityText))
            {
                MessageBox.Show("Please fill in all fields: Batch Name, Cost Price, Sale Price, and Quantity.",
                                "Input Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!decimal.TryParse(costText, out decimal cost_price))
            {
                MessageBox.Show("Invalid cost price. Please enter a valid number.",
                                "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!decimal.TryParse(saleText, out decimal sale_price))
            {
                MessageBox.Show("Invalid sale price. Please enter a valid number.",
                                "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(quantityText, out int quantity))
            {
                MessageBox.Show("Invalid quantity. Please enter a whole number.",
                                "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var detail = new BatchDetails(0, batchname, cost_price, sale_price, selectedProductId, selectedProductName, quantity);
                var result=ibl.adddetails(detail);
                if (result)
                {
                    MessageBox.Show("Customer added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                  
                
                }
                else
                {
                    MessageBox.Show("Failed to add customer. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (ArgumentNullException ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Database error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An unexpected error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void txtBname_TextChanged(object sender, EventArgs e)
        {
            if (!txtBname.DroppedDown)
            {
                txtBname.DroppedDown = true;
                txtBname.SelectionStart = txtBname.Text.Length;
                txtBname.SelectionLength = 0;
            }
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string text = txtproducts.Text.Trim();
            var list = DatabaseHelper.Instance.GetProductsByNames(text);
            dataGridView2.DataSource = list;
            dataGridView2.Columns["Id"].Visible=false;
            dataGridView2.Columns["Price"].Visible=false;
            dataGridView2.Columns["quantity"].Visible = false;


        }
        private void loadbatches()
        {
            var batchNames =DatabaseHelper.Instance.Getbatches("");
            if (batchNames != null && batchNames.Count > 0)
            {
                txtBname.Items.Clear();
                txtBname.Items.AddRange(batchNames.ToArray());

                var autoSource = new AutoCompleteStringCollection();
                autoSource.AddRange(batchNames.ToArray());
                txtBname.AutoCompleteCustomSource = autoSource;
                txtBname.AutoCompleteMode = AutoCompleteMode.Suggest;
                txtBname.SelectedIndex = -1;
            }
        }
        private void Addbatchdetailsform_Load(object sender, EventArgs e)
        {
            loadbatches();
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var rows = dataGridView2.Rows[e.RowIndex];
                selectedProductId = Convert.ToInt32(rows.Cells["Id"].Value);
                selectedProductDescription=rows.Cells["Description"].Value.ToString();
                selectedProductName=rows.Cells["Name"].Value.ToString();
                txtproducts.Text = selectedProductName;
                txtSprice.Text= ibl.getsaleprice(selectedProductId).ToString();
                MessageBox.Show($"Selected: ID={selectedProductId}, Name={selectedProductName}, Desc={selectedProductDescription}");

            }
        }
        private void txtBname_TextUpdate(object sender, EventArgs e)
        {
            string searchText = txtBname.Text.Trim();

            // Fetch filtered list from database
            var filteredBatches = DatabaseHelper.Instance.Getbatches(searchText);

            if (filteredBatches != null)
            {
                txtBname.Items.Clear();
                txtBname.Items.AddRange(filteredBatches.ToArray());

                txtBname.SelectionStart = searchText.Length;
                txtBname.SelectionLength = 0;
                txtBname.DroppedDown = true;
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            dataGridView2.Columns.Clear();
        }
    }
}