using System.Globalization;
using System.IO;
using System.Linq.Expressions;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using CsvHelper;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace OrderDataProcessor
{
    public class OrderRawDataProcessor
    {
        private readonly ILogger<OrderRawDataProcessor> _logger;

        public OrderRawDataProcessor(ILogger<OrderRawDataProcessor> logger)
        {
            _logger = logger;
        }

        [Function(nameof(OrderRawDataProcessor))]
        public async Task Run([BlobTrigger("ordersraw/{name}", Connection = "OrdersStorage")] Stream stream, string name)
        {
            _logger.LogInformation($"C# Blob trigger function Processed blob\n Name: {name}");
            try
            {
                using (var reader = new StreamReader(stream))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    var records = csv.GetRecords<OrdersDto>();
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
                            ShippingAddress = order.ShippingAddress
                            //TotalPrice = order.Quantity * order.Price
                        };

                        // Order Ser Format
                        var orderJson = JsonConvert.SerializeObject(curOrder);
                        _logger.LogInformation($"{curOrder}");
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
