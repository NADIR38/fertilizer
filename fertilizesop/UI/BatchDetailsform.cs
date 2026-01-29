using fertilizesop.BL.Models;
using fertilizesop.DL;
using fertilizesop.Interfaces.BLInterfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace fertilizesop.UI
{
    public partial class BatchDetailsform : Form
    {
        public int BatchId { get; set; }
        private readonly IbatchdetailsBl ibl;

        public BatchDetailsform(IbatchdetailsBl ibl)
        {
            InitializeComponent();
            this.ibl = ibl;

            UIHelper.StyleGridView(dataGridView2);
            this.KeyPreview = true;
            this.KeyDown += CustomerForm_KeyDown;
        }

        private void CustomerForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.R)
            {
                pictureBox1_Click(sender, e);
                e.Handled = true;
            }
        }

        private void BatchDetailsform_Load(object sender, EventArgs e)
        {
            if (BatchId != 0)
                LoadBatchDetails();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            LoadBatchDetails();
        }

        private void LoadBatchDetails()
        {
            var list = BatchdetailsDl.GetAllBatchDetails(BatchId);
            BindGrid(list);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string searchText = textBox1.Text.Trim();

            List<BatchDetails> list;
            if (string.IsNullOrEmpty(searchText))
            {
                list = BatchdetailsDl.GetAllBatchDetails(BatchId);
            }
            else
            {
                // ✅ Pass BatchId to the search method
                list = BatchdetailsDl.SearchBatchDetails(BatchId, searchText);
            }

            BindGrid(list);
        }

        private void BindGrid(List<BatchDetails> list)
        {
            dataGridView2.Columns.Clear();
            dataGridView2.DataSource = list;

            // Hide columns we don't want visible
            if (dataGridView2.Columns.Contains("details_id"))
                dataGridView2.Columns["details_id"].Visible = false;
            if (dataGridView2.Columns.Contains("product_id"))
                dataGridView2.Columns["product_id"].Visible = false;
            if (dataGridView2.Columns.Contains("sale_price"))
                dataGridView2.Columns["sale_price"].Visible = false;
        }

        private void iconButton9_Click(object sender, EventArgs e)
        {
            // Add new batch detail logic can go here
        }
    }
}
