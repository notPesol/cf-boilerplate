using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using ProductApi.Middlewares;
using ProductApi.Models;

namespace ProductApi.Services;
public class AuthenticationService
{
  private readonly IConfiguration _configuration;

  public AuthenticationService(IConfiguration configuration)
  {
    _configuration = configuration;
  }

  public string Login(User user)
  {
    if (user.Username == "user_1" && user.Password == "user_1_pass") // Sample validation
    {
      return CreateToken(user);
    }

    throw new StatusException(StatusCodes.Status401Unauthorized, "Invalid username or password");
  }

  public string CreateToken(User user)
  {
    try
    {
      var tokenHandler = new JwtSecurityTokenHandler();
      var secToken = new JwtSecurityToken(_configuration["Jwt:Issuer"],
        _configuration["Jwt:Audience"],
        [
          new Claim("Id", "1"), // Example claim
          new Claim(JwtRegisteredClaimNames.Sub, user.Username),
          new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        ],
        expires: DateTime.UtcNow.AddDays(1),
        signingCredentials: new SigningCredentials(
          new SymmetricSecurityKey(
            Encoding.ASCII.GetBytes(_configuration["Jwt:Key"])),
            SecurityAlgorithms.HmacSha512Signature
          )
      );
      return tokenHandler.WriteToken(secToken);
    }
    catch (SecurityTokenEncryptionFailedException ex)
    {
      throw new StatusException(StatusCodes.Status401Unauthorized, ex.Message);
    }
  }
}

