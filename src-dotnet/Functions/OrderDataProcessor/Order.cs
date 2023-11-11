using Newtonsoft.Json;

namespace OrderDataProcessor;
public class Order
{
    public string OrderID { get; set; }
    public string CustomerName { get; set; }
    public string ProductID { get; set; }
    public string ProductName { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public string OrderDate { get; set; }
    public string ShippingAddress { get; set; }
    public decimal TotalPrice { get; set; }

    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }
}
