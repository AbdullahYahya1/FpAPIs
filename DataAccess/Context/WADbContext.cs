using Microsoft.EntityFrameworkCore;
using DataAccess.Models;
using System.Reflection.Emit;

namespace DataAccess.Context
{
    public class FPDbContext : DbContext
    {
        public DbSet<AppUser> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Material> Materials { get; set; }
        public DbSet<Style> Styles { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<UserPurchaseTransaction> UserPurchaseTransactions { get; set; }
        public DbSet<UserAddress> UserAddresses { get; set; }
        public DbSet<ServiceRequest> ServiceRequests { get; set; }
        public DbSet<ProductImage> Images { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<WishlistItem> WishlistItems { get; set; }


        public FPDbContext(DbContextOptions<FPDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configure relationships and constraints

             builder.Entity<Product>()
            .HasMany(p => p.Images)     
            .WithOne(i => i.Product)    
            .HasForeignKey(i => i.ProductId)  
            .OnDelete(DeleteBehavior.Cascade);

            // Category - Product relationship
            builder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            // Material - Product relationship
            builder.Entity<Product>()
                .HasOne(p => p.Material)
                .WithMany(m => m.Products)
                .HasForeignKey(p => p.MaterialId)
                .OnDelete(DeleteBehavior.Restrict);

            // Style - Product relationship
            builder.Entity<Product>()
                .HasOne(p => p.Style)
                .WithMany(s => s.Products)
                .HasForeignKey(p => p.StyleId)
                .OnDelete(DeleteBehavior.Restrict);

            // Brand - Product relationship
            builder.Entity<Product>()
                .HasOne(p => p.Brand)
                .WithMany(b => b.Products)
                .HasForeignKey(p => p.BrandId)
                .OnDelete(DeleteBehavior.Restrict);

            // Order - OrderItem relationship
            builder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            // Product - OrderItem relationship
            builder.Entity<OrderItem>()
                .HasOne(oi => oi.Product)
                .WithMany()
                .HasForeignKey(oi => oi.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            // AppUser - UserPurchaseTransaction relationship
            builder.Entity<UserPurchaseTransaction>()
                .HasOne(t => t.CreatedBy)
                .WithMany()
                .HasForeignKey(t => t.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);

            // AppUser - UserAddress relationship
            builder.Entity<UserAddress>()
                .HasOne(ua => ua.User)
                .WithMany(u => u.Addresses)
                .HasForeignKey(ua => ua.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // AppUser - ServiceRequest relationship
            builder.Entity<ServiceRequest>()
                .HasOne(sr => sr.CreatedBy)
                .WithMany(u => u.ServiceRequests)
                .HasForeignKey(sr => sr.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);

            // Order - User relationship
            builder.Entity<Order>()
                .HasOne(o => o.Customer)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            // UserPurchaseTransaction - Order relationship
            builder.Entity<Order>()
                .HasOne(o => o.Transaction)
                .WithMany()
                .HasForeignKey(o => o.TransactionId)
                .OnDelete(DeleteBehavior.Restrict);

            // UserAddress - Order relationship
            builder.Entity<Order>()
                .HasOne(o => o.ShippingAddress)
                .WithMany()
                .HasForeignKey(o => o.ShippingAddressId)
                .OnDelete(DeleteBehavior.Restrict);



            // AppUser - CartItem relationship
            builder.Entity<CartItem>()
                .HasOne(ci => ci.User)
                .WithMany(u => u.CartItems)
                .HasForeignKey(ci => ci.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Product - CartItem relationship
            builder.Entity<CartItem>()
                .HasOne(ci => ci.Product)
                .WithMany(p => p.CartItems)
                .HasForeignKey(ci => ci.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            // Add a unique constraint to prevent duplicates
            builder.Entity<CartItem>()
                .HasIndex(ci => new { ci.UserId, ci.ProductId })
                .IsUnique();

            // AppUser - WishlistItem relationship
            builder.Entity<WishlistItem>()
                .HasOne(ci => ci.User)
                .WithMany(u => u.WishlistItems)
                .HasForeignKey(ci => ci.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Product - WishlistItem relationship
            builder.Entity<WishlistItem>()
                .HasOne(ci => ci.Product)
                .WithMany(p => p.WishlistItems)
                .HasForeignKey(ci => ci.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            // Add a unique constraint to prevent duplicates
            builder.Entity<WishlistItem>()
                .HasIndex(ci => new { ci.UserId, ci.ProductId })
                .IsUnique();
        }
    }
}
