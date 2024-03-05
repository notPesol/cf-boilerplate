using Microsoft.EntityFrameworkCore;

namespace ProductApi.Models;

public class DBContext : DbContext
{
  public DbSet<Product> Products { get; set; } = null!;
  public DbSet<Category> Categories { get; set; } = null!;
  public DBContext(DbContextOptions<DBContext> options)
      : base(options)
  {
  }

  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {
    base.OnConfiguring(optionsBuilder);
    Console.WriteLine("OnConfiguring");
  }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);
    Console.WriteLine("OnModelCreating");
    modelBuilder.Entity<Product>().HasOne<Category>(p => p.Category).WithMany(c => c.Products).HasForeignKey(p => p.CategoryId);
  }
}