namespace fertilizesop.UI
{
    partial class BulkSupplierPaymentForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dgvSuppliers = new System.Windows.Forms.DataGridView();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.lblSearch = new System.Windows.Forms.Label();
            this.grpPayment = new System.Windows.Forms.GroupBox();
            this.txtPaymentAmount = new System.Windows.Forms.TextBox();
            this.txtRemarks = new System.Windows.Forms.TextBox();
            this.lblPaymentAmount = new System.Windows.Forms.Label();
            this.lblRemarks = new System.Windows.Forms.Label();
            this.btnProcessPayment = new System.Windows.Forms.Button();
            this.lblSelectedSupplier = new System.Windows.Forms.Label();
            this.lblTotalPending = new System.Windows.Forms.Label();
            this.lblTotalPaid = new System.Windows.Forms.Label();
            this.btnRefresh = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSuppliers)).BeginInit();
            this.grpPayment.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvSuppliers
            // 
            this.dgvSuppliers.AllowUserToAddRows = false;
            this.dgvSuppliers.AllowUserToDeleteRows = false;
            this.dgvSuppliers.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvSuppliers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSuppliers.Location = new System.Drawing.Point(12, 45);
            this.dgvSuppliers.MultiSelect = false;
            this.dgvSuppliers.Name = "dgvSuppliers";
            this.dgvSuppliers.ReadOnly = true;
            this.dgvSuppliers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSuppliers.Size = new System.Drawing.Size(776, 300);
            this.dgvSuppliers.TabIndex = 0;
            this.dgvSuppliers.SelectionChanged += new System.EventHandler(this.DgvSuppliers_SelectionChanged);
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(70, 12);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(618, 20);
            this.txtSearch.TabIndex = 1;
            this.txtSearch.TextChanged += new System.EventHandler(this.TxtSearch_TextChanged);
            // 
            // lblSearch
            // 
            this.lblSearch.AutoSize = true;
            this.lblSearch.Location = new System.Drawing.Point(12, 15);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new System.Drawing.Size(47, 13);
            this.lblSearch.TabIndex = 2;
            this.lblSearch.Text = "Search:";
            // 
            // grpPayment
            // 
            this.grpPayment.Controls.Add(this.lblSelectedSupplier);
            this.grpPayment.Controls.Add(this.lblTotalPending);
            this.grpPayment.Controls.Add(this.lblTotalPaid);
            this.grpPayment.Controls.Add(this.txtPaymentAmount);
            this.grpPayment.Controls.Add(this.txtRemarks);
            this.grpPayment.Controls.Add(this.lblPaymentAmount);
            this.grpPayment.Controls.Add(this.lblRemarks);
            this.grpPayment.Controls.Add(this.btnProcessPayment);
            this.grpPayment.Location = new System.Drawing.Point(12, 351);
            this.grpPayment.Name = "grpPayment";
            this.grpPayment.Size = new System.Drawing.Size(776, 180);
            this.grpPayment.TabIndex = 3;
            this.grpPayment.TabStop = false;
            this.grpPayment.Text = "Bulk Payment";
            // 
            // lblSelectedSupplier
            // 
            this.lblSelectedSupplier.AutoSize = true;
            this.lblSelectedSupplier.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.lblSelectedSupplier.Location = new System.Drawing.Point(15, 25);
            this.lblSelectedSupplier.Name = "lblSelectedSupplier";
            this.lblSelectedSupplier.Size = new System.Drawing.Size(200, 17);
            this.lblSelectedSupplier.TabIndex = 0;
            this.lblSelectedSupplier.Text = "Select a supplier above";
            // 
            // lblTotalPending
            // 
            this.lblTotalPending.AutoSize = true;
            this.lblTotalPending.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.lblTotalPending.ForeColor = System.Drawing.Color.Red;
            this.lblTotalPending.Location = new System.Drawing.Point(15, 50);
            this.lblTotalPending.Name = "lblTotalPending";
            this.lblTotalPending.Size = new System.Drawing.Size(100, 15);
            this.lblTotalPending.TabIndex = 1;
            this.lblTotalPending.Text = "Total Pending: -";
            // 
            // lblTotalPaid
            // 
            this.lblTotalPaid.AutoSize = true;
            this.lblTotalPaid.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.lblTotalPaid.ForeColor = System.Drawing.Color.Green;
            this.lblTotalPaid.Location = new System.Drawing.Point(15, 70);
            this.lblTotalPaid.Name = "lblTotalPaid";
            this.lblTotalPaid.Size = new System.Drawing.Size(80, 15);
            this.lblTotalPaid.TabIndex = 2;
            this.lblTotalPaid.Text = "Total Paid: -";
            // 
            // txtPaymentAmount
            // 
            this.txtPaymentAmount.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.txtPaymentAmount.Location = new System.Drawing.Point(140, 95);
            this.txtPaymentAmount.Name = "txtPaymentAmount";
            this.txtPaymentAmount.Size = new System.Drawing.Size(200, 23);
            this.txtPaymentAmount.TabIndex = 4;
            // 
            // txtRemarks
            // 
            this.txtRemarks.Location = new System.Drawing.Point(140, 125);
            this.txtRemarks.Multiline = true;
            this.txtRemarks.Name = "txtRemarks";
            this.txtRemarks.Size = new System.Drawing.Size(400, 40);
            this.txtRemarks.TabIndex = 5;
            // 
            // lblPaymentAmount
            // 
            this.lblPaymentAmount.AutoSize = true;
            this.lblPaymentAmount.Location = new System.Drawing.Point(15, 100);
            this.lblPaymentAmount.Name = "lblPaymentAmount";
            this.lblPaymentAmount.Size = new System.Drawing.Size(95, 13);
            this.lblPaymentAmount.TabIndex = 6;
            this.lblPaymentAmount.Text = "Payment Amount:";
            // 
            // lblRemarks
            // 
            this.lblRemarks.AutoSize = true;
            this.lblRemarks.Location = new System.Drawing.Point(15, 128);
            this.lblRemarks.Name = "lblRemarks";
            this.lblRemarks.Size = new System.Drawing.Size(52, 13);
            this.lblRemarks.TabIndex = 7;
            this.lblRemarks.Text = "Remarks:";
            // 
            // btnProcessPayment
            // 
            this.btnProcessPayment.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.btnProcessPayment.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnProcessPayment.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.btnProcessPayment.ForeColor = System.Drawing.Color.White;
            this.btnProcessPayment.Location = new System.Drawing.Point(560, 95);
            this.btnProcessPayment.Name = "btnProcessPayment";
            this.btnProcessPayment.Size = new System.Drawing.Size(200, 70);
            this.btnProcessPayment.TabIndex = 8;
            this.btnProcessPayment.Text = "Process Payment";
            this.btnProcessPayment.UseVisualStyleBackColor = false;
            this.btnProcessPayment.Click += new System.EventHandler(this.BtnProcessPayment_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(694, 10);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(94, 23);
            this.btnRefresh.TabIndex = 4;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.BtnRefresh_Click);
            // 
            // BulkSupplierPaymentForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 543);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.grpPayment);
            this.Controls.Add(this.lblSearch);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.dgvSuppliers);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "BulkSupplierPaymentForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Bulk Supplier Payment";
            ((System.ComponentModel.ISupportInitialize)(this.dgvSuppliers)).EndInit();
            this.grpPayment.ResumeLayout(false);
            this.grpPayment.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvSuppliers;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.GroupBox grpPayment;
        private System.Windows.Forms.TextBox txtPaymentAmount;
        private System.Windows.Forms.TextBox txtRemarks;
        private System.Windows.Forms.Label lblPaymentAmount;
        private System.Windows.Forms.Label lblRemarks;
        private System.Windows.Forms.Button btnProcessPayment;
        private System.Windows.Forms.Label lblSelectedSupplier;
        private System.Windows.Forms.Label lblTotalPending;
        private System.Windows.Forms.Label lblTotalPaid;
        private System.Windows.Forms.Button btnRefresh;
    }
}
