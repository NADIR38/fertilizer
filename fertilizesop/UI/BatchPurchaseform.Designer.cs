namespace fertilizesop.UI
{
    partial class UnifiedBatchPurchaseForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.panelBatchInfo = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.txtBatchName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbSupplier = new System.Windows.Forms.ComboBox();
            this.btnAddSupplier = new FontAwesome.Sharp.IconPictureBox();
            this.label3 = new System.Windows.Forms.Label();
            this.dtpReceivedDate = new System.Windows.Forms.DateTimePicker();
            this.panelProductEntry = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.txtProductSearch = new System.Windows.Forms.TextBox();
            this.dgvProductSearch = new System.Windows.Forms.DataGridView();
            this.label5 = new System.Windows.Forms.Label();
            this.txtCostPrice = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtSalePrice = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtQuantity = new System.Windows.Forms.TextBox();
            this.btnAddProduct = new FontAwesome.Sharp.IconButton();
            this.panelBatchDetails = new System.Windows.Forms.Panel();
            this.label8 = new System.Windows.Forms.Label();
            this.dgvBatchDetails = new System.Windows.Forms.DataGridView();
            this.panelPayment = new System.Windows.Forms.Panel();
            this.Total = new System.Windows.Forms.Label();
            this.txtTotalAmount = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtPaidAmount = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.txtRemarks = new System.Windows.Forms.TextBox();
            this.btnSaveAll = new FontAwesome.Sharp.IconButton();
            this.btnCancel = new FontAwesome.Sharp.IconButton();
            this.btnClearAll = new FontAwesome.Sharp.IconButton();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panelBatchInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnAddSupplier)).BeginInit();
            this.panelProductEntry.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProductSearch)).BeginInit();
            this.panelBatchDetails.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBatchDetails)).BeginInit();
            this.panelPayment.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelBatchInfo
            // 
            this.panelBatchInfo.BackColor = System.Drawing.Color.White;
            this.panelBatchInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelBatchInfo.Controls.Add(this.label1);
            this.panelBatchInfo.Controls.Add(this.txtBatchName);
            this.panelBatchInfo.Controls.Add(this.label2);
            this.panelBatchInfo.Controls.Add(this.cmbSupplier);
            this.panelBatchInfo.Controls.Add(this.btnAddSupplier);
            this.panelBatchInfo.Controls.Add(this.label3);
            this.panelBatchInfo.Controls.Add(this.dtpReceivedDate);
            this.panelBatchInfo.Location = new System.Drawing.Point(16, 62);
            this.panelBatchInfo.Margin = new System.Windows.Forms.Padding(4);
            this.panelBatchInfo.Name = "panelBatchInfo";
            this.panelBatchInfo.Size = new System.Drawing.Size(1541, 98);
            this.panelBatchInfo.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(11)))), ((int)(((byte)(64)))), ((int)(((byte)(31)))));
            this.label1.Location = new System.Drawing.Point(13, 12);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(116, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "Batch Name *";
            // 
            // txtBatchName
            // 
            this.txtBatchName.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtBatchName.Location = new System.Drawing.Point(19, 43);
            this.txtBatchName.Margin = new System.Windows.Forms.Padding(4);
            this.txtBatchName.Name = "txtBatchName";
            this.txtBatchName.Size = new System.Drawing.Size(332, 30);
            this.txtBatchName.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(11)))), ((int)(((byte)(64)))), ((int)(((byte)(31)))));
            this.label2.Location = new System.Drawing.Point(373, 12);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 23);
            this.label2.TabIndex = 2;
            this.label2.Text = "Supplier *";
            // 
            // cmbSupplier
            // 
            this.cmbSupplier.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbSupplier.FormattingEnabled = true;
            this.cmbSupplier.Location = new System.Drawing.Point(379, 43);
            this.cmbSupplier.Margin = new System.Windows.Forms.Padding(4);
            this.cmbSupplier.Name = "cmbSupplier";
            this.cmbSupplier.Size = new System.Drawing.Size(399, 31);
            this.cmbSupplier.TabIndex = 3;
            // 
            // btnAddSupplier
            // 
            this.btnAddSupplier.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(11)))), ((int)(((byte)(64)))), ((int)(((byte)(31)))));
            this.btnAddSupplier.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAddSupplier.IconChar = FontAwesome.Sharp.IconChar.PlusSquare;
            this.btnAddSupplier.IconColor = System.Drawing.Color.White;
            this.btnAddSupplier.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnAddSupplier.IconSize = 37;
            this.btnAddSupplier.Location = new System.Drawing.Point(793, 41);
            this.btnAddSupplier.Margin = new System.Windows.Forms.Padding(4);
            this.btnAddSupplier.Name = "btnAddSupplier";
            this.btnAddSupplier.Size = new System.Drawing.Size(47, 37);
            this.btnAddSupplier.TabIndex = 4;
            this.btnAddSupplier.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(11)))), ((int)(((byte)(64)))), ((int)(((byte)(31)))));
            this.label3.Location = new System.Drawing.Point(867, 12);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(132, 23);
            this.label3.TabIndex = 5;
            this.label3.Text = "Received Date *";
            // 
            // dtpReceivedDate
            // 
            this.dtpReceivedDate.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.dtpReceivedDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpReceivedDate.Location = new System.Drawing.Point(872, 43);
            this.dtpReceivedDate.Margin = new System.Windows.Forms.Padding(4);
            this.dtpReceivedDate.Name = "dtpReceivedDate";
            this.dtpReceivedDate.Size = new System.Drawing.Size(265, 30);
            this.dtpReceivedDate.TabIndex = 6;
            // 
            // panelProductEntry
            // 
            this.panelProductEntry.BackColor = System.Drawing.Color.White;
            this.panelProductEntry.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelProductEntry.Controls.Add(this.label4);
            this.panelProductEntry.Controls.Add(this.txtProductSearch);
            this.panelProductEntry.Controls.Add(this.dgvProductSearch);
            this.panelProductEntry.Controls.Add(this.label5);
            this.panelProductEntry.Controls.Add(this.txtCostPrice);
            this.panelProductEntry.Controls.Add(this.label6);
            this.panelProductEntry.Controls.Add(this.txtSalePrice);
            this.panelProductEntry.Controls.Add(this.label7);
            this.panelProductEntry.Controls.Add(this.txtQuantity);
            this.panelProductEntry.Controls.Add(this.btnAddProduct);
            this.panelProductEntry.Location = new System.Drawing.Point(16, 172);
            this.panelProductEntry.Margin = new System.Windows.Forms.Padding(4);
            this.panelProductEntry.Name = "panelProductEntry";
            this.panelProductEntry.Size = new System.Drawing.Size(1541, 147);
            this.panelProductEntry.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(11)))), ((int)(((byte)(64)))), ((int)(((byte)(31)))));
            this.label4.Location = new System.Drawing.Point(13, 12);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(138, 23);
            this.label4.TabIndex = 0;
            this.label4.Text = "Search Product *";
            // 
            // txtProductSearch
            // 
            this.txtProductSearch.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtProductSearch.Location = new System.Drawing.Point(19, 43);
            this.txtProductSearch.Margin = new System.Windows.Forms.Padding(4);
            this.txtProductSearch.Name = "txtProductSearch";
            this.txtProductSearch.Size = new System.Drawing.Size(465, 30);
            this.txtProductSearch.TabIndex = 1;
            // 
            // dgvProductSearch
            // 
            this.dgvProductSearch.AllowUserToAddRows = false;
            this.dgvProductSearch.AllowUserToDeleteRows = false;
            this.dgvProductSearch.BackgroundColor = System.Drawing.Color.White;
            this.dgvProductSearch.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvProductSearch.Location = new System.Drawing.Point(19, 80);
            this.dgvProductSearch.Margin = new System.Windows.Forms.Padding(4);
            this.dgvProductSearch.Name = "dgvProductSearch";
            this.dgvProductSearch.ReadOnly = true;
            this.dgvProductSearch.RowHeadersWidth = 51;
            this.dgvProductSearch.Size = new System.Drawing.Size(467, 0);
            this.dgvProductSearch.TabIndex = 2;
            this.dgvProductSearch.Visible = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(11)))), ((int)(((byte)(64)))), ((int)(((byte)(31)))));
            this.label5.Location = new System.Drawing.Point(507, 12);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(98, 23);
            this.label5.TabIndex = 3;
            this.label5.Text = "Cost Price *";
            // 
            // txtCostPrice
            // 
            this.txtCostPrice.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtCostPrice.Location = new System.Drawing.Point(512, 43);
            this.txtCostPrice.Margin = new System.Windows.Forms.Padding(4);
            this.txtCostPrice.Name = "txtCostPrice";
            this.txtCostPrice.Size = new System.Drawing.Size(199, 30);
            this.txtCostPrice.TabIndex = 4;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.label6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(11)))), ((int)(((byte)(64)))), ((int)(((byte)(31)))));
            this.label6.Location = new System.Drawing.Point(733, 12);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(95, 23);
            this.label6.TabIndex = 5;
            this.label6.Text = "Sale Price *";
            // 
            // txtSalePrice
            // 
            this.txtSalePrice.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtSalePrice.Location = new System.Drawing.Point(739, 43);
            this.txtSalePrice.Margin = new System.Windows.Forms.Padding(4);
            this.txtSalePrice.Name = "txtSalePrice";
            this.txtSalePrice.Size = new System.Drawing.Size(199, 30);
            this.txtSalePrice.TabIndex = 6;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.label7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(11)))), ((int)(((byte)(64)))), ((int)(((byte)(31)))));
            this.label7.Location = new System.Drawing.Point(960, 12);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(89, 23);
            this.label7.TabIndex = 7;
            this.label7.Text = "Quantity *";
            // 
            // txtQuantity
            // 
            this.txtQuantity.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtQuantity.Location = new System.Drawing.Point(965, 43);
            this.txtQuantity.Margin = new System.Windows.Forms.Padding(4);
            this.txtQuantity.Name = "txtQuantity";
            this.txtQuantity.Size = new System.Drawing.Size(199, 30);
            this.txtQuantity.TabIndex = 8;
            // 
            // btnAddProduct
            // 
            this.btnAddProduct.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(11)))), ((int)(((byte)(64)))), ((int)(((byte)(31)))));
            this.btnAddProduct.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddProduct.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.btnAddProduct.ForeColor = System.Drawing.Color.White;
            this.btnAddProduct.IconChar = FontAwesome.Sharp.IconChar.Plus;
            this.btnAddProduct.IconColor = System.Drawing.Color.LimeGreen;
            this.btnAddProduct.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnAddProduct.IconSize = 24;
            this.btnAddProduct.Location = new System.Drawing.Point(1192, 34);
            this.btnAddProduct.Margin = new System.Windows.Forms.Padding(4);
            this.btnAddProduct.Name = "btnAddProduct";
            this.btnAddProduct.Size = new System.Drawing.Size(240, 47);
            this.btnAddProduct.TabIndex = 9;
            this.btnAddProduct.Text = "Add Product";
            this.btnAddProduct.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnAddProduct.UseVisualStyleBackColor = false;
            // 
            // panelBatchDetails
            // 
            this.panelBatchDetails.BackColor = System.Drawing.Color.White;
            this.panelBatchDetails.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelBatchDetails.Controls.Add(this.label8);
            this.panelBatchDetails.Controls.Add(this.dgvBatchDetails);
            this.panelBatchDetails.Location = new System.Drawing.Point(16, 332);
            this.panelBatchDetails.Margin = new System.Windows.Forms.Padding(4);
            this.panelBatchDetails.Name = "panelBatchDetails";
            this.panelBatchDetails.Size = new System.Drawing.Size(1541, 369);
            this.panelBatchDetails.TabIndex = 3;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold);
            this.label8.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(11)))), ((int)(((byte)(64)))), ((int)(((byte)(31)))));
            this.label8.Location = new System.Drawing.Point(13, 12);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(124, 25);
            this.label8.TabIndex = 0;
            this.label8.Text = "Batch Details";
            // 
            // dgvBatchDetails
            // 
            this.dgvBatchDetails.BackgroundColor = System.Drawing.Color.White;
            this.dgvBatchDetails.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvBatchDetails.Location = new System.Drawing.Point(19, 49);
            this.dgvBatchDetails.Margin = new System.Windows.Forms.Padding(4);
            this.dgvBatchDetails.Name = "dgvBatchDetails";
            this.dgvBatchDetails.RowHeadersWidth = 51;
            this.dgvBatchDetails.Size = new System.Drawing.Size(1504, 302);
            this.dgvBatchDetails.TabIndex = 1;
            // 
            // panelPayment
            // 
            this.panelPayment.BackColor = System.Drawing.Color.White;
            this.panelPayment.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelPayment.Controls.Add(this.Total);
            this.panelPayment.Controls.Add(this.txtTotalAmount);
            this.panelPayment.Controls.Add(this.label9);
            this.panelPayment.Controls.Add(this.txtPaidAmount);
            this.panelPayment.Controls.Add(this.label12);
            this.panelPayment.Controls.Add(this.txtRemarks);
            this.panelPayment.Location = new System.Drawing.Point(16, 714);
            this.panelPayment.Margin = new System.Windows.Forms.Padding(4);
            this.panelPayment.Name = "panelPayment";
            this.panelPayment.Size = new System.Drawing.Size(999, 123);
            this.panelPayment.TabIndex = 4;
            // 
            // Total
            // 
            this.Total.AutoSize = true;
            this.Total.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.Total.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(11)))), ((int)(((byte)(64)))), ((int)(((byte)(31)))));
            this.Total.Location = new System.Drawing.Point(13, 43);
            this.Total.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Total.Name = "Total";
            this.Total.Size = new System.Drawing.Size(113, 23);
            this.Total.TabIndex = 8;
            this.Total.Text = "Total Amount";
            // 
            // txtTotalAmount
            // 
            this.txtTotalAmount.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtTotalAmount.Location = new System.Drawing.Point(19, 74);
            this.txtTotalAmount.Margin = new System.Windows.Forms.Padding(4);
            this.txtTotalAmount.Name = "txtTotalAmount";
            this.txtTotalAmount.Size = new System.Drawing.Size(199, 30);
            this.txtTotalAmount.TabIndex = 9;
            this.txtTotalAmount.Text = "0";
            this.txtTotalAmount.TextChanged += new System.EventHandler(this.txtTotalAmount_TextChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.label9.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(11)))), ((int)(((byte)(64)))), ((int)(((byte)(31)))));
            this.label9.Location = new System.Drawing.Point(373, 43);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(110, 23);
            this.label9.TabIndex = 0;
            this.label9.Text = "Paid Amount";
            // 
            // txtPaidAmount
            // 
            this.txtPaidAmount.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtPaidAmount.Location = new System.Drawing.Point(379, 74);
            this.txtPaidAmount.Margin = new System.Windows.Forms.Padding(4);
            this.txtPaidAmount.Name = "txtPaidAmount";
            this.txtPaidAmount.Size = new System.Drawing.Size(199, 30);
            this.txtPaidAmount.TabIndex = 1;
            this.txtPaidAmount.Text = "0";
            this.txtPaidAmount.TextChanged += new System.EventHandler(this.TxtPaidAmount_TextChanged);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.label12.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(11)))), ((int)(((byte)(64)))), ((int)(((byte)(31)))));
            this.label12.Location = new System.Drawing.Point(693, 12);
            this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(76, 23);
            this.label12.TabIndex = 6;
            this.label12.Text = "Remarks";
            // 
            // txtRemarks
            // 
            this.txtRemarks.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtRemarks.Location = new System.Drawing.Point(699, 43);
            this.txtRemarks.Margin = new System.Windows.Forms.Padding(4);
            this.txtRemarks.Multiline = true;
            this.txtRemarks.Name = "txtRemarks";
            this.txtRemarks.Size = new System.Drawing.Size(279, 61);
            this.txtRemarks.TabIndex = 7;
            // 
            // btnSaveAll
            // 
            this.btnSaveAll.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(11)))), ((int)(((byte)(64)))), ((int)(((byte)(31)))));
            this.btnSaveAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaveAll.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnSaveAll.ForeColor = System.Drawing.Color.White;
            this.btnSaveAll.IconChar = FontAwesome.Sharp.IconChar.FloppyDisk;
            this.btnSaveAll.IconColor = System.Drawing.Color.LimeGreen;
            this.btnSaveAll.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnSaveAll.IconSize = 32;
            this.btnSaveAll.Location = new System.Drawing.Point(1093, 855);
            this.btnSaveAll.Margin = new System.Windows.Forms.Padding(4);
            this.btnSaveAll.Name = "btnSaveAll";
            this.btnSaveAll.Size = new System.Drawing.Size(213, 55);
            this.btnSaveAll.TabIndex = 6;
            this.btnSaveAll.Text = "Save All (Ctrl+S)";
            this.btnSaveAll.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSaveAll.UseVisualStyleBackColor = false;
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(76)))), ((int)(((byte)(60)))));
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.IconChar = FontAwesome.Sharp.IconChar.Remove;
            this.btnCancel.IconColor = System.Drawing.Color.White;
            this.btnCancel.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnCancel.IconSize = 28;
            this.btnCancel.Location = new System.Drawing.Point(1333, 855);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(224, 55);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "Cancel (Esc)";
            this.btnCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCancel.UseVisualStyleBackColor = false;
            // 
            // btnClearAll
            // 
            this.btnClearAll.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(140)))), ((int)(((byte)(66)))));
            this.btnClearAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClearAll.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnClearAll.ForeColor = System.Drawing.Color.White;
            this.btnClearAll.IconChar = FontAwesome.Sharp.IconChar.Eraser;
            this.btnClearAll.IconColor = System.Drawing.Color.White;
            this.btnClearAll.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnClearAll.IconSize = 28;
            this.btnClearAll.Location = new System.Drawing.Point(16, 855);
            this.btnClearAll.Margin = new System.Windows.Forms.Padding(4);
            this.btnClearAll.Name = "btnClearAll";
            this.btnClearAll.Size = new System.Drawing.Size(200, 55);
            this.btnClearAll.TabIndex = 7;
            this.btnClearAll.Text = "Clear All";
            this.btnClearAll.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnClearAll.UseVisualStyleBackColor = false;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(11)))), ((int)(((byte)(64)))), ((int)(((byte)(31)))));
            this.lblTitle.Location = new System.Drawing.Point(16, 11);
            this.lblTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(302, 41);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "New Batch Purchase";
            // 
            // UnifiedBatchPurchaseForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(255)))), ((int)(((byte)(228)))));
            this.ClientSize = new System.Drawing.Size(1573, 926);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.panelBatchInfo);
            this.Controls.Add(this.panelProductEntry);
            this.Controls.Add(this.panelBatchDetails);
            this.Controls.Add(this.panelPayment);
            this.Controls.Add(this.btnSaveAll);
            this.Controls.Add(this.btnClearAll);
            this.Controls.Add(this.btnCancel);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "UnifiedBatchPurchaseForm";
            this.Text = "New Batch Purchase";
            this.Load += new System.EventHandler(this.UnifiedBatchPurchaseForm_Load);
            this.panelBatchInfo.ResumeLayout(false);
            this.panelBatchInfo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnAddSupplier)).EndInit();
            this.panelProductEntry.ResumeLayout(false);
            this.panelProductEntry.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProductSearch)).EndInit();
            this.panelBatchDetails.ResumeLayout(false);
            this.panelBatchDetails.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBatchDetails)).EndInit();
            this.panelPayment.ResumeLayout(false);
            this.panelPayment.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTitle;

        // Batch Info Panel
        private System.Windows.Forms.Panel panelBatchInfo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtBatchName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbSupplier;
        private FontAwesome.Sharp.IconPictureBox btnAddSupplier;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker dtpReceivedDate;

        // Product Entry Panel
        private System.Windows.Forms.Panel panelProductEntry;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtProductSearch;
        private System.Windows.Forms.DataGridView dgvProductSearch;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtCostPrice;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtSalePrice;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtQuantity;
        private FontAwesome.Sharp.IconButton btnAddProduct;

        // Batch Details Panel
        private System.Windows.Forms.Panel panelBatchDetails;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.DataGridView dgvBatchDetails;

        // Payment Panel
        private System.Windows.Forms.Panel panelPayment;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtPaidAmount;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtRemarks;

        // Action Buttons
        private FontAwesome.Sharp.IconButton btnSaveAll;
        private FontAwesome.Sharp.IconButton btnClearAll;
        private FontAwesome.Sharp.IconButton btnCancel;
        private System.Windows.Forms.Label Total;
        private System.Windows.Forms.TextBox txtTotalAmount;
    }
}