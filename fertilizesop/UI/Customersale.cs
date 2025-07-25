﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using fertilizesop.BL.Models;
using fertilizesop.DL;
using FontAwesome.Sharp;
using Newtonsoft.Json;

namespace fertilizesop.UI
{
    public partial class Customersale : Form
    {
        private DataGridView dgvproductsearch = new DataGridView();
        private DataGridView dgvcustomersearch = new DataGridView();
        Customersaledl _customersaledl = new Customersaledl();
        private DataGridViewRow row;
        private int selectedRowIndex = -1;
        public Customersale()
        {
            InitializeComponent();
            dataGridView1.CellEndEdit += dataGridView1_CellEndEdit;
            dataGridView1.RowsAdded += dataGridView1_RowsAdded;
            dataGridView1.CurrentCellDirtyStateChanged += dataGridView1_CurrentCellDirtyStateChanged;
            dataGridView1.CellValueChanged += dataGridView1_CellValueChanged;
            dataGridView1.AllowUserToAddRows = false;
            button2.Visible = false;
            this.VisibleChanged += Customersale_VisibleChanged;
            buttonshow();
            setupproductsearch();
            setupcustomersearch();
            txtproductsearch.TextChanged += txtproductsearch_TextChanged;
        }

        private void buttonshow()
        {
            if(txtcustsearch.Focused || txtproductsearch.Focused)
            {
                button2.Visible=true;
            }
        }

