using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductApi.Dtos;
using ProductApi.Models;
using ProductApi.Services;

namespace ProductApi.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class ProductAssociationController : ControllerBase
{
  private readonly ProductAssociationService _service;

  public ProductAssociationController(ProductAssociationService service)
  {
    _service = service;
  }

  [HttpGet("{id}")]
  public async Task<ActionResult<Product>> GetByIdAsync(long id)
  {
    var product = await _service.GetByIdAsync(id);

    if (product == null)
    {
      return NotFound();
    }

    return Ok(product);
  }

  [HttpGet]
  public async Task<ActionResult<QueryResult<Product>>> GetAllAsync([FromQuery] ProductSearchDTO searchDTO)
  {
    return Ok(await _service.GetAllAsync(searchDTO));
  }
}