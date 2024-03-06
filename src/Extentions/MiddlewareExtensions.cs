using ProductApi.Middlewares;
using ProductApi.Services;

namespace ProductApi;
public static class MiddlewareExtensions
{
  public static void MiddlewareRegister(this IApplicationBuilder app)
  {
    app.UseSecurityMiddleware();
    app.UseErrorHandlingMiddleware();
    app.UseStatusExceptionMiddleware();
  }
}