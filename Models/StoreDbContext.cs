using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Models;

public class StoreDbContext : IdentityDbContext<ApplicationUser>
{
    public StoreDbContext(DbContextOptions<StoreDbContext> options) : base(options) { }

    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) 
    { 
        base.OnModelCreating(modelBuilder); 
        modelBuilder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "Electronics", Description = "Electronic devices" }, 
            new Category { Id = 2, Name = "Books", Description = "Books and literature" }
            ); 
        modelBuilder.Entity<Product>().HasData(
            new Product { Id = 1, Name = "Smartphone", Description = "Latest smartphone", Price = 699.99m, CategoryId = 1, Stock = 50, ImageUrl = "" }, 
            new Product { Id = 2, Name = "Laptop", Description = "High-performance laptop", Price = 999.99m, CategoryId = 1, Stock = 30, ImageUrl = "" }, 
            new Product { Id = 3, Name = "Novel", Description = "Bestseller novel", Price = 19.99m, CategoryId = 2, Stock = 100, ImageUrl = "" }
            ); 
    }
}
