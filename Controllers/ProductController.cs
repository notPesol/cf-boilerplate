
using Microsoft.AspNetCore.Mvc;
using ProductApi.Models;
using ProductApi.Services;
using ProductApi.Dtos;
using Microsoft.AspNetCore.Authorization;

namespace ProductApi.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
  private readonly ProductService _service;

  public ProductController(ProductService service)
  {
    _service = service;
  }
  // GET: api/Product/5
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

  // GET: api/Product
  [HttpGet]
  public async Task<ActionResult<QueryResult<Product>>> GetAllAsync([FromQuery] ProductSearchDTO searchDTO)
  {
    var token = Request.Headers["Authorization"];
    Console.WriteLine($"token === {token}");
    return Ok(await _service.GetAllAsync(searchDTO));
  }

  // PUT: api/Product/5
  // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
  [HttpPut("{id}")]
  public async Task<IActionResult> UpdateAsync(long id, Product product)
  {
    int count = await _service.UpdateAsync(id, product);
    if (count > 0)
    {
      return NoContent();
    }

    return BadRequest();

  }

  // POST: api/Product
  // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
  [HttpPost]
  public async Task<ActionResult<Product>> CreateAsync(Product product)
  {
    var result = await _service.CreateAsync(product);
    return await GetByIdAsync(result.Id);
  }

  // DELETE: api/Product/5
  [HttpDelete("{id}")]
  public async Task<IActionResult> DeleteAsync(long id)
  {
    var count = await _service.DeleteAsync(id);

    if (count == 0)
    {
      return NotFound();
    }

    return NoContent();
  }
}

