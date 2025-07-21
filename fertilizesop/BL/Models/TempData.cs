using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fertilizesop.BL.Models
{
    public class TempInvoiceData
    {
        public List<InvoiceItem> Items { get; set; }
    }

    public class InvoiceItem
    {
        public string ProductName { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }
        public int Total { get; set; }

    }
}
