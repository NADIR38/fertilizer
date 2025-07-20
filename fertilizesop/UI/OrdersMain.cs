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
        OrderDAL o=new OrderDAL();
        private readonly Isupplierbl _customerbl = new Supplierbl(new Supplierdl()); // or whatever your concrete class is

        private List<Suppliers> allSuppliers = new List<Suppliers>();
        int orderId;

        public OrdersMain()
        {
            InitializeComponent();
            orderdata.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            LoadOrderGrid();
            LoadSuppliers();
            paneledit.Visible = false;

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
            var dt = o.LoadOrdersWithDetails(); // replace with actual class
            orderdata.DataSource = dt;
        }

        private void btnsave_Click(object sender, EventArgs e)
        {
            string supp=cmbSuppliers.Text;
            int supplierId = Convert.ToInt32(cmbSuppliers.SelectedValue);
            DateTime datePicker = date.Value;
            Order or=new Order(supplierId, datePicker);
            orderId = o.InsertOrder(or);
            MessageBox.Show($"Order with orderId {orderId} created! Now add products.");
            PlacingOrder placingOrder = new PlacingOrder(orderId,supp);
            placingOrder.Show();
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
        }
    }
}
