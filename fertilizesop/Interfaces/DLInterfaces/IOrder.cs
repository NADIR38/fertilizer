﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using fertilizesop.BL.Models;

namespace fertilizesop.Interfaces.DLinterfaces
{
    internal interface IOrder
    {
        int InsertOrder(Order order);
        void InsertOrderDetail(OrderDetail detail);
        void UpdateOrderDetail(OrderDetail detail);
        bool Delete(int orderDetailId);
        DataTable LoadOrdersWithDetails();
        DataTable GetProducts();
        DataTable GetAllSuppliers();
        DataTable GetOrders();

        void CreateOrderInvoicePdf(DataGridView cart, string filePath, string Name, DateTime saleDate);
        void PrintOrderInvoiceDirectly(DataGridView cart, string supplierName, DateTime Date);
        void DrawPurchaseInvoice(PrintPageEventArgs e, DataGridView cart, string supplierName, DateTime Date);

    }
}
