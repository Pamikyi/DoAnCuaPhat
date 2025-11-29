namespace SneakerShop.Models
{
    public class ShoeImage
    {
        public int Id { get; set; }

        public int ShoeId { get; set; }
        public Shoe? Shoe { get; set; }

        public string? ImageUrl { get; set; }
        public bool IsPrimary { get; set; }
    }
}
