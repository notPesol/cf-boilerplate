using System.Text.Json;
using ProductApi.Dtos;

namespace ProductApi.Middlewares;
public class ErrorCommonHandlerMiddleware : IMiddleware
{

  public async Task InvokeAsync(HttpContext context, RequestDelegate next)
  {
    try
    {
      await next(context);
    }
    catch (Exception error)
    {
      Console.WriteLine("An unexpected error occurred: {0}", error.Message);

      context.Response.StatusCode = StatusCodes.Status500InternalServerError;
      context.Response.ContentType = "application/json";

      // Return a JSON response with error details
      var response = new ErrorResponseDTO { StatusCode = StatusCodes.Status500InternalServerError, Message = error.Message };
      await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
  }
}

public static class ErrorHandlingMiddlewareExtensions
{
  public static IApplicationBuilder UseErrorHandlingMiddleware(this IApplicationBuilder builder)
  {
    return builder.UseMiddleware<ErrorCommonHandlerMiddleware>();
  }
}