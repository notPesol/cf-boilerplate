using ProductApi.Middlewares;
using ProductApi.Services;

namespace ProductApi;
public static class ServiceExtensions
{
  public static void Register(this IServiceCollection services)
  {
    services.AddSingleton<SecurityMiddleware>();
    services.AddSingleton<StatusExceptionMiddleware>();
    services.AddSingleton<ErrorCommonHandlerMiddleware>();

    services.AddScoped<AuthenticationService>();
    services.AddScoped<ProductService>();
    services.AddScoped<CategoryService>();
  }
}