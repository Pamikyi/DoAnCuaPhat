using System;
using System.Collections.Generic;

namespace SneakerShop.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Slug { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation
        public ICollection<Shoe>? Shoes { get; set; }
    }
}
