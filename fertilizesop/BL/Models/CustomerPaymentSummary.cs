using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fertilizesop.BL.Models
{
    public class CustomerPaymentSummary
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public decimal TotalPending { get; set; }
        public decimal TotalPaid { get; set; }
        public int PendingBillCount { get; set; }

        public CustomerPaymentSummary()
        {
        }

        public CustomerPaymentSummary(int customerId, string customerName, string customerPhone, decimal totalPending, decimal totalPaid, int pendingBillCount)
        {
            CustomerId = customerId;
            CustomerName = customerName;
            CustomerPhone = customerPhone;
            TotalPending = totalPending;
            TotalPaid = totalPaid;
            PendingBillCount = pendingBillCount;
        }
    }
}
