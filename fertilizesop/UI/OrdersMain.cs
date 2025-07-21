using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using fertilizesop.BL.Bl;
using fertilizesop.BL.Models;
using fertilizesop.DL;
using fertilizesop.Interfaces.BLInterfaces;
using fertilizesop.Interfaces.DLinterfaces;
using KIMS;
using static System.Net.Mime.MediaTypeNames;

namespace fertilizesop.UI
{

    public partial class OrdersMain : Form
    {
        OrderBl o=new OrderBl();
        int orderId;
        DataTable dt;

        public OrdersMain()
        {
            InitializeComponent();
            orderdata.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            date.Value = DateTime.Now;
            LoadOrderGrid();
            LoadSuppliers();
            paneledit.Visible = false;
            this.KeyPreview = true; // Put this in OrdersMain constructor
            this.KeyDown += OrdersMain_KeyDown;


        }
        private void LoadSuppliers()
        {
            
            DataTable dt = o.GetAllSuppliers();

            cmbSuppliers.DataSource = dt;
            cmbSuppliers.DisplayMember = "name";          // What user sees
            cmbSuppliers.ValueMember = "supplier_id";     // What you actually use
            cmbSuppliers.SelectedIndex = -1;              // Optional: no item selected

        }

        private void LoadOrderGrid()
        {
            dt = o.LoadOrdersWithDetails(); // replace with actual class
            orderdata.DataSource = dt;
        }

        private void btnsave_Click(object sender, EventArgs e)
        {
            SaveOrder();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void iconButton9_Click(object sender, EventArgs e)
        {
            paneledit.Visible = true;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            LoadOrderGrid();
            paneledit.Visible = false;
        }

        private void OrdersMain_Load(object sender, EventArgs e)
        {
                                                                                                                                                                                                                                
        }

        private void btncancle1_Click(object sender, EventArgs e)
        {
            paneledit.Visible = false;
        }

        // search function
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text.Trim().Replace("'", "''");

            if (dt != null)
            {
                DataView dv = dt.DefaultView;
                dv.RowFilter = $@"
            Convert(OrderID, 'System.String') LIKE '%{keyword}%' OR
            SupplierName LIKE '%{keyword}%' OR
            ProductName LIKE '%{keyword}%'";

                orderdata.DataSource = dv;
            }
        }

        // keys logic here 

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
        
        private void SaveOrder()
        {
            if (string.IsNullOrWhiteSpace(cmbSuppliers.Text) || cmbSuppliers.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a supplier before proceeding.", "Missing Supplier", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbSuppliers.Focus();
                return;
            }


            string supp = cmbSuppliers.Text;
            int supplierId = Convert.ToInt32(cmbSuppliers.SelectedValue);
            DateTime datePicker = date.Value;
            Order or = new Order(supplierId, datePicker);
            orderId = o.InsertOrder(or);
            MessageBox.Show($"Order with orderId {orderId} created! Now add products.");
            paneledit.Visible = false;
            PlacingOrder placingOrder = new PlacingOrder(orderId, supp);
            placingOrder.Show();
            
        }

        private void OrdersMain_KeyDown(object sender, KeyEventArgs e)
        {
            // Prevent duplicate handling
            if (e.Handled) return;

            // Enter → Open Edit Panel
            if (e.KeyCode == Keys.Enter)
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
    }
}
