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
        private DataGridView dgvproductsearch= new DataGridView();
        Customersaledl _customersaledl = new Customersaledl();
        public Customersale()
        {
            InitializeComponent();
            setupproductsearch();
            txtproductsearch.TextChanged += txtproductsearch_TextChanged;
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
            dgvproductsearch.Location = new System.Drawing.Point(50, 200); // ✅ Correct
            dgvproductsearch.Size = new System.Drawing.Size(dataGridView1.Width, dataGridView1.Height/2);
            dgvproductsearch.BringToFront();
            //dgvproductsearch.CellClick += dgvproductsearch_CellCliick;
        }

        //private void dgvproductsearch_CellContentClick(object sender, DataGridViewCellEventArgs e)
        //{
        //    if (e.RowIndex >= 0)
        //    {
        //        txtcustomer.Text = dgvproductsearch.Rows[e.RowIndex].Cells["name"].Value.ToString();
        //        dgvproductsearch.Visible = false;
        //    }
        //}

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void txtproductsearch_TextChanged(object sender, EventArgs e)
        {
            if(string.IsNullOrWhiteSpace(txtproductsearch.Text))
            {
                clearfields();
                return;
            }
            if (dgvproductsearch.Columns.Contains("product_id"))
            {
                dgvproductsearch.Columns["product_id"].Visible = false;
            }
            dgvproductsearch.Visible=true;
            DataTable dt = new DataTable();
            dt = _customersaledl.getproductthings(txtproductsearch.Text);
            dgvproductsearch.DataSource = dt;
        }
        private void clearfields()
        {
            txtcustsearch.Text = string.Empty;
            txtproductsearch.Text = string.Empty; txtproductsearch.Focus();
        }
    }
}
