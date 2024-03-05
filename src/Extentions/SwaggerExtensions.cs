using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ProductApi;
public static class SwaggerExtensions
{
  public static void ConfigureSwaggerForJwt(this SwaggerGenOptions options)
  {
    options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
    {
      In = ParameterLocation.Header,
      Description = "Please enter a valid token",
      Name = "Authorization",
      Type = SecuritySchemeType.Http,
      BearerFormat = "JWT",
      Scheme = JwtBearerDefaults.AuthenticationScheme
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
      {
        {
          new OpenApiSecurityScheme {
            Reference = new OpenApiReference {
              Type = ReferenceType.SecurityScheme,
              Id =  JwtBearerDefaults.AuthenticationScheme
            }
          },
          Array.Empty<string>()
        }
      });
  }
}