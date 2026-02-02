using fertilizesop.BL.Bl;
using fertilizesop.BL.Models;
using fertilizesop.Interfaces.BLInterfaces;
using KIMS;
using Microsoft.Extensions.DependencyInjection;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace fertilizesop.UI
{
    public partial class Batchform : Form
    {
        private readonly IBatchesBl ibl;
        private readonly ISupplierBillBl ibr;
        private readonly IbatchdetailsBl detailsBl; // Injected

        private int selectedid = -1;
        
        // Master-Detail Controls
        private DataGridView dgvDetails;
        private Panel panelDetails;

        // Unified Add Batch Controls
        private Panel panelAddBatch;
        private TextBox txtNewBatchName;
        private DateTimePicker dtpNewBatchDate;
        private ComboBox cmbNewSupplier;
        private DataGridView dgvNewBatchItems;
        private DataGridView dgvProductSearch;
        private TextBox txtProductSearch;
        private TextBox txtQty, txtCost, txtSale;
        private int selectedProductIdToAdd;
        private string selectedProductNameToAdd;

        public Batchform(IBatchesBl ibl, ISupplierBillBl ibr, IbatchdetailsBl detailsBl)
        {
            InitializeComponent();
            this.ibl = ibl;
            this.ibr = ibr;
            this.detailsBl = detailsBl;

            UIHelper.StyleGridView(dataGridView2);
            paneledit.Visible = false;
            UIHelper.ApplyButtonStyles(dataGridView2);
            this.KeyPreview = true;
            this.KeyDown += CustomerForm_KeyDown;
            //PanelBill.Visible = false;

            InitializeMasterDetailControls();
            InitializeAddBatchPanel();
        }

        private void InitializeMasterDetailControls()
        {
            // Panel for Details (Bottom Half)
            panelDetails = new Panel
            {
                Height = 300,
                Dock = DockStyle.Bottom,
                Visible = false,
                BackColor = Color.WhiteSmoke
            };
            this.Controls.Add(panelDetails);

            // Label for Details
            Label lbl = new Label { Text = "Batch Details", Dock = DockStyle.Top, Height = 30, Font = new Font("Segoe UI", 12, FontStyle.Bold) };
            panelDetails.Controls.Add(lbl);

            // Grid for Details
            dgvDetails = new DataGridView();
            dgvDetails.Dock = DockStyle.Fill;
            UIHelper.StyleGridView(dgvDetails);
            panelDetails.Controls.Add(dgvDetails);

            // Close Button for Details Panel
            Button btnCloseDetails = new Button { Text = "X", Width = 30, Height = 30, Dock = DockStyle.Right, FlatStyle = FlatStyle.Flat };
            btnCloseDetails.Click += (s, e) => panelDetails.Visible = false;
            lbl.Controls.Add(btnCloseDetails);
        }

        // --- Serialization Helper ---
        private string DraftFilePath => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "temp_batch.json");

        public class TempBatchData
        {
            public string BatchName { get; set; }
            public string SupplierName { get; set; }
            public DateTime BatchDate { get; set; }
            public List<TempBatchItem> Items { get; set; } = new List<TempBatchItem>();
        }

        public class TempBatchItem
        {
            public int ProductId { get; set; }
            public string ProductName { get; set; }
            public decimal Cost { get; set; }
            public decimal Sale { get; set; }
            public int Qty { get; set; }
        }

        private void SaveDraft()
        {
            try
            {
                var data = new TempBatchData
                {
                    BatchName = txtNewBatchName.Text,
                    SupplierName = cmbNewSupplier.Text,
                    BatchDate = dtpNewBatchDate.Value,
                    Items = new List<TempBatchItem>()
                };

                foreach (DataGridViewRow row in dgvNewBatchItems.Rows)
                {
                    if (row.IsNewRow) continue;
                    data.Items.Add(new TempBatchItem
                    {
                        ProductId = Convert.ToInt32(row.Cells["Id"].Value),
                        ProductName = row.Cells["Name"].Value.ToString(),
                        Cost = Convert.ToDecimal(row.Cells["Cost"].Value),
                        Sale = Convert.ToDecimal(row.Cells["Sale"].Value),
                        Qty = Convert.ToInt32(row.Cells["Qty"].Value)
                    });
                }

                File.WriteAllText(DraftFilePath, JsonConvert.SerializeObject(data));
            }
            catch { /* Ignore errors during auto-save */ }
        }

        private void RestoreDraft()
        {
            if (!File.Exists(DraftFilePath)) return;

            if (MessageBox.Show("Unsaved draft found. Do you want to restore it?", "Restore Draft", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    var data = JsonConvert.DeserializeObject<TempBatchData>(File.ReadAllText(DraftFilePath));
                    if (data != null)
                    {
                        txtNewBatchName.Text = data.BatchName;
                        cmbNewSupplier.Text = data.SupplierName;
                        dtpNewBatchDate.Value = data.BatchDate;
                        
                        dgvNewBatchItems.Rows.Clear();
                        foreach (var item in data.Items)
                        {
                            dgvNewBatchItems.Rows.Add(item.ProductId, item.ProductName, item.Cost, item.Sale, item.Qty);
                        }
                        panelAddBatch.Visible = true;
                        panelAddBatch.BringToFront();
                    }
                }
                catch (Exception ex) 
                { 
                    MessageBox.Show("Failed to restore draft: " + ex.Message); 
                }
            }
        }

        // --- Improved UI Construction ---
        private void InitializeAddBatchPanel()
        {
            panelAddBatch = new Panel
            {
                Size = new Size(1000, 650),
                Location = new Point(50, 40),
                BackColor = Color.White,
                Visible = false,
                BorderStyle = BorderStyle.None // We'll add a header panel
            };
            this.Controls.Add(panelAddBatch);
            panelAddBatch.BringToFront();

            // Header Panel
            Panel pnlHeader = new Panel { Dock = DockStyle.Top, Height = 50, BackColor = Color.FromArgb(0, 64, 0) }; // Dark Green
            Label lblTitle = new Label { Text = "  New Batch Entry", Dock = DockStyle.Left, ForeColor = Color.White, Font = new Font("Segoe UI", 14, FontStyle.Bold), TextAlign = ContentAlignment.MiddleLeft, AutoSize = true };
            pnlHeader.Controls.Add(lblTitle);
            
            Button btnClose = new Button { Text = "X", Dock = DockStyle.Right, Width = 50, FlatStyle = FlatStyle.Flat, ForeColor = Color.White, BackColor = Color.DarkRed };
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.Click += (s, e) => { panelAddBatch.Visible = false; };
            pnlHeader.Controls.Add(btnClose);
            panelAddBatch.Controls.Add(pnlHeader);


            pnlHeader.Controls.Add(btnClose);
            panelAddBatch.Controls.Add(pnlHeader);


            // 1. Batch Info Group
            GroupBox grpInfo = new GroupBox { Text = "Batch Information", Location = new Point(20, 60), Size = new Size(960, 80), Font = new Font("Segoe UI", 10, FontStyle.Regular) };
            
            grpInfo.Controls.Add(new Label { Text = "Batch Name:", Location = new Point(20, 30), AutoSize = true });
            txtNewBatchName = new TextBox { Location = new Point(110, 27), Width = 250 };
            txtNewBatchName.TextChanged += (s, e) => SaveDraft(); // Trigger Save
            grpInfo.Controls.Add(txtNewBatchName);

            grpInfo.Controls.Add(new Label { Text = "Supplier:", Location = new Point(380, 30), AutoSize = true });
            cmbNewSupplier = new ComboBox { Location = new Point(450, 27), Width = 200, DropDownStyle = ComboBoxStyle.DropDown };
            cmbNewSupplier.SelectedIndexChanged += (s, e) => SaveDraft(); // Trigger Save
            grpInfo.Controls.Add(cmbNewSupplier);
            txtNewBatchName.Enter += (s, e) => LoadSuppliersForCombo();

            grpInfo.Controls.Add(new Label { Text = "Date:", Location = new Point(680, 30), AutoSize = true });
            dtpNewBatchDate = new DateTimePicker { Location = new Point(730, 27), Width = 200 };
            grpInfo.Controls.Add(dtpNewBatchDate);

            panelAddBatch.Controls.Add(grpInfo);


            // 2. Product Entry Group
            GroupBox grpEntry = new GroupBox { Text = "Add Products", Location = new Point(20, 150), Size = new Size(960, 100), Font = new Font("Segoe UI", 10, FontStyle.Regular) };
            
            int yRow1 = 40;
            grpEntry.Controls.Add(new Label { Text = "Product:", Location = new Point(20, yRow1), AutoSize = true });
            txtProductSearch = new TextBox { Location = new Point(90, yRow1-3), Width = 250};
            txtProductSearch.TextChanged += TxtProductSearch_TextChanged;
            grpEntry.Controls.Add(txtProductSearch);

            grpEntry.Controls.Add(new Label { Text = "Cost Price:", Location = new Point(360, yRow1), AutoSize = true });
            txtCost = new TextBox { Location = new Point(440, yRow1-3), Width = 100 };
            grpEntry.Controls.Add(txtCost);

            grpEntry.Controls.Add(new Label { Text = "Sale Price:", Location = new Point(560, yRow1), AutoSize = true });
            txtSale = new TextBox { Location = new Point(640, yRow1-3), Width = 100 };
            grpEntry.Controls.Add(txtSale);

            grpEntry.Controls.Add(new Label { Text = "Qty:", Location = new Point(760, yRow1), AutoSize = true });
            txtQty = new TextBox { Location = new Point(800, yRow1-3), Width = 60 };
            grpEntry.Controls.Add(txtQty);

            Button btnAddProduct = new Button { Text = "Add Item", Location = new Point(880, yRow1-5), Width = 70, Height = 30, BackColor = Color.Teal, ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
            btnAddProduct.Click += BtnAddProduct_Click;
            grpEntry.Controls.Add(btnAddProduct);

            // Product Search Grid (Floating)
            dgvProductSearch = new DataGridView { Location = new Point(130, 280), Width = 400, Height = 200, Visible = false }; // Absolute position relative to Panel
            UIHelper.StyleGridView(dgvProductSearch);
            dgvProductSearch.CellClick += DgvProductSearch_CellClick;
            // Add to panelAddBatch to float over content
            panelAddBatch.Controls.Add(dgvProductSearch); 
            dgvProductSearch.BringToFront();

            panelAddBatch.Controls.Add(grpEntry);


            // 3. Current Items Grid
            dgvNewBatchItems = new DataGridView { Location = new Point(20, 270), Width = 960, Height = 300 };
            dgvNewBatchItems.Columns.Add("Id", "Id");
            dgvNewBatchItems.Columns.Add("Name", "Product");
            dgvNewBatchItems.Columns.Add("Cost", "Cost");
            dgvNewBatchItems.Columns.Add("Sale", "Sale");
            dgvNewBatchItems.Columns.Add("Qty", "Qty");
            UIHelper.StyleGridView(dgvNewBatchItems);
            dgvNewBatchItems.RowsAdded += (s,e) => SaveDraft();
            dgvNewBatchItems.RowsRemoved += (s,e) => SaveDraft();
            panelAddBatch.Controls.Add(dgvNewBatchItems);


            // 4. Footer Buttons
            Button btnSaveBatch = new Button { Text = "Save Batch", Location = new Point(840, 590), Width = 140, Height = 40, BackColor = Color.Green, ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
            btnSaveBatch.Click += BtnSaveBatch_Click;
            panelAddBatch.Controls.Add(btnSaveBatch);

        }

        private void LoadSuppliersForCombo()
        {
             // Optimization: Load only once or when needed
             if (cmbNewSupplier.Items.Count == 0)
             {
                 var suppliers = ibl.getsuppliernames("");
                 cmbNewSupplier.Items.AddRange(suppliers.ToArray());
             }
        }

        private void TxtProductSearch_TextChanged(object sender, EventArgs e)
        {
            string text = txtProductSearch.Text.Trim();
            if (string.IsNullOrEmpty(text)) { dgvProductSearch.Visible = false; return; }

            var list = DatabaseHelper.Instance.GetProductsByNames(text);
            dgvProductSearch.DataSource = list;
            // Hide unwanted columns
            if (dgvProductSearch.Columns["Id"] != null) dgvProductSearch.Columns["Id"].Visible = false;
            dgvProductSearch.Visible = true;
            dgvProductSearch.BringToFront();
        }

        private void DgvProductSearch_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var row = dgvProductSearch.Rows[e.RowIndex];
            selectedProductIdToAdd = Convert.ToInt32(row.Cells["Id"].Value);
            selectedProductNameToAdd = row.Cells["Name"].Value.ToString();
            
            // Auto-fill Prices
            if (row.Cells["LastCost"].Value != null)
                txtCost.Text = row.Cells["LastCost"].Value.ToString();

            if (row.Cells["Price"].Value != null)
                txtSale.Text = row.Cells["Price"].Value.ToString();
            
            txtProductSearch.Text = selectedProductNameToAdd;
            dgvProductSearch.Visible = false;
            txtCost.Focus();
        }

        private void BtnAddProduct_Click(object sender, EventArgs e)
        {
            if (selectedProductIdToAdd == 0) return;
            if (!decimal.TryParse(txtCost.Text, out decimal cost)) return;
            if (!decimal.TryParse(txtSale.Text, out decimal sale)) return;
            if (!int.TryParse(txtQty.Text, out int qty)) return;

            dgvNewBatchItems.Rows.Add(selectedProductIdToAdd, selectedProductNameToAdd, cost, sale, qty);
            
            // Allow duplicates? Logic says maybe aggregate, but let's just add new row for now
            txtProductSearch.Clear(); txtCost.Clear(); txtSale.Clear(); txtQty.Clear();
            selectedProductIdToAdd = 0;
            SaveDraft(); // Explicit save
        }

        private void BtnSaveBatch_Click(object sender, EventArgs e)
        {
            string bName = txtNewBatchName.Text.Trim();
            string sName = cmbNewSupplier.Text.Trim();
            if (string.IsNullOrEmpty(bName) || string.IsNullOrEmpty(sName)) { MessageBox.Show("Batch Name and Supplier are required"); return; }
            if (dgvNewBatchItems.Rows.Count == 0) { MessageBox.Show("Add at least one item"); return; }

            try
            {
                // 1. Create Batch
                var batch = new Batches(0, bName, dtpNewBatchDate.Value, sName, "", 0);
                bool batchAdded = ibl.addbatches(batch);
                if (!batchAdded) { MessageBox.Show("Failed to create batch header"); return; }

                // 2. Add Details
                foreach (DataGridViewRow row in dgvNewBatchItems.Rows)
                {
                     if (row.IsNewRow) continue;
                     int pid = Convert.ToInt32(row.Cells["Id"].Value);
                     string pname = row.Cells["Name"].Value.ToString();
                     decimal cost = Convert.ToDecimal(row.Cells["Cost"].Value);
                     decimal sale = Convert.ToDecimal(row.Cells["Sale"].Value);
                     int qty = Convert.ToInt32(row.Cells["Qty"].Value);

                     var detail = new BatchDetails(0, bName, cost, sale, pid, pname, qty);
                     detailsBl.adddetails(detail);
                }

                MessageBox.Show("Batch saved successfully!");
                panelAddBatch.Visible = false;
                
                // Clear Draft
                if (File.Exists(DraftFilePath)) File.Delete(DraftFilePath);
                
                load(); // Refresh main grid
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void CustomerForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.R)
            {
                pictureBox1_Click(sender, e);
                e.Handled = true;
            }
            else if (e.Control && e.KeyCode == Keys.S)
            {
                if (paneledit.Visible)
                {
                    btnsave.PerformClick();
                    e.Handled = true;
                }
            }
          
            else if (e.Control && e.KeyCode == Keys.A)
            {
                    iconButton9.PerformClick();
                    e.Handled = true;
            }
            else if (e.Control && e.KeyCode == Keys.Enter)
            {
                if (!paneledit.Visible && dataGridView2.Focused)
                {
                   // Open Details in Panel
                   int id = Convert.ToInt32(dataGridView2.CurrentRow.Cells["batch_id"].Value);
                   ShowDetails(id);
                   e.Handled = true;
                }
            }

            else if (e.KeyCode == Keys.Escape)
            {
                if (paneledit.Visible)
                {
                    paneledit.Visible = false;
                    e.Handled = true;
                }
                else if (panelDetails.Visible)
                {
                    panelDetails.Visible = false;
                    e.Handled = true;
                }
            }
        }
        private void editSelectedCustomer()
        {
            if (dataGridView2.CurrentRow != null)
            {
                var row = dataGridView2.CurrentRow;
                selectedid= Convert.ToInt32(row.Cells["batch_id"].Value);
                txtname.Text = row.Cells["batch_name"].Value?.ToString();
                dateTimePicker1.Text = row.Cells["received_date"].Value?.ToString();
                comboBox1.Text = row.Cells["supplier_name"].Value?.ToString();

                UIHelper.RoundPanelCorners(paneledit, 20);
                UIHelper.ShowCenteredPanel(this, paneledit);
            }
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string text = textBox1.Text.Trim();
            if (string.IsNullOrEmpty(text))
            {
                load();
                return;
            }
            var list = ibl.SearchBatches(text);
            dataGridView2.Columns.Clear();
            dataGridView2.DataSource = list;
            dataGridView2.Columns["batch_id"].Visible = false;

            UIHelper.AddButtonColumn(dataGridView2, "Edit", "Edit", "Edit");
        }

        private void iconButton9_Click(object sender, EventArgs e)
        {
            // Open consolidated Add Batch Panel
            var f= Program.ServiceProvider.GetRequiredService<UnifiedBatchPurchaseForm>();
            f.ShowDialog(this);

        }

        private void btnsave_Click(object sender, EventArgs e)
        {
            string name = txtname.Text.Trim();
            DateTime date = dateTimePicker1.Value;
            string supplier_name = comboBox1.Text.Trim();

            // ✅ Input Validation
            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Please enter batch name.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtname.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(supplier_name))
            {
                MessageBox.Show("Please enter supplier name.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                comboBox1.Focus();
                return;
            }

            try
            {
                var batch = new Batches(selectedid, name, date, supplier_name, "",0);

                var result = ibl.UpdateBatch(batch);

                if (result)
                {
                    MessageBox.Show("Batch Updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    paneledit.Visible = false;
                    load();
                }
                else
                {
                    MessageBox.Show("Failed to Update batch. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (ArgumentNullException ex)
            {
                MessageBox.Show("Missing required data: " + ex.Message, "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show("Invalid input: " + ex.Message, "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Database error: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unexpected error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btncancle1_Click(object sender, EventArgs e)
        {
            paneledit.Visible = false;
        }

        private void load()
        {
            var list=ibl.GetAllBatches();
            dataGridView2.Columns.Clear();
            dataGridView2.DataSource = list;
            dataGridView2.Columns["batch_id"].Visible = false;

            UIHelper.AddButtonColumn(dataGridView2,"Edit", "Edit", "Edit");
            UIHelper.AddButtonColumn(dataGridView2, "Details", "Details", "Details");
        }

        private void Batchform_Load(object sender, EventArgs e)
        {
            load();
            dataGridView2.Focus();
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            var columnName = dataGridView2.Columns[e.ColumnIndex].Name;
            var row = dataGridView2.Rows[e.RowIndex];
            selectedid = Convert.ToInt32(row.Cells["batch_id"].Value);

            if (columnName == "Edit")
            {
                txtname.Text = row.Cells["batch_name"].Value?.ToString();
                dateTimePicker1.Text = row.Cells["received_date"].Value?.ToString();
                comboBox1.Text = row.Cells["supplier_name"].Value?.ToString();


                UIHelper.RoundPanelCorners(paneledit, 20);
                UIHelper.ShowCenteredPanel(this, paneledit);
            }
            else if (columnName == "Details")
            {
                ShowDetails(selectedid);
            }
        }

        private void ShowDetails(int batchId)
        {
            // NEW: Open separate form instead of inline panel
            var detailsForm = Program.ServiceProvider.GetRequiredService<BatchDetailsform>();
            detailsForm.BatchId = batchId;
            detailsForm.ShowDialog();
        }


        private void iconPictureBox2_Click(object sender, EventArgs e)
        {
            var f = Program.ServiceProvider.GetRequiredService<Addsupplier>();
            f.ShowDialog(this);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            load();
            textBox1.Clear();   
        }

        private void btnbill_Click(object sender, EventArgs e)
        {
            //if (dataGridView2.CurrentRow == null)
            //{
            //    MessageBox.Show("Please select a row first.");
            //    return;
            //}

            //string batchName = dataGridView2.CurrentRow.Cells["batch_name"].Value.ToString();

            //// Get bill info by batch name
            //var billData = ibr.getbills(batchName); // Make sure this doesn't return null

            //if (billData != null)
            //{
            //    if (billData.total_price != 0 && billData.paid_price != 0)
            //    {
            //        MessageBox.Show("Bill already generated. Go to Supplier Bills to add payment.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        return;
            //    }

            //    // Fill the form if bill is editable
            //    txtSupplierName.Text = billData.supplier_name;
            //    textBox2.Text = billData.batch_name;
            //    txtTotal.Text = billData.total_price.ToString("0.00");
            //    txtDate.Text = billData.date.ToShortDateString();
            //    textBox3.Text = billData.paid_price.ToString("0.00");

            //    txtSupplierName.ReadOnly = false;
            //    textBox2.ReadOnly = false;
            //    txtTotal.ReadOnly = false;
            //    textBox3.ReadOnly = false;

            //    PanelBill.Visible = true;
            //    UIHelper.RoundPanelCorners(PanelBill, 20);
            //    UIHelper.ShowCenteredPanel(this, PanelBill);
            //}
            //else
            //{
            //    MessageBox.Show("No bill found for selected batch.");
            //}
        }

       

       

        private void iconButton1_Click(object sender, EventArgs e)
        {
             // Use new details panel
            //if (dataGridView2.CurrentRow != null)
            //{
            //    int id = Convert.ToInt32(dataGridView2.CurrentRow.Cells["batch_id"].Value);
            //    ShowDetails(id);
            //}
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
