using Microsoft.EntityFrameworkCore;
using ProductApi.Models;

public static class DatabaseExtensions
{
  public static void AddDatabase(this IServiceCollection services, string connectionString)
  {
    services.AddDbContext<DBContext>(options =>
      options.UseSqlite(connectionString));
  }
}