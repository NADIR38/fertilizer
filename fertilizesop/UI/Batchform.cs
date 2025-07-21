using fertilizesop.BL.Models;
using fertilizesop.Interfaces.BLInterfaces;
using Microsoft.Extensions.DependencyInjection;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace fertilizesop.UI
{
    public partial class Batchform : Form
    {
        private readonly IBatchesBl ibl;
        private int selectedid = -1;
        public Batchform(IBatchesBl ibl)

        {

            InitializeComponent();
            this.ibl = ibl;
            UIHelper.StyleGridView(dataGridView2);
            paneledit.Visible = false;
            UIHelper.ApplyButtonStyles(dataGridView2);
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
                    OpenBatchDetailsForm();  // ✅ Open the batch details form with batch name
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
            }
            else if (e.KeyCode == Keys.Enter)
            {
                if (!paneledit.Visible && dataGridView2.Focused)
                {
                    editSelectedCustomer();
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
            var f = Program.ServiceProvider.GetRequiredService<Addbatchform>();
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
                var batch = new Batches(selectedid, name, date, supplier_name, "");

                var result = ibl.UpdateBatch(batch);

                if (result)
                {
                    MessageBox.Show("Batch Updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close(); // Optional: close form after success
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
        }
        private void OpenBatchDetailsForm()
        {
            if (dataGridView2.CurrentRow != null)
            {
                var batchName = dataGridView2.CurrentRow.Cells["batch_name"].Value?.ToString();

                if (!string.IsNullOrEmpty(batchName))
                {
                    var form = Program.ServiceProvider.GetRequiredService<Addbatchdetailsform>();
                    form.InitialBatchName = batchName; // ✅ Inject batch name via property
                    form.ShowDialog(this);
                }
                else
                {
                    MessageBox.Show("No batch name found in selected row.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void iconPictureBox2_Click(object sender, EventArgs e)
        {
            var f = Program.ServiceProvider.GetRequiredService<Addsupplier>();
            f.ShowDialog(this);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            load();
        }
    }
}
