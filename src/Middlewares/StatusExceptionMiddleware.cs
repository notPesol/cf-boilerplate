
using Newtonsoft.Json;
using ProductApi.Dtos;

namespace ProductApi.Middlewares;
public class StatusExceptionMiddleware : IMiddleware
{
  public async Task InvokeAsync(HttpContext context, RequestDelegate next)
  {
    try
    {
      await next(context);
    }
    catch (StatusException ex)
    {
      await HandleStatusExceptionAsync(context, ex);
    }
  }

  private static async Task HandleStatusExceptionAsync(HttpContext context, StatusException ex)
  {
    context.Response.ContentType = "application/json";
    context.Response.StatusCode = ex.StatusCode;

    var response = new ErrorResponseDTO
    {
      StatusCode = ex.StatusCode,
      Message = ex.Message
    };

    await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
  }
}

public class StatusException : Exception
{
  public int StatusCode { get; }

  public StatusException(int statusCode, string message) : base(message)
  {
    StatusCode = statusCode;
  }
}

public static class StatusExceptionMiddlewareExtensions
{
  public static IApplicationBuilder UseStatusExceptionMiddleware(this IApplicationBuilder builder)
  {
    return builder.UseMiddleware<StatusExceptionMiddleware>();
  }
}