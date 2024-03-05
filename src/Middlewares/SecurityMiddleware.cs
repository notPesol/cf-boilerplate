using System.Text.Json;
using ProductApi.Models;

public class SecurityMiddleware
{
    private readonly RequestDelegate _next;

    public SecurityMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            var token = context.Request.Headers.Authorization;
        }
        catch (Exception error)
        {
           Console.WriteLine("An unexpected error occurred: {0}", error.Message);
        //    throw new Exception("Invalid3");
        //    throw 
        }
    }
    
}

public static class SecurityMiddlewareExtensions
{
    public static IApplicationBuilder UseSecurityMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<SecurityMiddleware>();
    }
}
