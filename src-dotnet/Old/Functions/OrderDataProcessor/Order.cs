using System;
using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace OrderDataProcessor
{
    public class Order
    {
        public string OrderID { get; set; }
        public string CustomerName { get; set; }
        public string ProductID { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public string OrderDate { get; set; }
        public string ShippingAddress { get; set; }
        public double OrderTotal { get; set; }
    }
}