using ProductApi.Services;

namespace ProductApi;
public static class ServiceExtensions {
  public static void Register (this IServiceCollection services) {
    services.AddScoped<ProductService>();
    services.AddScoped<CategoryService>();
  }
}