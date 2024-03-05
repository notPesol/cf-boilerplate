using System.Text.Json;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            // var token = context.Request.Headers.Authorization;
            // Console.WriteLine($"token1 === {token}");
            await _next(context);
        }
        catch (Exception error)
        {
            Console.WriteLine("An unexpected error occurred: {0}", error.Message);

            context.Response.StatusCode = 500;
            context.Response.ContentType  = "application/json";

            // Return a JSON response with error details
            var errorResponse = new { error = "An unexpected error occurred", details = error.Message };
            var jsonResponse = JsonSerializer.Serialize(errorResponse);
            await context.Response.WriteAsync(jsonResponse);
            return; // Return after writing the response to avoid further processing
        }
    }
}

public static class ErrorHandlingMiddlewareExtensions
{
    public static IApplicationBuilder UseErrorHandlingMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ErrorHandlingMiddleware>();
    }
}
