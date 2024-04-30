using API.Models;
using API.Models.Product;
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
    public DbSet<ProductSpecification> ProductSpecifications { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<ProductSpecification>()
            .HasKey(p => new { p.ProductId, p.SpecificationId });

        builder.Entity<ProductSpecification>()
            .HasOne(p => p.Product)
            .WithMany(s => s.ProductSpecifications)
            .HasForeignKey(p => p.ProductId);

        builder.Entity<ProductSpecification>()
            .HasOne(p => p.Specification)
            .WithMany(s => s.ProductSpecifications)
            .HasForeignKey(p => p.SpecificationId);

        builder.Entity<ProductCategory>()
            .HasIndex(p => p.CategoryName)
            .IsUnique();

        builder.Entity<ProductSubCategory>()
            .HasIndex(p => p.SubCategoryName)
            .IsUnique();
            
    }
}
