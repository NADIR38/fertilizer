using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using fertilizesop.BL.Models;
using fertilizesop.DL;
using System.IO;
using fertilizesop.BL.Bl;

using Newtonsoft.Json;

namespace fertilizesop.UI
{
    public partial class PlacingOrder : Form

    {
        OrderBl o=new OrderBl();
        private DataGridView dgvProductSearch;
        private DataTable allProducts; // holds all products from DB or dummy
        private int? editingRowIndex = null;
        int selectedProductId = 0;
        int SelctedOrder;
        string supplierName;
        public PlacingOrder(int passedOrderId, string supplierName)
        {
            InitializeComponent();
            SelctedOrder = passedOrderId;
            ConfigureInvoiceGrid();
            SetupSearchGrid();
            LoadProductData(); // Fill allProducts
            dgvInvoice.AllowUserToAddRows = false;
            this.supplierName = supplierName;
            this.KeyPreview = true; // Put this in OrdersMain constructor
            

        }

        private void ConfigureInvoiceGrid()
        {
            dgvInvoice.Columns.Clear();

            dgvInvoice.Columns.Add("product_id", "Product ID");
            dgvInvoice.Columns["product_id"].Visible = false; // hide it
            dgvInvoice.Columns.Add("Name", "Product Name");
            dgvInvoice.Columns.Add("Description", "Description");
            dgvInvoice.Columns.Add("Price", "Price");
            dgvInvoice.Columns.Add("Quantity", "Quantity");
            dgvInvoice.Columns.Add("Total", "Total");

            dgvInvoice.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }
        private void LoadProductData()
        {
            allProducts = o.GetProducts();

        }

        private void SetupSearchGrid()
        {
            dgvProductSearch = new DataGridView
            {
                Location = new Point(txtProductName.Left, txtProductName.Bottom + 5),
                Width = txtProductName.Width + 1200,
                Height = 580,
                Visible = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AllowUserToAddRows = false,
                MultiSelect = false,
                BackgroundColor = SystemColors.Window,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BorderStyle = BorderStyle.Fixed3D
            };
            dgvProductSearch.Columns.Add("product_id", "product_id");
            dgvProductSearch.Columns.Add("Name", "Product Name");
            dgvProductSearch.Columns.Add("Description", "Description");
            dgvProductSearch.Columns.Add("Price", "sale_price");

            //dgvProductSearch.Columns["product_id"].Visible = false;
            dgvProductSearch.CellClick += DgvProductSearch_CellClick;
            this.Controls.Add(dgvProductSearch);
        }

        private void DgvProductSearch_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var value = dgvProductSearch.Rows[e.RowIndex].Cells["product_id"].Value;
                if (value != null && int.TryParse(value.ToString(), out int id))
                {
                    selectedProductId = id;
                }
                else
                {
                    // Handle the error (e.g., show a message)
                    MessageBox.Show("Invalid product ID.");
                }

                string name = dgvProductSearch.Rows[e.RowIndex].Cells["Name"].Value.ToString();
                string desc = dgvProductSearch.Rows[e.RowIndex].Cells["Description"].Value.ToString();
                string prices = dgvProductSearch.Rows[e.RowIndex].Cells["Price"].Value.ToString();


                txtProductName.Text = name;
                txtdescription.Text = desc;
                Price.Text = prices;

