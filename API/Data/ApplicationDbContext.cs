using API.Models;
using API.Models.Cart;
using API.Models.Coupon;
using API.Models.Order;
using API.Models.ProductModel;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {

    }

    public DbSet<Address> Addresses { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductCategory> ProductCategories { get; set; }
    public DbSet<ProductSubCategory> ProductSubCategories { get; set; }
    public DbSet<ProductImage> ProductImages { get; set; }
    public DbSet<ProductSpecification> ProductSpecifications { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<Feedback> Feedbacks { get; set; }
    public DbSet<Coupon> Coupons { get; set; }
    public DbSet<CartHeader> CartHeaders { get; set; }
    public DbSet<CartDetail> CartDetails { get; set; }
    public DbSet<OrderHeader> OrderHeaders { get; set; }
    public DbSet<OrderDetail> OrderDetails { get; set; }
    public DbSet<Color> Colors { get; set; }
    public DbSet<ProductColor> ProductColors { get; set; }



    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<ProductCategory>()
            .HasIndex(p => p.CategoryName)
            .IsUnique();

        builder.Entity<ProductSubCategory>()
            .HasIndex(p => p.SubCategoryName)
            .IsUnique();

        builder.Entity<ProductColor>()
            .HasKey(pk => new { pk.ProductId, pk.ColorId });

        builder.Entity<ProductColor>()
            .HasOne(pc => pc.Color)
            .WithMany(pc => pc.Colors)
            .HasForeignKey(pc => pc.ColorId);

        builder.Entity<ProductColor>()
            .HasOne(pc => pc.Product)
            .WithMany(pc => pc.Colors)
            .HasForeignKey(pc => pc.ProductId);
        
        builder.Entity<Review>()
            .Property(r => r.Comment).IsRequired();
            
    }
}
