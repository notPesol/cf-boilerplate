
using Microsoft.AspNetCore.Mvc;
using ProductApi.Models;
using ProductApi.Dtos;
using ProductApi.Services;

namespace ProductApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthenticationController : ControllerBase
{
  private readonly AuthenticationService _service;

  public AuthenticationController(AuthenticationService service)
  {
    _service = service;
  }
  [HttpPost]
  public ActionResult<JwtResponseDTO> Login(User user)
  {
    var response = new JwtResponseDTO
    {
      AccessToken = _service.Login(user)
    };

    return Ok(response);
  }
}

