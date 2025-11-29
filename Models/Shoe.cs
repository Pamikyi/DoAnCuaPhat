using System;
using System.Collections.Generic;

namespace SneakerShop.Models
{
    public class Shoe
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Slug { get; set; }

        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        public decimal Price { get; set; }
        public string? Description { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        // Navigation
        public ICollection<ShoeImage>? ShoeImages { get; set; }
        public ICollection<ShoeSize>? ShoeSizes { get; set; }
        public ICollection<OrderItem>? OrderItems { get; set; }
    }
}
