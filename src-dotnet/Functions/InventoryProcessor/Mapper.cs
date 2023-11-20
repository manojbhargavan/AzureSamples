namespace InventoryProcessor
{
    public static class Mapper
    {
        public static Inventory Map(this InventoryDto inventoryDto)
        {
            return new Inventory()
            {
                ExpiryDate = inventoryDto.ExpiryDate,
                Id = inventoryDto.Id,
                InventoryValue = inventoryDto.UnitPrice * inventoryDto.Quantity,
                ProductName = inventoryDto.ProductName,
                Quantity = inventoryDto.Quantity,
                UnitPrice = inventoryDto.UnitPrice
            };
        }
    }
}