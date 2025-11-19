using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderMgmt.Domain
{
    public class Order
    {
        public int OrderId { get; set; }
        public string OrderNo { get; set; } = string.Empty;

        public int CustomerId { get; set; }
        public int ProductId { get; set; }

        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }   // Birim fiyat
        public decimal TotalAmount { get; set; } // Toplam tutar

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public bool DeliveryConfirmed { get; set; } = false;
        public string? DeliveryConfirmToken { get; set; }

        /* Navigation property'ler (- Navigation property’ler, ilişkili nesneleri (Customer, Product) 
           siparişin içine bağlamak için var) (EF Kullanılmadığı için elle girildi.)
         */
        public Customer? Customer { get; set; }
        public Product? Product { get; set; }
    }
}
