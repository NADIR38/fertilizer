using fertilizesop.BL.Bl;
using fertilizesop.BL.Models;
using fertilizesop.Interfaces.BLInterfaces;
using KIMS;
using Microsoft.Extensions.DependencyInjection;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace fertilizesop.UI
{
    public partial class UnifiedBatchPurchaseForm : Form
    {
        #region Fields
        private readonly IBatchesBl _batchBl;
        private readonly IbatchdetailsBl _batchDetailsBl;
        private readonly ISupplierBillBl _supplierBillBl;
        private int _selectedProductId;
        private string _selectedProductName;
        private string _selectedProductDescription;
        private decimal _totalBillAmount = 0;
        private const string TEMP_FOLDER_NAME = "TempBatchPurchases";
        private System.Windows.Forms.Timer _autoSaveTimer;
        private const int AUTO_SAVE_INTERVAL = 30000; // 30 seconds
        private bool _dataSavedSuccessfully = false;
        private bool _isEditMode = false; // Track if we're editing a product
        private int _editingRowIndex = -1; // Track which row is being edited
        #endregion

        #region Constructor
        public UnifiedBatchPurchaseForm(
     IBatchesBl batchBl,
     IbatchdetailsBl batchDetailsBl,
     ISupplierBillBl supplierBillBl)
        {
            InitializeComponent();
            _batchBl = batchBl;
            _batchDetailsBl = batchDetailsBl;
            _supplierBillBl = supplierBillBl;

            InitializeEventHandlers();
            SetupFormSettings();
            InitializeAutoSaveTimer();
        }
        #endregion

        #region Initialization
        private void InitializeEventHandlers()
        {
            this.Load += UnifiedBatchPurchaseForm_Load;
            this.FormClosing += UnifiedBatchPurchaseForm_FormClosing;
            this.KeyPreview = true;
            this.KeyDown += UnifiedBatchPurchaseForm_KeyDown;

            // Batch info events
            txtBatchName.TextChanged += TxtBatchName_TextChanged;
            cmbSupplier.TextUpdate += CmbSupplier_TextUpdate;

            // Product search events
            txtProductSearch.TextChanged += TxtProductSearch_TextChanged;
            dgvProductSearch.CellClick += DgvProductSearch_CellClick;

            // Batch details grid events
            dgvBatchDetails.CellDoubleClick += DgvBatchDetails_CellDoubleClick;
            dgvBatchDetails.CellClick += DgvBatchDetails_CellClick; // NEW: Single click to edit
            dgvBatchDetails.CellEndEdit += DgvBatchDetails_CellEndEdit;

            // Button events
            btnAddProduct.Click += BtnAddProduct_Click;
            btnSaveAll.Click += BtnSaveAll_Click;
            btnCancel.Click += BtnCancel_Click;
            btnClearAll.Click += BtnClearAll_Click;

            // Payment events
            txtPaidAmount.TextChanged += TxtPaidAmount_TextChanged;
        }

        private void InitializeAutoSaveTimer()
        {
            _autoSaveTimer = new System.Windows.Forms.Timer();
            _autoSaveTimer.Interval = AUTO_SAVE_INTERVAL;
            _autoSaveTimer.Tick += AutoSaveTimer_Tick;
            _autoSaveTimer.Start();
        }

        private void AutoSaveTimer_Tick(object sender, EventArgs e)
        {
            if (dgvBatchDetails.Rows.Count > 0 &&
                !string.IsNullOrWhiteSpace(txtBatchName.Text) &&
                !_dataSavedSuccessfully)
            {
                SaveTempData();
                System.Diagnostics.Debug.WriteLine($"[AUTO-SAVE] Data saved at {DateTime.Now:HH:mm:ss}");
            }
        }

        private void SetupFormSettings()
        {
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MaximizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
        }

        private void UnifiedBatchPurchaseForm_Load(object sender, EventArgs e)
        {
            SetupGridColumns();
            LoadSuppliers();
            LoadTempData();
            dtpReceivedDate.Value = DateTime.Now;
            txtProductSearch.Focus();

            // Style grids
            UIHelper.StyleGridView(dgvBatchDetails);
            UIHelper.StyleGridView(dgvProductSearch);

            dgvProductSearch.Location = new Point(txtProductSearch.Left, txtProductSearch.Bottom + 2);
            dgvProductSearch.Width = 500;
            dgvProductSearch.Height = 200;
            dgvProductSearch.Anchor = AnchorStyles.None;
            dgvProductSearch.Visible = false;
            dgvProductSearch.BringToFront();
            btnAddProduct.BackColor = Color.Green;
            UpdateBillSummary();

            // Set button text for add mode
            UpdateButtonText();
        }

        private void SetupGridColumns()
        {
            // Batch Details Grid
            dgvBatchDetails.Columns.Clear();
            dgvBatchDetails.Columns.Add("ProductId", "Product ID");
            dgvBatchDetails.Columns.Add("ProductName", "Product Name");
            dgvBatchDetails.Columns.Add("Description", "Description");
            dgvBatchDetails.Columns.Add("CostPrice", "Cost Price");
            dgvBatchDetails.Columns.Add("SalePrice", "Sale Price");
            dgvBatchDetails.Columns.Add("Quantity", "Quantity");
            dgvBatchDetails.Columns.Add("TotalCost", "Total Cost");

            dgvBatchDetails.Columns["ProductId"].ReadOnly = true;
            dgvBatchDetails.Columns["ProductName"].ReadOnly = true;
            dgvBatchDetails.Columns["Description"].ReadOnly = true;
            dgvBatchDetails.Columns["TotalCost"].ReadOnly = true;

            dgvBatchDetails.Columns["CostPrice"].ValueType = typeof(decimal);
            dgvBatchDetails.Columns["SalePrice"].ValueType = typeof(decimal);
            dgvBatchDetails.Columns["Quantity"].ValueType = typeof(int);
            dgvBatchDetails.Columns["TotalCost"].ValueType = typeof(decimal);

            dgvBatchDetails.AllowUserToAddRows = false;
            dgvBatchDetails.AllowUserToDeleteRows = false;
            dgvBatchDetails.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvBatchDetails.SelectionMode = DataGridViewSelectionMode.FullRowSelect; // NEW: Full row selection
            dgvBatchDetails.MultiSelect = false; // NEW: Single row selection only
        }

        private void LoadSuppliers()
        {
            try
            {
                var suppliers = _batchBl.getsuppliernames("");
                if (suppliers != null && suppliers.Count > 0)
                {
                    cmbSupplier.Items.Clear();
                    cmbSupplier.Items.AddRange(suppliers.ToArray());

                    var autoSource = new AutoCompleteStringCollection();
                    autoSource.AddRange(suppliers.ToArray());
                    cmbSupplier.AutoCompleteCustomSource = autoSource;
                    cmbSupplier.AutoCompleteMode = AutoCompleteMode.Suggest;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading suppliers: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Serialization
        private string GetTempFilePath()
        {
            string folder = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "Fertilizer",
                TEMP_FOLDER_NAME
            );

            try
            {
                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error creating temp folder: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            string fileName = string.IsNullOrWhiteSpace(txtBatchName.Text)
                ? "temp_purchase.json"
                : $"{txtBatchName.Text.Trim()}_purchase.json";

            return Path.Combine(folder, fileName);
        }

        private void SaveTempData()
        {
            try
            {
                var tempData = new TempBatchPurchaseData
                {
                    BatchName = txtBatchName.Text.Trim(),
                    SupplierName = cmbSupplier.Text.Trim(),
                    ReceivedDate = dtpReceivedDate.Value,
                    TotalAmount = string.IsNullOrWhiteSpace(txtTotalAmount.Text) ? 0 : decimal.Parse(txtTotalAmount.Text),
                    PaidAmount = string.IsNullOrWhiteSpace(txtPaidAmount.Text) ? 0 : decimal.Parse(txtPaidAmount.Text),
                    Remarks = txtRemarks.Text.Trim(),
                    BatchDetails = new List<TempBatchDetailItem>()
                };

                foreach (DataGridViewRow row in dgvBatchDetails.Rows)
                {
                    if (row.IsNewRow) continue;

                    tempData.BatchDetails.Add(new TempBatchDetailItem
                    {
                        ProductId = Convert.ToInt32(row.Cells["ProductId"].Value),
                        ProductName = row.Cells["ProductName"].Value.ToString(),
                        Description = row.Cells["Description"].Value.ToString(),
                        CostPrice = Convert.ToDecimal(row.Cells["CostPrice"].Value),
                        SalePrice = Convert.ToDecimal(row.Cells["SalePrice"].Value),
                        Quantity = Convert.ToInt32(row.Cells["Quantity"].Value)
                    });
                }

                string json = JsonConvert.SerializeObject(tempData, Formatting.Indented);
                File.WriteAllText(GetTempFilePath(), json);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error saving temp data: {ex.Message}");
            }
        }

        private void LoadTempData()
        {
            try
            {
                string path = GetTempFilePath();
                if (!File.Exists(path)) return;

                FileInfo fileInfo = new FileInfo(path);
                DateTime lastModified = fileInfo.LastWriteTime;
                TimeSpan timeSinceModified = DateTime.Now - lastModified;

                string timeAgo = timeSinceModified.TotalMinutes < 60
                    ? $"{(int)timeSinceModified.TotalMinutes} minutes ago"
                    : timeSinceModified.TotalHours < 24
                        ? $"{(int)timeSinceModified.TotalHours} hours ago"
                        : $"{(int)timeSinceModified.TotalDays} days ago";

                var result = MessageBox.Show(
                    $"🔄 Recovered Data Available!\n\n" +
                    $"📋 Batch: {Path.GetFileNameWithoutExtension(path).Replace("_purchase", "")}\n" +
                    $"⏰ Last saved: {timeAgo}\n" +
                    $"📅 Date: {lastModified:yyyy-MM-dd HH:mm:ss}\n\n" +
                    $"Would you like to restore this data?",
                    "Recover Previous Session",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (result == DialogResult.No)
                {
                    DeleteTempData();
                    return;
                }

                string json = File.ReadAllText(path);
                var tempData = JsonConvert.DeserializeObject<TempBatchPurchaseData>(json);

                if (tempData != null)
                {
                    txtBatchName.Text = tempData.BatchName;
                    cmbSupplier.Text = tempData.SupplierName;
                    dtpReceivedDate.Value = tempData.ReceivedDate;
                    txtTotalAmount.Text = tempData.TotalAmount > 0 ? tempData.TotalAmount.ToString("0.00") : "";
                    txtPaidAmount.Text = tempData.PaidAmount.ToString("0.00");
                    txtRemarks.Text = tempData.Remarks;

                    dgvBatchDetails.Rows.Clear();
                    foreach (var item in tempData.BatchDetails)
                    {
                        decimal totalCost = item.CostPrice * item.Quantity;
                        dgvBatchDetails.Rows.Add(
                            item.ProductId,
                            item.ProductName,
                            item.Description,
                            item.CostPrice,
                            item.SalePrice,
                            item.Quantity,
                            totalCost
                        );
                    }

                    UpdateBillSummary();

                    MessageBox.Show(
                        $"✅ Data recovered successfully!\n\n" +
                        $"📦 {tempData.BatchDetails.Count} products loaded",
                        "Recovery Complete",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"⚠️ Error loading saved data: {ex.Message}\n\n" +
                    "The temp file may be corrupted.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);

                try
                {
                    DeleteTempData();
                }
                catch { }
            }
        }

        private void DeleteTempData()
        {
            try
            {
                string path = GetTempFilePath();
                if (File.Exists(path))
                {
                    File.Delete(path);
                    System.Diagnostics.Debug.WriteLine($"[TEMP-DELETE] Deleted temp file: {path}");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error deleting temp file: {ex.Message}");
            }
        }
        #endregion

        #region Product Search
        private void TxtProductSearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string searchText = txtProductSearch.Text.Trim();

                if (!string.IsNullOrEmpty(searchText))
                {
                    dgvProductSearch.Cursor = Cursors.WaitCursor;

                    var products = DatabaseHelper.Instance.GetProductsByNames(searchText);

                    if (products == null || products.Count == 0)
                    {
                        dgvProductSearch.DataSource = null;
                        dgvProductSearch.Visible = false;
                        return;
                    }

                    dgvProductSearch.DataSource = products;

                    if (dgvProductSearch.Columns.Contains("Id"))
                        dgvProductSearch.Columns["Id"].Visible = false;
                    if (dgvProductSearch.Columns.Contains("Price"))
                        dgvProductSearch.Columns["Price"].Visible = false;
                    if (dgvProductSearch.Columns.Contains("quantity"))
                        dgvProductSearch.Columns["quantity"].Visible = false;

                    dgvProductSearch.Location = new Point(txtProductSearch.Left, txtProductSearch.Bottom + 2);
                    dgvProductSearch.Width = Math.Max(txtProductSearch.Width + 200, 400);
                    dgvProductSearch.Height = Math.Min(products.Count * 35 + 40, 200);
                    dgvProductSearch.Visible = true;
                    dgvProductSearch.BringToFront();
                    dgvProductSearch.Cursor = Cursors.Default;
                }
                else
                {
                    dgvProductSearch.Visible = false;
                    dgvProductSearch.SendToBack();
                }
            }
            catch (Exception ex)
            {
                dgvProductSearch.Visible = false;
                dgvProductSearch.Cursor = Cursors.Default;

                System.Diagnostics.Debug.WriteLine($"Product search error: {ex.Message}");
            }
        }

        private void DgvProductSearch_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var row = dgvProductSearch.Rows[e.RowIndex];
                _selectedProductId = Convert.ToInt32(row.Cells["Id"].Value);
                _selectedProductName = row.Cells["Name"].Value.ToString();
                _selectedProductDescription = row.Cells["Description"].Value?.ToString() ?? "";

                txtProductSearch.Text = _selectedProductName;
                txtSalePrice.Text = _batchDetailsBl.getsaleprice(_selectedProductId).ToString();

                dgvProductSearch.Visible = false;
                txtCostPrice.Focus();
            }
        }
        #endregion

        #region Add/Update Product
        private void BtnAddProduct_Click(object sender, EventArgs e)
        {
            if (_isEditMode)
            {
                UpdateProductInGrid();
            }
            else
            {
                AddProductToGrid();
            }
        }

        private void AddProductToGrid()
        {
            // Validation
            if (_selectedProductId == 0)
            {
                MessageBox.Show("Please select a product.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtProductSearch.Focus();
                return;
            }

            if (!decimal.TryParse(txtCostPrice.Text, out decimal costPrice) || costPrice <= 0)
            {
                MessageBox.Show("Please enter a valid cost price.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCostPrice.Focus();
                return;
            }

            if (!decimal.TryParse(txtSalePrice.Text, out decimal salePrice) || salePrice <= 0)
            {
                MessageBox.Show("Please enter a valid sale price.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSalePrice.Focus();
                return;
            }

            if (!int.TryParse(txtQuantity.Text, out int quantity) || quantity <= 0)
            {
                MessageBox.Show("Please enter a valid quantity.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtQuantity.Focus();
                return;
            }

            // Check if product already exists in grid
            foreach (DataGridViewRow row in dgvBatchDetails.Rows)
            {
                if (Convert.ToInt32(row.Cells["ProductId"].Value) == _selectedProductId)
                {
                    MessageBox.Show("Product already added. Please click on the row to edit it instead.",
                        "Duplicate Product", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }

            // Calculate total cost
            decimal totalCost = costPrice * quantity;

            // Add to grid
            dgvBatchDetails.Rows.Add(
                _selectedProductId,
                _selectedProductName,
                _selectedProductDescription,
                costPrice,
                salePrice,
                quantity,
                totalCost
            );

            // Clear input fields
            ClearProductInputs();

            if (!_dataSavedSuccessfully)
            {
                SaveTempData();
            }

            UpdateBillSummary();
            txtProductSearch.Focus();
        }

        private void UpdateProductInGrid()
        {
            // Validation
            if (_editingRowIndex < 0 || _editingRowIndex >= dgvBatchDetails.Rows.Count)
            {
                MessageBox.Show("Invalid row selected.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                CancelEdit();
                return;
            }

            if (!decimal.TryParse(txtCostPrice.Text, out decimal costPrice) || costPrice <= 0)
            {
                MessageBox.Show("Please enter a valid cost price.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCostPrice.Focus();
                return;
            }

            if (!decimal.TryParse(txtSalePrice.Text, out decimal salePrice) || salePrice <= 0)
            {
                MessageBox.Show("Please enter a valid sale price.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSalePrice.Focus();
                return;
            }

            if (!int.TryParse(txtQuantity.Text, out int quantity) || quantity <= 0)
            {
                MessageBox.Show("Please enter a valid quantity.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtQuantity.Focus();
                return;
            }

            // Update the row
            var row = dgvBatchDetails.Rows[_editingRowIndex];
            row.Cells["CostPrice"].Value = costPrice;
            row.Cells["SalePrice"].Value = salePrice;
            row.Cells["Quantity"].Value = quantity;
            row.Cells["TotalCost"].Value = costPrice * quantity;

            // Clear selection highlight
            row.DefaultCellStyle.BackColor = Color.White;
            row.DefaultCellStyle.SelectionBackColor = SystemColors.Highlight;

            MessageBox.Show("✅ Product updated successfully!", "Success",
                MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Clear input fields and exit edit mode
            CancelEdit();

            if (!_dataSavedSuccessfully)
            {
                SaveTempData();
            }

            UpdateBillSummary();
            txtProductSearch.Focus();
        }

        private void ClearProductInputs()
        {
            txtProductSearch.Clear();
            txtCostPrice.Clear();
            txtSalePrice.Clear();
            txtQuantity.Clear();
            _selectedProductId = 0;
            _selectedProductName = null;
            _selectedProductDescription = null;
        }

        private void CancelEdit()
        {
            // Clear highlight from previously selected row
            if (_editingRowIndex >= 0 && _editingRowIndex < dgvBatchDetails.Rows.Count)
            {
                var row = dgvBatchDetails.Rows[_editingRowIndex];
                row.DefaultCellStyle.BackColor = Color.White;
                row.DefaultCellStyle.SelectionBackColor = SystemColors.Highlight;
            }

            _isEditMode = false;
            _editingRowIndex = -1;
            ClearProductInputs();
            UpdateButtonText();

            // Clear grid selection
            dgvBatchDetails.ClearSelection();
        }

        private void UpdateButtonText()
        {
            if (_isEditMode)
            {
                btnAddProduct.Text = "✓ Update Product";
                btnAddProduct.BackColor = Color.Orange;

                // Show cancel button or use existing clear button
                // You might want to add a cancel button here
            }
            else
            {
                btnAddProduct.Text = "+ Add Product";
                btnAddProduct.BackColor = Color.Green;
            }
        }
        #endregion

        #region Grid Events
        private void DgvBatchDetails_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && !dgvBatchDetails.Rows[e.RowIndex].IsNewRow)
            {
                var row = dgvBatchDetails.Rows[e.RowIndex];

                // Enter edit mode
                _isEditMode = true;
                _editingRowIndex = e.RowIndex;

                // Load product data into input fields
                _selectedProductId = Convert.ToInt32(row.Cells["ProductId"].Value);
                _selectedProductName = row.Cells["ProductName"].Value.ToString();
                _selectedProductDescription = row.Cells["Description"].Value.ToString();

                txtProductSearch.Text = _selectedProductName;
                txtCostPrice.Text = row.Cells["CostPrice"].Value.ToString();
                txtSalePrice.Text = row.Cells["SalePrice"].Value.ToString();
                txtQuantity.Text = row.Cells["Quantity"].Value.ToString();

                // Highlight the row being edited
                row.DefaultCellStyle.BackColor = Color.LightYellow;
                row.DefaultCellStyle.SelectionBackColor = Color.Gold;

                // Update button text
                UpdateButtonText();

                // Focus on cost price for quick editing
                txtCostPrice.Focus();
                txtCostPrice.SelectAll();
            }
        }

        private void DgvBatchDetails_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && !dgvBatchDetails.Rows[e.RowIndex].IsNewRow)
            {
                var result = MessageBox.Show(
                    "Do you want to delete this product?",
                    "Confirm Delete",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    // If deleting the row being edited, cancel edit mode
                    if (_editingRowIndex == e.RowIndex)
                    {
                        CancelEdit();
                    }

                    dgvBatchDetails.Rows.RemoveAt(e.RowIndex);

                    if (!_dataSavedSuccessfully)
                    {
                        SaveTempData();
                    }

                    UpdateBillSummary();

                    MessageBox.Show("✅ Product deleted successfully!", "Deleted",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void DgvBatchDetails_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var row = dgvBatchDetails.Rows[e.RowIndex];
            string columnName = dgvBatchDetails.Columns[e.ColumnIndex].Name;

            // Recalculate total cost if cost price or quantity changed
            if (columnName == "CostPrice" || columnName == "Quantity")
            {
                try
                {
                    decimal costPrice = Convert.ToDecimal(row.Cells["CostPrice"].Value);
                    int quantity = Convert.ToInt32(row.Cells["Quantity"].Value);
                    row.Cells["TotalCost"].Value = costPrice * quantity;

                    UpdateBillSummary();

                    if (!_dataSavedSuccessfully)
                    {
                        SaveTempData();
                    }
                }
                catch
                {
                    MessageBox.Show("Invalid input. Please enter valid numbers.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        #endregion

        #region Bill Summary
        private void UpdateBillSummary()
        {
            decimal totalAmount = 0;
            if (!string.IsNullOrWhiteSpace(txtTotalAmount.Text))
            {
                decimal.TryParse(txtTotalAmount.Text, out totalAmount);
            }
        }

        private void TxtPaidAmount_TextChanged(object sender, EventArgs e)
        {
        }
        #endregion

        #region Save All
        private void BtnSaveAll_Click(object sender, EventArgs e)
        {
            SaveAllData();
        }

        private void SaveAllData()
        {
            if (string.IsNullOrWhiteSpace(txtBatchName.Text))
            {
                MessageBox.Show("Please enter batch name.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtBatchName.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(cmbSupplier.Text))
            {
                MessageBox.Show("Please select a supplier.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbSupplier.Focus();
                return;
            }

            if (dgvBatchDetails.Rows.Count == 0)
            {
                MessageBox.Show("Please add at least one product to the batch.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Check if user is in edit mode
            if (_isEditMode)
            {
                var result = MessageBox.Show(
                    "⚠️ You are currently editing a product.\n\n" +
                    "Do you want to update this product before saving?",
                    "Product Being Edited",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    UpdateProductInGrid();
                    // Continue with save after update
                }
                else if (result == DialogResult.Cancel)
                {
                    return; // Don't save
                }
                else // No
                {
                    CancelEdit(); // Discard changes and continue with save
                }
            }

            decimal calculatedTotal = 0;
            foreach (DataGridViewRow row in dgvBatchDetails.Rows)
            {
                if (!row.IsNewRow)
                {
                    calculatedTotal += Convert.ToDecimal(row.Cells["TotalCost"].Value);
                }
            }

            decimal manualTotal = 0;
            if (!string.IsNullOrWhiteSpace(txtTotalAmount.Text))
            {
                decimal.TryParse(txtTotalAmount.Text, out manualTotal);
            }

            decimal totalAmount = manualTotal > 0 ? manualTotal : calculatedTotal;

            decimal paidAmount = 0;
            if (!string.IsNullOrWhiteSpace(txtPaidAmount.Text))
            {
                decimal.TryParse(txtPaidAmount.Text, out paidAmount);
            }

            if (paidAmount > totalAmount)
            {
                MessageBox.Show(
                    $"Paid amount (Rs. {paidAmount:N2}) cannot be greater than total amount (Rs. {totalAmount:N2}).",
                    "Validation Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                txtPaidAmount.Focus();
                return;
            }

            var confirmResult = MessageBox.Show(
                $"Are you sure you want to save this batch purchase?\n\n" +
                $"Batch: {txtBatchName.Text}\n" +
                $"Supplier: {cmbSupplier.Text}\n" +
                $"Total Amount: Rs. {totalAmount:N2}\n" +
                $"Paid Amount: Rs. {paidAmount:N2}\n" +
                $"Pending: Rs. {(totalAmount - paidAmount):N2}\n" +
                $"Products: {dgvBatchDetails.Rows.Count}",
                "Confirm Save",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (confirmResult != DialogResult.Yes)
                return;

            try
            {
                this.Enabled = false;
                Cursor = Cursors.WaitCursor;

                var batch = new Batches(
                    0,
                    txtBatchName.Text.Trim(),
                    dtpReceivedDate.Value,
                    cmbSupplier.Text.Trim(),
                    "",
                    0
                );

                bool batchCreated = _batchBl.addbatches(batch);
                if (!batchCreated)
                {
                    throw new Exception("Failed to create batch.");
                }

                int batchId = DatabaseHelper.Instance.getbatchid(txtBatchName.Text.Trim());
                int supplierId = DatabaseHelper.Instance.getsuppierid(cmbSupplier.Text.Trim());

                bool supplierBillCreated = CreateSupplierBill(batchId, supplierId, totalAmount, paidAmount);
                if (!supplierBillCreated)
                {
                    throw new Exception("Failed to create supplier bill.");
                }

                int successCount = 0;
                int failCount = 0;

                foreach (DataGridViewRow row in dgvBatchDetails.Rows)
                {
                    if (row.IsNewRow) continue;

                    try
                    {
                        var detail = new BatchDetails(
                            0,
                            txtBatchName.Text.Trim(),
                            Convert.ToDecimal(row.Cells["CostPrice"].Value),
                            Convert.ToDecimal(row.Cells["SalePrice"].Value),
                            Convert.ToInt32(row.Cells["ProductId"].Value),
                            row.Cells["ProductName"].Value.ToString(),
                            Convert.ToInt32(row.Cells["Quantity"].Value)
                        );

                        bool detailAdded = _batchDetailsBl.adddetails(detail);
                        if (detailAdded)
                            successCount++;
                        else
                            failCount++;
                    }
                    catch (Exception ex)
                    {
                        failCount++;
                        System.Diagnostics.Debug.WriteLine($"Error adding detail: {ex.Message}");
                    }
                }

                if (paidAmount > 0)
                {
                    int billId = GetSupplierBillId(batchId, supplierId);
                    if (billId > 0)
                    {
                        AddPaymentRecord(supplierId, billId, paidAmount, txtRemarks.Text.Trim());
                    }
                }

                _dataSavedSuccessfully = true;

                this.Enabled = true;
                Cursor = Cursors.Default;

                MessageBox.Show(
                    $"✅ Batch purchase saved successfully!\n\n" +
                    $"📦 Batch Details Added: {successCount}\n" +
                    $"❌ Failed: {failCount}\n" +
                    $"💰 Total Amount: Rs. {totalAmount:N2}\n" +
                    $"💵 Paid: Rs. {paidAmount:N2}\n" +
                    $"📊 Pending: Rs. {(totalAmount - paidAmount):N2}",
                    "Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                DeleteTempData();
                ClearForm();
                _dataSavedSuccessfully = false;
            }
            catch (Exception ex)
            {
                this.Enabled = true;
                Cursor = Cursors.Default;

                MessageBox.Show(
                    $"❌ Error saving batch purchase:\n\n{ex.Message}\n\n{ex.InnerException?.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private bool CreateSupplierBill(int batchId, int supplierId, decimal totalAmount, decimal paidAmount)
        {
            try
            {
                using (var conn = DatabaseHelper.Instance.GetConnection())
                {
                    conn.Open();

                    string paymentStatus = paidAmount >= totalAmount ? "Paid" : "Due";

                    string query = @"
                INSERT INTO supplierbills 
                (batch_id, supplier_id, total_price, paid_amount, date, payment_status)
                VALUES (@batch_id, @supplier_id, @total_price, @paid_amount, @date, @payment_status)";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@batch_id", batchId);
                        cmd.Parameters.AddWithValue("@supplier_id", supplierId);
                        cmd.Parameters.AddWithValue("@total_price", totalAmount);
                        cmd.Parameters.AddWithValue("@paid_amount", paidAmount);
                        cmd.Parameters.AddWithValue("@date", DateTime.Now);
                        cmd.Parameters.AddWithValue("@payment_status", paymentStatus);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error creating supplier bill: {ex.Message}");
                throw new Exception($"Failed to create supplier bill: {ex.Message}", ex);
            }
        }

        private int GetSupplierBillId(int batchId, int supplierId)
        {
            try
            {
                using (var conn = DatabaseHelper.Instance.GetConnection())
                {
                    conn.Open();
                    string query = @"
                SELECT supplier_bill_id 
                FROM supplierbills 
                WHERE batch_id = @batch_id AND supplier_id = @supplier_id 
                LIMIT 1";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@batch_id", batchId);
                        cmd.Parameters.AddWithValue("@supplier_id", supplierId);

                        object result = cmd.ExecuteScalar();
                        return result != null ? Convert.ToInt32(result) : 0;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting supplier bill ID: {ex.Message}");
                return 0;
            }
        }

        private bool AddPaymentRecord(int supplierId, int billId, decimal paymentAmount, string remarks)
        {
            try
            {
                using (var conn = DatabaseHelper.Instance.GetConnection())
                {
                    conn.Open();
                    string query = @"
                INSERT INTO supplierpricerecord 
                (supplier_id, supplier_bill_id, date, payment, remarks)
                VALUES (@supplier_id, @bill_id, @date, @payment, @remarks)";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@supplier_id", supplierId);
                        cmd.Parameters.AddWithValue("@bill_id", billId);
                        cmd.Parameters.AddWithValue("@date", DateTime.Now);
                        cmd.Parameters.AddWithValue("@payment", paymentAmount);
                        cmd.Parameters.AddWithValue("@remarks", string.IsNullOrWhiteSpace(remarks) ? "Initial Payment" : remarks);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error adding payment record: {ex.Message}");
                return false;
            }
        }
        #endregion

        #region Form Management
        private void ClearForm()
        {
            txtBatchName.Clear();
            cmbSupplier.SelectedIndex = -1;
            dtpReceivedDate.Value = DateTime.Now;
            txtTotalAmount.Clear();
            txtPaidAmount.Clear();
            txtRemarks.Clear();
            dgvBatchDetails.Rows.Clear();
            CancelEdit(); // Clear edit mode
            UpdateBillSummary();
            txtBatchName.Focus();
        }

        private void BtnClearAll_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show(
                "Are you sure you want to clear all data? This will delete unsaved changes.",
                "Confirm Clear",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (result == DialogResult.Yes)
            {
                DeleteTempData();
                ClearForm();
                _dataSavedSuccessfully = false;
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void UnifiedBatchPurchaseForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _autoSaveTimer?.Stop();
            _autoSaveTimer?.Dispose();

            if (_dataSavedSuccessfully)
            {
                return;
            }

            if (dgvBatchDetails.Rows.Count > 0 && !string.IsNullOrWhiteSpace(txtBatchName.Text))
            {
                var result = MessageBox.Show(
                    "💾 You have unsaved changes.\n\n" +
                    "Yes - Save for later (will auto-recover next time)\n" +
                    "No - Discard changes permanently\n" +
                    "Cancel - Return to form",
                    "Unsaved Changes",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Question
                );

                if (result == DialogResult.Yes)
                {
                    SaveTempData();
                    MessageBox.Show("✅ Your work has been saved for recovery!", "Saved",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (result == DialogResult.Cancel)
                {
                    e.Cancel = true;
                    _autoSaveTimer?.Start();
                    return;
                }
                else
                {
                    DeleteTempData();
                }
            }
            else if (dgvBatchDetails.Rows.Count > 0)
            {
                var result = MessageBox.Show(
                    "💾 You have unsaved product entries.\n\n" +
                    "Save for later?",
                    "Unsaved Changes",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Question
                );

                if (result == DialogResult.Yes)
                {
                    SaveTempData();
                }
                else if (result == DialogResult.Cancel)
                {
                    e.Cancel = true;
                    _autoSaveTimer?.Start();
                    return;
                }
                else
                {
                    DeleteTempData();
                }
            }
        }
        #endregion

        #region Keyboard Shortcuts
        private void UnifiedBatchPurchaseForm_KeyDown(object sender, KeyEventArgs e)
        {
            // Ctrl + S = Save
            if (e.Control && e.KeyCode == Keys.S)
            {
                btnSaveAll.PerformClick();
                e.Handled = true;
            }
            // Ctrl + N = Clear/New
            else if (e.Control && e.KeyCode == Keys.N)
            {
                btnClearAll.PerformClick();
                e.Handled = true;
            }
            // Escape = Cancel edit OR close form
            else if (e.KeyCode == Keys.Escape)
            {
                if (_isEditMode)
                {
                    CancelEdit();
                    e.Handled = true;
                }
                else
                {
                    this.Close();
                    e.Handled = true;
                }
            }
            // Enter on product search
            else if (e.KeyCode == Keys.Enter && txtProductSearch.Focused)
            {
                if (!dgvProductSearch.Visible && _selectedProductId > 0)
                {
                    txtCostPrice.Focus();
                }
                e.Handled = true;
            }
            // Enter on quantity = Add/Update product
            else if (e.KeyCode == Keys.Enter && txtQuantity.Focused)
            {
                if (_isEditMode)
                {
                    UpdateProductInGrid();
                }
                else
                {
                    AddProductToGrid();
                }
                e.Handled = true;
            }
        }
        #endregion

        #region Supplier Autocomplete
        private void TxtBatchName_TextChanged(object sender, EventArgs e)
        {
            if (dgvBatchDetails.Rows.Count > 0 && !_dataSavedSuccessfully)
            {
                SaveTempData();
            }
        }

        private void CmbSupplier_TextUpdate(object sender, EventArgs e)
        {
            string searchText = cmbSupplier.Text.Trim();
            var filteredSuppliers = _batchBl.getsuppliernames(searchText);

            if (filteredSuppliers != null && filteredSuppliers.Count > 0)
            {
                cmbSupplier.Items.Clear();
                cmbSupplier.Items.AddRange(filteredSuppliers.ToArray());
                cmbSupplier.SelectionStart = searchText.Length;
                cmbSupplier.SelectionLength = 0;
                cmbSupplier.DroppedDown = true;
            }
        }
        #endregion

        private void txtTotalAmount_TextChanged(object sender, EventArgs e)
        {
            UpdateBillSummary();
        }
    }

    #region Temp Data Models
    public class TempBatchPurchaseData
    {
        public string BatchName { get; set; }
        public string SupplierName { get; set; }
        public DateTime ReceivedDate { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public string Remarks { get; set; }
        public List<TempBatchDetailItem> BatchDetails { get; set; }
    }

    public class TempBatchDetailItem
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public decimal CostPrice { get; set; }
        public decimal SalePrice { get; set; }
        public int Quantity { get; set; }
    }
    #endregion
}