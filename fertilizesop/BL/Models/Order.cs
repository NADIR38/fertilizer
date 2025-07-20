using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fertilizesop.BL.Models
{
   
        public class Order
        {
            
            public int SupplierId { get; set; }
            public DateTime OrderDate { get; set; }

            public Order(int supplierId,DateTime date)
            {
            SupplierId=supplierId;
            OrderDate=date;
            }

        }

    public class OrderDetail
        {
            
            public int OrderId { get; set; }
            public int ProductId { get; set; }
            public int Quantity { get; set; }

             public OrderDetail(int orderId, int productId, int quantity)
             {
            OrderId = orderId;
            ProductId = productId;
            Quantity = quantity;
            }
    }
}

