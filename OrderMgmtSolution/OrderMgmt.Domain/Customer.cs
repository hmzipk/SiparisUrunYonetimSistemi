using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderMgmt.Domain
{
    public class Customer
    {
            public int CustomerId { get; set; }          // Primary Key
            public string CustomerCode { get; set; }     
            public string CustomerName { get; set; }     
            public string Email { get; set; }            
            public DateTime CreatedAt { get; set; }      

            public bool IsActive { get; set; }           // Müşteri aktif mi?   

    }
}
