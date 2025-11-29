using Microsoft.EntityFrameworkCore;
using SneakerShop.Models;

namespace SneakerShop.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // DbSet
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Shoe> Shoes { get; set; }
        public DbSet<ShoeSize> ShoeSizes { get; set; }
        public DbSet<ShoeImage> ShoeImages { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // =======================
            // USERS
            // =======================
            modelBuilder.Entity<User>()
                .HasKey(u => u.Id);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // =======================
            // CATEGORY
            // =======================
            modelBuilder.Entity<Category>()
                .HasKey(c => c.Id);

            modelBuilder.Entity<Category>()
                .Property(c => c.Name)
                .HasMaxLength(100);

            // 1 Category -> many Shoes
            modelBuilder.Entity<Category>()
                .HasMany(c => c.Shoes)
                .WithOne(s => s.Category!)
                .HasForeignKey(s => s.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            // =======================
            // SHOES
            // =======================
            modelBuilder.Entity<Shoe>()
                .HasKey(s => s.Id);

            modelBuilder.Entity<Shoe>()
                .Property(s => s.Name)
                .HasMaxLength(150);

            modelBuilder.Entity<Shoe>()
                .Property(s => s.Price)
                .HasColumnType("decimal(18,2)");

            // =======================
            // SHOE IMAGES
            // =======================
            modelBuilder.Entity<ShoeImage>()
                .HasKey(i => i.Id);

            // 1 Shoe -> many Images
            modelBuilder.Entity<ShoeImage>()
                .HasOne(i => i.Shoe)
                .WithMany(s => s.ShoeImages)
                .HasForeignKey(i => i.ShoeId)
                .OnDelete(DeleteBehavior.Cascade);

            // =======================
            // SHOE SIZES
            // =======================
            modelBuilder.Entity<ShoeSize>()
                .HasKey(ss => ss.Id);

            // 1 Shoe -> many Sizes
            modelBuilder.Entity<ShoeSize>()
                .HasOne(ss => ss.Shoe)
                .WithMany(s => s.ShoeSizes)
                .HasForeignKey(ss => ss.ShoeId)
                .OnDelete(DeleteBehavior.Cascade);

            // =======================
            // CART
            // =======================
            modelBuilder.Entity<Cart>()
                .HasKey(c => c.Id);

            modelBuilder.Entity<Cart>()
                .HasOne(c => c.User)
                .WithMany(u => u.Carts)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // =======================
            // CART ITEMS
            // =======================
            // modelBuilder.Entity<CartItem>()
            //     .HasKey(ci => ci.Id);

            // modelBuilder.Entity<CartItem>()
            //     .HasOne(ci => ci.Cart)
            //     .WithMany(c => c.CartItems)
            //     .HasForeignKey(ci => ci.CartId)
            //     .OnDelete(DeleteBehavior.Cascade);

            // modelBuilder.Entity<CartItem>()
            //     .HasOne(ci => ci.Shoe)
            //     .WithMany()
            //     .HasForeignKey(ci => ci.ShoeId);

            // =======================
            // ORDERS
            // =======================
            modelBuilder.Entity<Order>()
                .HasKey(o => o.Id);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Order>()
                .Property(o => o.TotalAmount)
                .HasColumnType("decimal(18,2)");

            // =======================
            // ORDER ITEMS
            // =======================
            modelBuilder.Entity<OrderItem>()
                .HasKey(oi => oi.Id);

            modelBuilder.Entity<OrderItem>()
                .Property(oi => oi.UnitPrice)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Shoe)
                .WithMany(s => s.OrderItems)
                .HasForeignKey(oi => oi.ShoeId);
        }
    }
}
