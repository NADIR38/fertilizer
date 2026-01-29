using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fertilizesop.BL.Models
{
    public class SupplierPaymentSummary
    {
        public int SupplierId { get; set; }
        public string SupplierName { get; set; }
        public string SupplierPhone { get; set; }
        public decimal TotalPending { get; set; }
        public decimal TotalPaid { get; set; }
        public int PendingBillCount { get; set; }

        public SupplierPaymentSummary()
        {
        }

        public SupplierPaymentSummary(int supplierId, string supplierName, string supplierPhone, decimal totalPending, decimal totalPaid, int pendingBillCount)
        {
            SupplierId = supplierId;
            SupplierName = supplierName;
            SupplierPhone = supplierPhone;
            TotalPending = totalPending;
            TotalPaid = totalPaid;
            PendingBillCount = pendingBillCount;
        }
    }
}
