using System;
using System.Collections.Generic;

namespace SneakerShop.Models
{
    public class Order
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User? User { get; set; }

        public decimal TotalAmount { get; set; }
        public string? Status { get; set; } // Pending, Paid, Shipped...

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation
        public ICollection<OrderItem>? OrderItems { get; set; }
    }
}
