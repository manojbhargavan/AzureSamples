using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderProcessor
{
    internal class OrdersDto
    {
        public string OrderID { get; set; }
        public string CustomerName { get; set; }
        public string ProductID { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public string OrderDate { get; set; }
        public string ShippingAddress { get; set; }
    }
}