                dgvProductSearch.Visible = false;
                txtQuantity.Focus();
            }
        }

        private void CalculateTotalAmount()
        {
            decimal total = 0;

            foreach (DataGridViewRow row in dgvInvoice.Rows)
            {
                if (row.Cells["Total"]?.Value != null)
                {
                    decimal rowTotal;
                    if (decimal.TryParse(row.Cells["Total"].Value.ToString(), out rowTotal))
                    {
                        total += rowTotal;
                    }
                }
            }

            texttotal.Text = total.ToString("0.00"); // format to 2 decimal places
        }

        private void SaveOrder()
        {
            if (string.IsNullOrWhiteSpace(txtProductName.Text) || string.IsNullOrWhiteSpace(txtQuantity.Text) || string.IsNullOrWhiteSpace(txtdescription.Text) || string.IsNullOrWhiteSpace(Price.Text))
            {
                MessageBox.Show("Please fill in product name and quantity.");
                return;
            }

            if (!int.TryParse(txtQuantity.Text.Trim(), out int quantity))
            {
                MessageBox.Show("Quantity should be in digits only.", "Invalid Quantity", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtQuantity.Focus();
                return;
            }

            if (selectedProductId == 0)
            {
                MessageBox.Show("Please select a valid product from the list.", "Invalid Product", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Create OrderDetail object
            OrderDetail detail = new OrderDetail(SelctedOrder, selectedProductId, quantity);


            decimal price = Convert.ToDecimal(Price.Text.Trim());
            decimal total = price * quantity;

            try
            {
                if (editingRowIndex != null)
                {

                    o.UpdateOrderDetail(detail);


                    dgvInvoice.Rows[(int)editingRowIndex].SetValues(
                        selectedProductId,
                        txtProductName.Text,
                        txtdescription.Text,
                        Price.Text,
                        txtQuantity.Text,
                        total.ToString("0.00")
                    );

                    editingRowIndex = null;
                    CalculateTotalAmount();
                    MessageBox.Show("Row and database updated successfully!");

                }
                else
                {

                    o.InsertOrderDetail(detail);


                    dgvInvoice.Rows.Add(
                        selectedProductId,
                        txtProductName.Text,
                        txtdescription.Text,
                        Price.Text,
                        txtQuantity.Text,
                        total.ToString("0.00")
                    );

                    CalculateTotalAmount();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database error: " + ex.Message);
            }

            // Clear all input fields
            txtProductName.Clear();
            txtdescription.Clear();
            Price.Clear();
            txtQuantity.Clear();
            selectedProductId = 0;
        }

        private void btnadd_Click(object sender, EventArgs e)
        {
            SaveOrder();
        }
        private void btnedit_Click(object sender, EventArgs e)
        {

            if (dgvInvoice.SelectedRows.Count > 0)
            {
                var row = dgvInvoice.SelectedRows[0];
                txtProductName.Text = row.Cells["Name"].Value?.ToString();
                txtdescription.Text = row.Cells["Description"].Value?.ToString();
                txtQuantity.Text = row.Cells["Quantity"].Value?.ToString();
                Price.Text = row.Cells["Price"].Value?.ToString();

                editingRowIndex = row.Index;
            }
            else
            {
                MessageBox.Show("Please select a row to edit.");
            }
        }

        private void DeleteSelectedRow()
        {
            if (dgvInvoice.SelectedRows.Count > 0)
            {
                DialogResult result = MessageBox.Show("Are you sure you want to delete this item?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                   
                    int id = Convert.ToInt32(dgvInvoice.SelectedRows[0].Cells["Product_ID"].Value);

                   
                    bool success = o.Delete(id); 

                    if (success)
                    {
                        dgvInvoice.Rows.RemoveAt(dgvInvoice.SelectedRows[0].Index);
                        CalculateTotalAmount();
                        MessageBox.Show("Item deleted successfully.");
                    }
                    else
                    {
                        MessageBox.Show("Failed to delete item from database.");
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a row to delete.");
            }
        }

        private void btndelete_Click(object sender, EventArgs e)
        {
            DeleteSelectedRow();
        }

        private void txtProductName_TextChanged_1(object sender, EventArgs e)
        {
            string search = txtProductName.Text.Trim().ToLower();
            if (allProducts == null || string.IsNullOrEmpty(search))
            {
                dgvProductSearch.Visible = false;
                return;
            }

            var filtered = allProducts.AsEnumerable()
        .Where(row =>
                    row.Field<string>("name").ToLower().Contains(search) ||
                    row.Field<string>("description").ToLower().Contains(search) ||
                   row.Field<int>("sale_price").ToString().Contains(search)
             )
            .ToList();

            dgvProductSearch.Rows.Clear();

            if (filtered.Any())
            {
                foreach (var row in filtered)
                {
                    dgvProductSearch.Rows.Add(
                        row["product_id"].ToString(),
                        row["name"].ToString(),
                        row["description"].ToString(),
                        row["sale_price"].ToString()
                    );
                }
                dgvProductSearch.Visible = true;
                dgvProductSearch.BringToFront();
            }
            else
            {
                dgvProductSearch.Visible = false;
            }
        }

        private void btnsave_Click(object sender, EventArgs e)
        {
            

            // ✅ Move the actual logic outside the if
            DateTime Date = DateTime.Now;

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "PDF Files (*.pdf)|*.pdf";
            saveFileDialog.Title = "Save Order Invoice";
            saveFileDialog.FileName = "OrderInvoice.pdf";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = saveFileDialog.FileName;
                o.CreateOrderInvoicePdf(dgvInvoice, filePath, supplierName, Date);
                MessageBox.Show("PDF Generated Successfully.");

                if (File.Exists("TempInvoice.json"))
                {
                    File.Delete("TempInvoice.json");
                }

                ClearInvoiceForm();
            }
            else
            {
                MessageBox.Show("PDF generation was cancelled.");
            }
        }

        private void ClearInvoiceForm()
        {
           
            dgvInvoice.Rows.Clear();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Validate data
            if (dgvInvoice.Rows.Count == 0)
            {
                MessageBox.Show("No items to print in invoice.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            
            DateTime purchaseDate = DateTime.Now;
            o.PrintOrderInvoiceDirectly(dgvInvoice, supplierName, purchaseDate);


            if (File.Exists("TempInvoice.json"))
            {
                File.Delete("TempInvoice.json");
            }


            ClearInvoiceForm();

            MessageBox.Show("Invoice printed", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // storing data functions here

        private void SaveTempInvoice()
        {
            var data = new TempInvoiceData
            {
                Items = dgvInvoice.Rows
                    .Cast<DataGridViewRow>()
                    .Where(r => !r.IsNewRow)
                    .Select(r => new InvoiceItem
                    {
                        ProductName = r.Cells["Name"].Value?.ToString(),
                        Description = r.Cells["Description"].Value?.ToString(),
                        Quantity = int.TryParse(r.Cells["Quantity"].Value?.ToString(), out int q) ? q : 0,
                        Price = int.TryParse(r.Cells["Price"].Value?.ToString(), out int p) ? p : 0,
                        Total = int.TryParse(r.Cells["Total"].Value?.ToString(), out int t) ? t : 0
                    }).ToList()
            };

            string json = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText("TempInvoice.json", json);
        }

        private void LoadTempInvoice()
        {
            if (!File.Exists("TempInvoice.json")) return;

            string json = File.ReadAllText("TempInvoice.json");
            var data = JsonConvert.DeserializeObject<TempInvoiceData>(json);
            dgvInvoice.Rows.Clear();
            foreach (var item in data.Items)
            {
                dgvInvoice.Rows.Add(item.ProductName, item.Description, item.Quantity, item.Price, item.Total);
            }
        }

        //keys function 

        private int selectedRowIndex;
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            // When Enter is pressed and focus is NOT on DataGridView (to avoid accidental row edits)
            if (keyData == Keys.Enter && !(ActiveControl is DataGridView))
            {
                btnadd.PerformClick(); // Simulate button click
                return true; // Mark event as handled
            }

            else if (keyData == (Keys.Control | Keys.S))
            {
                btnsave.PerformClick();
                return true;
            }

            else if (keyData == Keys.Delete)
            {
                btndelete.PerformClick();
                return true;
            }

            else if (keyData == Keys.Up)
            {
                if (dgvProductSearch.Visible && selectedRowIndex > 0)
                {
                    selectedRowIndex--;
                    dgvProductSearch.ClearSelection();
                    dgvProductSearch.Rows[selectedRowIndex].Selected = true;
                    return true;
                }
            }
            else if (keyData == Keys.Down)
            {
                if (dgvProductSearch.Visible && selectedRowIndex < dgvProductSearch.Rows.Count - 1)
                {
                    selectedRowIndex++;
                    dgvProductSearch.ClearSelection();
                    dgvProductSearch.Rows[selectedRowIndex].Selected = true;
                    return true;
                }
            }

            return base.ProcessCmdKey(ref msg, keyData); // Allow default behavior
        }

        private void PlacingOrder_FormClosing(object sender, FormClosingEventArgs e)
        {
            //SaveTempInvoice();
        }

        private void PlacingOrder_Load(object sender, EventArgs e)
        {
            //LoadTempInvoice();
        }

        private void PlacingOrder_VisibleChanged(object sender, EventArgs e)
        {
            if (!this.Visible)
            {
                SaveTempInvoice();
            }
        }
    }

}
