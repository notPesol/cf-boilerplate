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
  }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);

    modelBuilder.Entity<Category>(entity =>
    {
      entity.HasKey(e => e.Id);
    });

    modelBuilder.Entity<Product>(entity =>
    {
      entity.HasKey(e => e.Id);
      entity.HasOne(e => e.Category)
        .WithMany(c => c.Products)
        .HasForeignKey(p => p.CategoryId);
    });
  }
}