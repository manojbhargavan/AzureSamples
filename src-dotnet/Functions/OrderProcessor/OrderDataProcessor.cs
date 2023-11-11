using System;
using System.Formats.Asn1;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace OrderProcessor
{
    public class OrderDataProcessor
    {
        [FunctionName("OrderDataProcessor")]
        public void Run([BlobTrigger("ordersraw/{name}", Connection = "OrdersDataStorage")]Stream myBlob, string name, ILogger _logger)
        {
            _logger.LogInformation($"C# Blob trigger function Processed blob\n Name: {name}");
            try
            {
                using (var reader = new StreamReader(myBlob))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    var records = csv.GetRecords<OrdersDto>()?.ToList();
                    _logger.LogInformation($"Record Count: {records.Count()}");
                    foreach (var order in records)
                    {
                        _logger.LogInformation($"Order: {order}");
                        // Calculate Total Price of the Order Line Item
                        Order curOrder = new Order()
                        {
                            OrderID = order.OrderID,
                            CustomerName = order.CustomerName,
                            ProductID = order.ProductID,
                            ProductName = order.ProductName,
                            Quantity = order.Quantity,
                            Price = order.Price,
                            OrderDate = order.OrderDate,
                            ShippingAddress = order.ShippingAddress,
                            TotalPrice = order.Quantity * order.Price
                        };

                        // Order Ser Format
                        var orderJson = JsonConvert.SerializeObject(curOrder);
                        _logger.LogInformation($"{orderJson}");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }
    }
}
