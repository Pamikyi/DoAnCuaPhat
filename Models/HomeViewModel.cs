namespace SneakerShop.Models
{
    public class HomeViewModel
    {
        public List<Shoe> AllShoes { get; set; } = new List<Shoe>();
        public List<Shoe> Running { get; set; } = new List<Shoe>();
        public List<Shoe> Training { get; set; } = new List<Shoe>();
        public List<Shoe> Lifestyle { get; set; } = new List<Shoe>();
    }
}
