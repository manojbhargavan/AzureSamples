namespace InventoryProcessor
{
    public class InventoryDto
    {
        public string Id { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public double UnitPrice { get; set; }
        public string ExpiryDate { get; set; }
    }
}