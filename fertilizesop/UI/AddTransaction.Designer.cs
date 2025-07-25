namespace fertilizesop.UI
{
    partial class AddTransaction
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
            this.dtpDate = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtAmount = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.cmbTransactionType = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btncancle1 = new FontAwesome.Sharp.IconButton();
            this.btnsave = new FontAwesome.Sharp.IconButton();
            this.SuspendLayout();
            // 
            // dtpDate
            // 
            this.dtpDate.Location = new System.Drawing.Point(81, 229);
            this.dtpDate.Name = "dtpDate";
            this.dtpDate.Size = new System.Drawing.Size(313, 22);
            this.dtpDate.TabIndex = 202;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(11)))), ((int)(((byte)(64)))), ((int)(((byte)(31)))));
            this.label4.Location = new System.Drawing.Point(76, 196);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(60, 30);
            this.label4.TabIndex = 206;
            this.label4.Text = "Date";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(11)))), ((int)(((byte)(64)))), ((int)(((byte)(31)))));
            this.label3.Location = new System.Drawing.Point(76, 370);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(127, 30);
            this.label3.TabIndex = 205;
            this.label3.Text = "Description";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtAmount
            // 
            this.txtAmount.BackColor = System.Drawing.Color.Gainsboro;
            this.txtAmount.Location = new System.Drawing.Point(81, 137);
            this.txtAmount.Multiline = true;
            this.txtAmount.Name = "txtAmount";
            this.txtAmount.Size = new System.Drawing.Size(313, 35);
            this.txtAmount.TabIndex = 199;
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(11)))), ((int)(((byte)(64)))), ((int)(((byte)(31)))));
            this.label6.Location = new System.Drawing.Point(76, 104);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(94, 30);
            this.label6.TabIndex = 204;
            this.label6.Text = "Amount";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(11)))), ((int)(((byte)(64)))), ((int)(((byte)(31)))));
            this.label1.Location = new System.Drawing.Point(105, 47);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(270, 38);
            this.label1.TabIndex = 203;
            this.label1.Text = "Add Transaction";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtDescription
            // 
            this.txtDescription.BackColor = System.Drawing.Color.Gainsboro;
            this.txtDescription.Location = new System.Drawing.Point(81, 403);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(313, 111);
            this.txtDescription.TabIndex = 207;
            // 
            // cmbTransactionType
            // 
            this.cmbTransactionType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTransactionType.FormattingEnabled = true;
            this.cmbTransactionType.Items.AddRange(new object[] {
            "Withdraw",
            "Deposit"});
            this.cmbTransactionType.Location = new System.Drawing.Point(81, 320);
            this.cmbTransactionType.Name = "cmbTransactionType";
            this.cmbTransactionType.Size = new System.Drawing.Size(313, 24);
            this.cmbTransactionType.TabIndex = 209;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(11)))), ((int)(((byte)(64)))), ((int)(((byte)(31)))));
            this.label2.Location = new System.Drawing.Point(76, 287);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(178, 30);
            this.label2.TabIndex = 210;
            this.label2.Text = "Transaction Type";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btncancle1
            // 
            this.btncancle1.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btncancle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(11)))), ((int)(((byte)(64)))), ((int)(((byte)(31)))));
            this.btncancle1.FlatAppearance.BorderColor = System.Drawing.Color.Indigo;
            this.btncancle1.FlatAppearance.BorderSize = 2;
            this.btncancle1.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DodgerBlue;
            this.btncancle1.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(5)))), ((int)(((byte)(51)))), ((int)(((byte)(69)))));
            this.btncancle1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btncancle1.Font = new System.Drawing.Font("Segoe UI Semibold", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btncancle1.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btncancle1.IconChar = FontAwesome.Sharp.IconChar.FloppyDisk;
            this.btncancle1.IconColor = System.Drawing.Color.Red;
            this.btncancle1.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btncancle1.IconSize = 35;
            this.btncancle1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btncancle1.Location = new System.Drawing.Point(278, 563);
            this.btncancle1.Name = "btncancle1";
            this.btncancle1.Size = new System.Drawing.Size(151, 62);
            this.btncancle1.TabIndex = 211;
            this.btncancle1.Text = "Cancel";
            this.btncancle1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btncancle1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btncancle1.UseVisualStyleBackColor = false;
            this.btncancle1.Click += new System.EventHandler(this.btncancle1_Click);
            // 
            // btnsave
            // 
            this.btnsave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(11)))), ((int)(((byte)(64)))), ((int)(((byte)(31)))));
            this.btnsave.FlatAppearance.BorderColor = System.Drawing.Color.Indigo;
            this.btnsave.FlatAppearance.BorderSize = 2;
            this.btnsave.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DodgerBlue;
            this.btnsave.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(5)))), ((int)(((byte)(51)))), ((int)(((byte)(69)))));
            this.btnsave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnsave.Font = new System.Drawing.Font("Segoe UI Semibold", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnsave.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnsave.IconChar = FontAwesome.Sharp.IconChar.FloppyDisk;
            this.btnsave.IconColor = System.Drawing.Color.LimeGreen;
            this.btnsave.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnsave.IconSize = 35;
            this.btnsave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnsave.Location = new System.Drawing.Point(31, 563);
            this.btnsave.Name = "btnsave";
            this.btnsave.Size = new System.Drawing.Size(152, 62);
            this.btnsave.TabIndex = 201;
            this.btnsave.Text = "Save";
            this.btnsave.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnsave.UseVisualStyleBackColor = false;
            this.btnsave.Click += new System.EventHandler(this.btnsave_Click);
            // 
            // AddTransaction
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(255)))), ((int)(((byte)(228)))));
            this.ClientSize = new System.Drawing.Size(471, 665);
            this.Controls.Add(this.btncancle1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cmbTransactionType);
            this.Controls.Add(this.txtDescription);
            this.Controls.Add(this.btnsave);
            this.Controls.Add(this.dtpDate);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtAmount);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddTransaction";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AddTransaction";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private FontAwesome.Sharp.IconButton btnsave;
        private System.Windows.Forms.DateTimePicker dtpDate;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtAmount;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.ComboBox cmbTransactionType;
        private System.Windows.Forms.Label label2;
        private FontAwesome.Sharp.IconButton btncancle1;
    }
}