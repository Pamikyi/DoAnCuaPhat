namespace SneakerShop.Models
{
    public class ShoeSize
    {
        public int Id { get; set; }

        public int ShoeId { get; set; }
        public Shoe? Shoe { get; set; }

        public int Size { get; set; } // 38, 39, 40...
        public int Stock { get; set; }
    }
}
