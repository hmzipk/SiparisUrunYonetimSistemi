using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderMgmt.Domain
{
    public class Product
    {
        public int ProductId { get; set; }
        public string StockCode { get; set; } = "";
        public string StockName { get; set; } = "";
        public bool IsActive { get; set; }

        public decimal Price { get; set; }


    }
}
