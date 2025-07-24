using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using fertilizesop.BL.Bl;
using fertilizesop.BL.Models;
using FontAwesome.Sharp;

namespace fertilizesop.UI
{
    public partial class customer_bills : Form
    {
        private readonly Customerbillbl _billingBL;
        public customer_bills()
        {
            InitializeComponent();
            _billingBL = new Customerbillbl();
            this.Load += Customerbill_Load;
            paneledit.Visible = false;
            UIHelper.StyleGridView(dataGridView2);
        }

        private void Customerbill_Load(object sender, EventArgs e)
        {

            load();
            dataGridView2.Focus();
        }

        //private void load()
        //{
        //    try
        //    {
        //        var list = _billingBL.getbill();

        //        // Filter out bills where total_price == 0
        //        var filteredList = list.Where(b => b.total_price != 0.00m).ToList();

        //        dataGridView2.Columns.Clear();
        //        dataGridView2.DataSource = filteredList;
        //        dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        //        if (dataGridView2.Columns.Contains("CustomerID"))
        //        {
        //            dataGridView2.Columns["CustomerID"].Visible = false;
        //        }

        //        UIHelper.AddButtonColumn(dataGridView2, "Edit", "View Details", "Details");
        //        UIHelper.AddButtonColumn(dataGridView2, "Delete", "Add payment", "payement");
        //    }
        //    catch(Exception ex)
        //    {
        //        MessageBox.Show("Error in fetching the customerbills " + ex.Message);
        //    }
        //}

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dataGridView2.Columns[e.ColumnIndex] is DataGridViewButtonColumn)
            {
                string columnName = dataGridView2.Columns[e.ColumnIndex].Name;

                if (columnName == "Edit")
                {
                    var selectedBill = dataGridView2.Rows[e.RowIndex].DataBoundItem as Supplierbill;

                    if (selectedBill != null)
                    {
                        // Open new form and pass selected bill ID
                        var detailsForm = new Sbilldetailform(selectedBill.bill_id);
                        detailsForm.ShowDialog();
                    }
                }
                if (columnName == "Delete")
                {
                    var selectedBill = dataGridView2.Rows[e.RowIndex].DataBoundItem as Supplierbill;

                    if (selectedBill != null)
                    {
                        // Assign data to panel textboxes (ensure these exist on your form)
                        txtname1.Text = selectedBill.supplier_name;
                        txtamount.Text = selectedBill.pending.ToString("0.00");
                        txtbill.Text = selectedBill.bill_id.ToString();
                        txtdate.Text = DateTime.Now.ToString("yyyy-MM-dd");

                        // Show the panel centered
                        UIHelper.RoundPanelCorners(paneledit, 20);
                        UIHelper.ShowCenteredPanel(this, paneledit);
                    }
                }

            }
        }

        private void load()
        {
            try
            {
                var list = _billingBL.getbill();
                MessageBox.Show($"Fetched {list.Count} bills from DB");

                var filteredList = list.Where(b => b.total_price != 0.00m).ToList();

                if (filteredList.Count == 0)
                {
                    MessageBox.Show("No bills with non-zero total_price were found.");
                }

                dataGridView2.Columns.Clear();
                dataGridView2.DataSource = filteredList;
                dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                if (dataGridView2.Columns.Contains("customer_id"))
                {
                    dataGridView2.Columns["customer_id"].Visible = false;
                }

                if (dataGridView2.Columns.Contains("batch_name"))
                {
                    dataGridView2.Columns["batch_name"].Visible = false;
                }

                UIHelper.AddButtonColumn(dataGridView2, "Edit", "View Details", "Details");
                UIHelper.AddButtonColumn(dataGridView2, "Delete", "Add payment", "payment");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in fetching the customer bills: " + ex.Message);
            }
        }


        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Enter)
            {
                if (paneledit.Visible)
                {
                    btnsave1.PerformClick();
                    return true;
                }

            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void LoadBillingRecords(string searchTerm = "")
        {
            try
            {
                DataTable dt = _billingBL.GetBillingRecords(searchTerm);
                dataGridView2.DataSource = dt;

                if (dt.Rows.Count > 0)
                {
                    dataGridView2.ClearSelection();
                    AddDetailsButtonColumn();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading billing records: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AddDetailsButtonColumn()
        {
            if (dataGridView2.Columns.Contains("btnDetails"))
                dataGridView2.Columns.Remove("btnDetails");
            if (dataGridView2.Columns.Contains("btnPayment"))
                dataGridView2.Columns.Remove("btnPayment");

            // View Details Button
            var btnDetails = new DataGridViewButtonColumn
            {
                Name = "btnDetails",
                Text = "View Details",
                UseColumnTextForButtonValue = true,
                HeaderText = "Actions",
                FlatStyle = FlatStyle.Flat,
                Width = 100,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    BackColor = Color.FromArgb(0, 126, 250),
                    ForeColor = Color.White,
                    Font = new Font("Segoe UI", 9, FontStyle.Bold)
                }
            };

            // Payment Button
            var btnPayment = new DataGridViewButtonColumn
            {
                Name = "btnPayment",
                Text = "Payment",
                UseColumnTextForButtonValue = true,
                HeaderText = "",
                FlatStyle = FlatStyle.Flat,
                Width = 100,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    BackColor = Color.FromArgb(46, 204, 113),
                    ForeColor = Color.White,
                    Font = new Font("Segoe UI", 9, FontStyle.Bold)
                }
            };

            dataGridView2.Columns.Add(btnDetails);
            dataGridView2.Columns.Add(btnPayment);
        }


       

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void txtdate_TextChanged(object sender, EventArgs e)
        {

        }

        private void toplbl_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void txtamount_TextChanged(object sender, EventArgs e)
        {

        }

        private void button9_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void txtname1_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox10_Click(object sender, EventArgs e)
        {
            load();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void txtpayment_TextChanged(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void btnsave1_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void txtbill_TextChanged(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btncancle1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(textBox1.Text))
                {
                    return;
                }
                var billdeta = _billingBL.searchbill(textBox1.Text);
                dataGridView2.DataSource = billdeta;
                dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                if (dataGridView2.Columns.Contains("customer_id"))
                {
                    dataGridView2.Columns["customer_id"].Visible = false;
                }

                if (dataGridView2.Columns.Contains("batch_name"))
                {
                    dataGridView2.Columns["batch_name"].Visible = false;
                }

                UIHelper.AddButtonColumn(dataGridView2, "Edit", "View Details", "Details");
                UIHelper.AddButtonColumn(dataGridView2, "Delete", "Add payment", "payment");
            }catch (Exception ex)
            {
                MessageBox.Show("error in searching " + ex.Message );
            }
        }

        private void txtremarks_TextChanged(object sender, EventArgs e)
        {

        }

        private void paneledit_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
