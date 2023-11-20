using System;
using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using CsvHelper;
using System.Globalization;
using System.Linq;

namespace OrderDataProcessor
{
    public class RawOrderProcessor
    {
        [FunctionName("RawOrderProcessor")]
        public void Run([BlobTrigger("ordersraw/{name}", Connection = "OrdersStorage")] Stream myBlob, string name, ILogger log, Binder binder)
        {
            log.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");

            using (var reader = new StreamReader(myBlob))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var orders = csv.GetRecords<OrderDto>().ToList();
                foreach (var order in orders)
                {
                    var mappedOrder = new Order()
                    {
                        CustomerName = order.CustomerName,
                        OrderDate = order.OrderDate,
                        OrderID = order.OrderID,
                        OrderTotal = order.Price * order.Quantity,
                        Price = order.Price,
                        ProductID = order.ProductID,
                        ProductName = order.ProductName,
                        Quantity = order.Quantity,
                        ShippingAddress = order.ShippingAddress
                    };
                    string orderStr = JsonConvert.SerializeObject(mappedOrder);
                    log.LogInformation($"{orderStr}");

                    var blobAttribute = new BlobAttribute($"orders/{order.OrderID}.json");
                    var storageAccountAttribute = new StorageAccountAttribute("OrdersStorage");

                    using (var writer = binder.BindAsync<TextWriter>(new Attribute[] { blobAttribute, storageAccountAttribute }).Result)
                    {
                        writer.Write(orderStr);
                    }
                }
            }

        }
    }
}
