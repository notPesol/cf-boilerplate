using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace ProductApi.Middlewares;
public class SecurityMiddleware : IMiddleware
{
  private readonly IConfiguration _configuration;

  public SecurityMiddleware(IConfiguration configuration)
  {
    _configuration = configuration;
  }

  public async Task InvokeAsync(HttpContext context, RequestDelegate next)
  {

    var token = context.Request.Headers.Authorization.FirstOrDefault()?.Split(" ").Last();

    if (token != null)
    {
      try
      {
        context.Items["User"] = ValidateToken(token);
        // Can access user claims later in controllers
        // Example in controllers ...
        // var userPrincipal = HttpContext.Items["User"] as ClaimsPrincipal;
        // if (userPrincipal != null)
        // {
        //   // Now you can access user claims
        //   var userId = userPrincipal.FindFirst("Id")?.Value;
        //   var userName = userPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        // }
        await next(context);
        return;
      }
      catch (SecurityTokenException)
      {
        throw new StatusException(StatusCodes.Status500InternalServerError, "Invalid token");
      }
    }

    await next(context);
  }

  public ClaimsPrincipal ValidateToken(string token)
  {
    var tokenHandler = new JwtSecurityTokenHandler();
    var tokenValidationParameters = new TokenValidationParameters
    {
      ValidIssuer = _configuration["JWT:Issuer"],
      ValidAudience = _configuration["JWT:Audience"],
      IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["Jwt:Key"])),
      ValidateIssuerSigningKey = true,
      ValidateIssuer = true,
      ValidateAudience = true,
    };
    return tokenHandler.ValidateToken(token, tokenValidationParameters, out var validatedToken);
  }
}

public static class SecurityMiddlewareExtensions
{
  public static IApplicationBuilder UseSecurityMiddleware(this IApplicationBuilder builder)
  {
    return builder.UseMiddleware<SecurityMiddleware>();
  }
}
