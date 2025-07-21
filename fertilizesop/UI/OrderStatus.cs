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
using fertilizesop.BL.Models.persons;
using fertilizesop.DL;
using KIMS;

namespace fertilizesop.UI
{
    public partial class OrderStatus : Form
    {
        DataTable dt;
        OrderBl o = new OrderBl();
        OrderDAL or =new OrderDAL();
        int orderId;
        string supp;
        public OrderStatus()
        {
            InitializeComponent();
            LoadOrderGrid();
            date.Value = DateTime.Now;
            ordersdata.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            cmbSuppliers.TextUpdate += cmbSuppliers_TextUpdate;
            cmbSuppliers.DropDownStyle = ComboBoxStyle.DropDown;
            //ordersdata.Columns["Order_id"].Visible=false;
            paneledit.Visible = false;

        }


        private void LoadOrderGrid()
        {
            dt = o.GetOrders(); // replace with actual class
            ordersdata.DataSource = dt;
        }
        private void paneledit_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btncancle1_Click(object sender, EventArgs e)
        {
            paneledit.Visible = false;
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text.Trim().Replace("'", "''");

            if (dt != null)
            {
                DataView dv = dt.DefaultView;
                dv.RowFilter = $@"
            Convert(OrderID, 'System.String') LIKE '%{keyword}%' OR
            SupplierName LIKE '%{keyword}%' OR
            Order Status LIKE '%{keyword}%'";

                ordersdata.DataSource = dv;
            }
        }

        private void iconButton9_Click(object sender, EventArgs e)
        {
            paneledit.Visible = true;
        }
        private void cmbSuppliers_TextUpdate(object sender, EventArgs e)
        {
            string searchText = cmbSuppliers.Text.Trim();

            List<Suppliers> filteredSuppliers =or.GetSuppliers(searchText);

            if (filteredSuppliers != null && filteredSuppliers.Count > 0)
            {
                cmbSuppliers.BeginUpdate();
                cmbSuppliers.DataSource = null;
                cmbSuppliers.DataSource = filteredSuppliers;
                cmbSuppliers.DisplayMember = "first_Name"; // match your Suppliers class
                cmbSuppliers.ValueMember = "Id";
                cmbSuppliers.DroppedDown = true;

                cmbSuppliers.SelectionStart = searchText.Length;
                cmbSuppliers.SelectionLength = 0;
                cmbSuppliers.EndUpdate();
            }
        }

        private void SaveOrder()
        {
            //if (string.IsNullOrWhiteSpace(cmbSuppliers.Text) || cmbSuppliers.SelectedIndex == -1)
            //{
            //    MessageBox.Show("Please select a supplier before proceeding.", "Missing Supplier", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    cmbSuppliers.Focus();
            //    return;
            //}

            //string sts=status.Text;
            //supp = cmbSuppliers.Text;
            //int supplierId = Convert.ToInt32(cmbSuppliers.SelectedValue);
            //DateTime datePicker = date.Value;
            //Order or = new Order(supplierId, datePicker,sts);
            //int orderIdd = o.InsertOrder(or);
            //MessageBox.Show($"Order with orderId {orderId} created! Now add products.");
            //paneledit.Visible = false;


            if (cmbSuppliers.SelectedItem == null || string.IsNullOrWhiteSpace(cmbSuppliers.Text))
            {
                MessageBox.Show("Please select a supplier before proceeding.", "Missing Supplier", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbSuppliers.Focus();
                return;
            }

            // Get selected supplier info
            var selectedSupplier = cmbSuppliers.SelectedItem as Suppliers;
            if (selectedSupplier == null)
            {
                MessageBox.Show("Invalid supplier selection.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int supplierId = selectedSupplier.Id;
            supp = selectedSupplier.first_Name;
            string sts = status.Text;
            DateTime datePicker = date.Value;

            Order or = new Order(supplierId, datePicker, sts);
            int orderIdd = o.InsertOrder(or);

            MessageBox.Show($"Order with orderId {orderIdd} created! Now add products.");
            paneledit.Visible = false;

        }
        private void cmbSuppliers_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            LoadOrderGrid();
        }

        private void iconButton1_Click(object sender, EventArgs e)
        {
            if (ordersdata.SelectedRows.Count > 0 || ordersdata.SelectedRows.Count ==1)
            {
                orderId = Convert.ToInt32(ordersdata.SelectedRows[0].Cells["OrderID"].Value);

               
                PlacingOrder placingOrder = new PlacingOrder(orderId, supp);
                placingOrder.Show();
            }
            else
            {
                MessageBox.Show("Please select a row first.");
            }
            
        }

        //keys logic 

        private void OpenEditPanel()
        {
            if (paneledit.Visible == false)
            {
                paneledit.Visible = true;
            }


        }

        private void CancelEdit()
        {
            paneledit.Visible = false;

        }

        private void OrdersMain_KeyDown(object sender, KeyEventArgs e)
        {
            // Prevent duplicate handling
            if (e.Handled) return;

            // Enter → Open Edit Panel
            if (e.Control && e.KeyCode == Keys.A)
            {
                OpenEditPanel();
                e.Handled = true; // Mark as handled
            }
            // Esc → Cancel Edit
            else if (e.KeyCode == Keys.Escape)
            {
                CancelEdit();
                e.Handled = true; // Mark as handled
            }
            // Ctrl + S → Save
            else if (e.Control && e.KeyCode == Keys.S)
            {
                SaveOrder();
                e.Handled = true; // Mark as handled
            }
            // Ctrl + R → Refresh
            else if (e.Control && e.KeyCode == Keys.R)
            {
                LoadOrderGrid();
                e.Handled = true; // Mark as handled
            }
        }

        private void btnsave_Click(object sender, EventArgs e)
        {
            SaveOrder();
        }
    }
}
