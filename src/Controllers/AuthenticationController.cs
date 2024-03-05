
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ProductApi.Models;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.IdentityModel.JsonWebTokens;
using System.IdentityModel.Tokens.Jwt;
using ProductApi.Dtos;

namespace ProductApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthenticationController : ControllerBase
{
  private readonly IConfiguration _configuration;

  public AuthenticationController(IConfiguration configuration)
  {
    _configuration = configuration;
  }
  [HttpPost]
  public ActionResult<JwtResponseDTO> CreteToken(User user)
  {
    if (user.Username == "user_1" && user.Password == "user_1_pass")
    {
      var issuer = _configuration["Jwt:Issuer"];
      var audience = _configuration["Jwt:Audience"];
      var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);

      // var tokenDescriptor = new SecurityTokenDescriptor
      // {
      //   Subject = new ClaimsIdentity(
      //     new[] {
      //       new Claim("Id", 1.ToString()),
      //       new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub, user.Username),
      //       // new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Email, user.Username),
      //       new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Jti,
      //       Guid.NewGuid().ToString())
      //     }
      //   ),
      //   Expires = DateTime.UtcNow.AddDays(1),
      //   Issuer = issuer,
      //   Audience = audience,
      //   SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
      // };
      var tokenHandler = new JwtSecurityTokenHandler();
      // var token = tokenHandler.CreateToken(tokenDescriptor);
      // var stringToken = tokenHandler.WriteToken(token);
      var secToken = new JwtSecurityToken(issuer,
        audience,
        [
          new Claim("Id", 1.ToString()),
          new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub, user.Username),
          // new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Email, user.Username),
          new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Jti,
          Guid.NewGuid().ToString())
        ],
        expires: DateTime.UtcNow.AddDays(1),
        signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
      );
      var token = tokenHandler.WriteToken(secToken);
      return Ok(new JwtResponseDTO { UserId = 1.ToString(), AccessToken = token });
      // return Ok(new JwtResponseDTO { Id = 1.ToString(), AccessToken = stringToken });
    }

    return Unauthorized();
  }
}