        private void dataGridView1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            for (int i = e.RowIndex; i < e.RowIndex + e.RowCount; i++)
            {
                var row = dataGridView1.Rows[i];
                if (row.Cells["quantity"].Value == null || Convert.ToString(row.Cells["quantity"].Value) == "")
                {
                    row.Cells["quantity"].Value = 1;
                }
            }
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
            try
            {
                if (keyData == Keys.Enter)
                {
                    if (txtproductsearch.Focused && dgvproductsearch.Visible)
                    {
                        if (string.IsNullOrWhiteSpace(txtproductsearch.Text))
                        {
                            MessageBox.Show("Please select atleast one product.", "Product not selected", (MessageBoxButtons)MessageBoxIcon.Warning);
                            return false ;
                        }
                        if (dgvproductsearch.SelectedRows.Count == 0)
                        {
                            MessageBox.Show("Please select a product from the list.", "No product selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return false;
                        }
                        // Access selected row
                        DataGridViewRow selectedRow = dgvproductsearch.SelectedRows[0]; 

                            // Get values from the row
                            string name = selectedRow.Cells["name"].Value.ToString();
                            string description = selectedRow.Cells["description"].Value.ToString();
                            int saleprice = Convert.ToInt32(selectedRow.Cells["sale_price"].Value.ToString());

                            dataGridView1.Rows.Add(name, description, saleprice);
                            dgvproductsearch.Visible = false;
                            button2.Visible = false;
                            clearfields();                        
                        return true;
                    }

                    else if (txtcustsearch.Focused && dgvcustomersearch.Visible)
                    {
                        if (string.IsNullOrEmpty(txtcustsearch.Text))
                        {
                            MessageBox.Show("Please select atleast one product.", "Product not selected", (MessageBoxButtons)MessageBoxIcon.Warning);
                            dgvcustomersearch.Visible = false;
                            return false;
                        }
                        if (dgvcustomersearch.SelectedRows.Count == 0)
                        {
                            MessageBox.Show("Please select a customer from the list.", "No customer selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return false;
                        }

                        DataGridViewRow selectedrow = dgvcustomersearch.SelectedRows[0];
                            string name = selectedrow.Cells["name"].Value.ToString();
                            txtcustsearch.Text = name;
                            dgvcustomersearch.Visible = false;
                            button2.Visible = false;
                            return true;                   
                    }
                }

                else if (keyData == (Keys.Control | Keys.P))
                {
                    iconButton1.PerformClick();
                    return true;
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

                else if (keyData == Keys.Up)
                {
                    if (dgvproductsearch.Visible && txtproductsearch.Focused && selectedRowIndex > 0)
                    {
                        selectedRowIndex--;
                        dgvproductsearch.ClearSelection();
                        dgvproductsearch.Rows[selectedRowIndex].Selected = true;
                    }

                    else if (dataGridView1.Focused && selectedRowIndex > 0)
                    {
                        selectedRowIndex--;
                        dataGridView1.ClearSelection();
                        dataGridView1.Rows[selectedRowIndex].Selected = true;
                    }
                    else if (txtcustsearch.Focused && selectedRowIndex > 0 && dgvcustomersearch.Visible)
                    {
                        selectedRowIndex--;
                        dgvcustomersearch.ClearSelection();
                        dgvcustomersearch.Rows[selectedRowIndex].Selected = true;
                    }
                }

                else if (keyData == Keys.Down)
                {
                    if (dgvproductsearch.Visible && txtproductsearch.Focused && selectedRowIndex < dgvproductsearch.Rows.Count - 1)
                    {
                        selectedRowIndex++;
                        dgvproductsearch.ClearSelection();
                        dgvproductsearch.Rows[selectedRowIndex].Selected = true;
                    }
                    else if (dataGridView1.Focused && selectedRowIndex < dataGridView1.Rows.Count - 1)
                    {
                        selectedRowIndex++;
                        dataGridView1.ClearSelection();
                        dataGridView1.Rows[selectedRowIndex].Selected = true;
                    }
                    else if (dgvcustomersearch.Visible && txtcustsearch.Visible && selectedRowIndex < dgvcustomersearch.Rows.Count - 1)
                    {
                        selectedRowIndex++;
                        dgvcustomersearch.ClearSelection();
                        dgvcustomersearch.Rows[selectedRowIndex].Selected = true;
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
            }
            catch(Exception e)
            {
                MessageBox.Show("Error in the keyboard" + e.Message , "Error in keyboard functionalities" , MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
                return base.ProcessCmdKey(ref msg, keyData);
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
                int saleprice = Convert.ToInt32(selectedRow.Cells["sale_price"].Value.ToString());


                dataGridView1.Rows.Add(name, description, saleprice);
                dgvproductsearch.Visible = false;
                clearfields();
            }
        }

        private void setupcustomersearch()
        {
            dgvcustomersearch.Visible = false;
            dgvcustomersearch.ReadOnly = true;
            dgvcustomersearch.AutoGenerateColumns = true;
            dgvcustomersearch.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvcustomersearch.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvcustomersearch.BackgroundColor = SystemColors.Control;
            this.Controls.Add(dgvcustomersearch);
            dgvcustomersearch.Size = new System.Drawing.Size(dataGridView1.Width, dataGridView1.Height/2);
            dgvcustomersearch.Location = new System.Drawing.Point(90, 400);
            dgvcustomersearch.BringToFront();
            dgvcustomersearch.CellClick += Dgvcustomersearch_CellClick;
        }

        private void Dgvcustomersearch_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex >= 0)
            {
                DataGridViewRow selectedrow = dgvcustomersearch.Rows[e.RowIndex];

                string name = selectedrow.Cells["name"].Value.ToString();
                txtcustsearch.Text = name;
                dgvcustomersearch.Visible = false;
            }
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

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Focused)
            {
                this.ActiveControl = dataGridView1; // Ensures focus stays on the DataGridView
            }

            // Get column name being edited
            string columnName = dataGridView1.Columns[e.ColumnIndex].Name;

            // Only run calculation if quantity or discount was edited
            if (columnName == "quantity" || columnName == "discount" || columnName == "sale_price")
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
                int salePrice = Convert.ToInt32(row.Cells["sale_price"].Value ?? 0);
                int discount = Convert.ToInt32(row.Cells["discount"].Value ?? 0);
                int quantity = Convert.ToInt32(row.Cells["quantity"].Value ?? 0);

                // Validation
                if (discount < 0 || discount > salePrice)
                {
                    MessageBox.Show("Discount cannot be negative or greater than sale price.", "Invalid Discount", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    row.Cells["discount"].Value = 0;
                    discount = 0;
                }

                // Calculate total
                int discountedPrice = salePrice - discount;
                int total = discountedPrice * quantity;
                int finaltotal = salePrice * quantity;

                // Set total cell value
                row.Cells["total"].Value = total;
                row.Cells["final"].Value = finaltotal;
                totalprice();
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
            dgvproductsearch.ReadOnly = true;
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

      

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void txtproductsearch_TextChanged(object sender, EventArgs e)
        {
            dgvcustomersearch.Visible = false ;
            if (string.IsNullOrWhiteSpace(txtproductsearch.Text))
            {
                clearfields();
                button2.Visible = false;
                return;
            }
            if (dgvproductsearch.Columns.Contains("product_id"))
            {
                dgvproductsearch.Columns["product_id"].Visible = false;
            }
            button2.Visible = true;
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

        private void totalprice()
        {
            int totalprice = 0;
            int discountedprice = 0;
            int finalprice = 0;

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.IsNewRow)
                    continue;

                // Safe parsing with fallback to 0
                int total = ConvertToIntSafe(row.Cells["final"]?.Value);
                int disc = ConvertToIntSafe(row.Cells["discount"]?.Value);
                int quantity = ConvertToIntSafe(row.Cells["quantity"]?.Value);

                discountedprice += (disc * quantity);
                totalprice += total;
            }

            finalprice = totalprice - discountedprice;

            txtfinalprice.Text = finalprice.ToString();
            txtfinaldiscount.Text = discountedprice.ToString();
            totalwithoutdisc.Text = totalprice.ToString();
        }

        private void txtfinalprice_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtcustsearch_TextChanged(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(txtcustsearch.Text))
            {
                dgvcustomersearch.Visible = false;
                button2.Visible = false;
                return;                  
            }
            buttonshow();
            dgvcustomersearch.Visible = true;
            if(dgvcustomersearch.Columns.Contains("customer_id"))
            {
                dgvcustomersearch.Columns["customer_id"].Visible = false;
            }
            DataTable dt = _customersaledl.getallcustomer(txtcustsearch.Text);
            dgvcustomersearch.DataSource = dt;
        }

        private void savetempsale()
        {
            try
            {
                var data = new Temporarycustomersale
                {
                    customername = txtcustsearch.Text ?? "",
                    productname = txtproductsearch.Text ?? "",
                    totaldiscount = int.TryParse(txtfinaldiscount.Text, out var discount) ? discount : 0,
                    finalpriceafterdisc = decimal.TryParse(txtfinalprice.Text, out var finalprice) ? finalprice : 0,
                    totalprice = decimal.TryParse(totalwithoutdisc.Text, out var total) ? total : 0,
                    date = dateTimePicker1.Value,
                    items = dataGridView1.Rows
                        .Cast<DataGridViewRow>()
                        .Where(r => !r.IsNewRow)
                        .Select(r => new saleitems
                        {
                            productname = r.Cells["name"]?.Value?.ToString() ?? "",
                            description = r.Cells["description"]?.Value?.ToString() ?? "",
                            unitprice = ConvertToIntSafe(r.Cells["sale_price"]?.Value),
                            quantity = ConvertToIntSafe(r.Cells["quantity"]?.Value),
                            discount = ConvertToIntSafe(r.Cells["discount"]?.Value),
                            total = ConvertToIntSafe(r.Cells["total"]?.Value),
                            finalprice = ConvertToIntSafe(r.Cells["final"]?.Value)
                        })
                        .ToList()
                };

                string json = JsonConvert.SerializeObject(data, Formatting.Indented);
                File.WriteAllText("Temporarydata.json", json);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving temporary sale: " + ex.Message);
            }
        }

        private int ConvertToIntSafe(object value)
        {
            return int.TryParse(value?.ToString(), out var result) ? result : 0;
        }


        private void loadtempsale()
        {
            if(!File.Exists("Temporarydata.json")) return;
            string json = File.ReadAllText("Temporarydata.json");
            var data = JsonConvert.DeserializeObject<Temporarycustomersale>(json);
            txtcustsearch.Text = data.customername;
            txtproductsearch.Text = data.productname;
            txtfinaldiscount.Text = data.totaldiscount.ToString();
            txtfinalprice.Text = data.finalpriceafterdisc.ToString();
            totalwithoutdisc.Text = data.totalprice.ToString();
            dateTimePicker1.Value = data.date;
            dataGridView1.Rows.Clear();
            foreach(var item in data.items)
            {
                dataGridView1.Rows.Add(item.productname, item.description , item.unitprice , item.quantity , item.discount ,item.total , item.finalprice);
            }
        }

        private void Customersale_Load(object sender, EventArgs e)
        {
            loadtempsale();
        }
        private void Customersale_FormClosing(object sender, FormClosingEventArgs e)
        {
            savetempsale();
        }

        private void Customersale_VisibleChanged(object sender, EventArgs e)
        {
            if (!this.Visible)
            {
                savetempsale();
            }
        }

        private void clearallfields()
        {
            txtcustsearch.Clear();
            txtproductsearch.Clear();
            txtfinaldiscount.Clear();
            txtfinalprice.Clear();
            totalwithoutdisc.Clear();
            dataGridView1.RowCount = 0;
            txtpaidamount.Clear();
            //dataGridView1.ClearSelection();
        }

        private void iconButton1_Click(object sender, EventArgs e)
        {
            try
            {
                if(string.IsNullOrEmpty(txtpaidamount.Text))
                {
                    DialogResult result1 = MessageBox.Show("Dont you want to enter the paid amount? " , "payment not enterd" , MessageBoxButtons.YesNoCancel, MessageBoxIcon.Error);
                    if(result1 == DialogResult.Yes)
                    {
                        txtpaidamount.Text = "0";
                    }
                    else
                    {
                        return;
                    }
                }
                SavehthermalPdfInvoice();
                int id = _customersaledl.getcustomerid(txtcustsearch.Text);
                bool result = _customersaledl.SaveDataToDatabase(id, dateTimePicker1.Value, Convert.ToInt32(txtfinalprice.Text), Convert.ToInt32(txtpaidamount.Text), dataGridView1);
                if (result)
                {
                    MessageBox.Show("data saved successfully");
                    clearallfields();
                }
                if (File.Exists("Temporarydata.json"))
                {
                    File.Delete("Temporarydata.json");
                }

                else
                {
                    MessageBox.Show("Data not saved to the database");
                    return;
                }
            }
            catch  (Exception ex) 
            {
                MessageBox.Show("Error in saving the data to database " + ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dgvcustomersearch.Visible)
            {
                dgvcustomersearch.Visible = false;
            }
            
            if(dgvproductsearch.Visible)
            {
                dgvproductsearch.Visible = false;
            }
            button2.Visible = false;
        }

        private void SavehthermalPdfInvoice()
        {
            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "PDF files (*.pdf)|*.pdf";
                saveDialog.Title = "Save PDF Invoice";
                saveDialog.FileName = $"Invoice_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        Customersaledl.CreateThermalReceiptPdf(dataGridView1, saveDialog.FileName, txtcustsearch.Text.Trim(), Convert.ToDecimal(txtfinalprice.Text), Convert.ToDecimal(txtpaidamount.Text));

                        MessageBox.Show("PDF saved successfully!", "PDF", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error generating PDF:\n" + ex.Message, "PDF Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

    }
}
