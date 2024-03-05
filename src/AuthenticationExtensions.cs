using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace ProductApi;
public static class AuthenticationExtensions
{
  public static void AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
  {
    services.AddAuthentication(options =>
      {
        options.RequireAuthenticatedSignIn = true;
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
      }).AddJwtBearer(options =>
      {
        options.TokenValidationParameters = new TokenValidationParameters()
        {
          ValidIssuer = configuration["JWT:Issuer"],
          ValidAudience = configuration["JWT:Audience"],
          IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"])),
          ValidateIssuer = true,
          ValidateAudience = true,
          ValidateLifetime = false,
          ValidateIssuerSigningKey = true
        };
      });
  }
}