namespace SneakerShop.SessionModels
{
    public class CartItem
    {
        public int ShoeId { get; set; }
        public string? Name { get; set; }
        public string? Image { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Total => Quantity * UnitPrice;
    }
}
