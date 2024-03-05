
using Microsoft.AspNetCore.Mvc;
using ProductApi.Models;
using ProductApi.Services;
using ProductApi.Dtos;

namespace ProductApi.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class CategoryController : ControllerBase
  {
    private readonly CategoryService _service;

    public CategoryController(CategoryService service)
    {
      _service = service;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Category>> GetByIdAsync(long id)
    {
      var result = await _service.GetByIdAsync(id);

      if (result == null)
      {
        return NotFound();
      }

      return Ok(result);
    }

    [HttpGet]
    public async Task<ActionResult<QueryResult<Category>>> GetAllAsync([FromQuery] BaseSearchDTO searchDTO)
    {
      return Ok(await _service.GetAllAsync(searchDTO));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsync(long id, Category product)
    {
      int count = await _service.UpdateAsync(id, product);
      if (count > 0)
      {
        return NoContent();
      }

      return BadRequest();

    }

    [HttpPost]
    public async Task<ActionResult<Category>> CreateAsync(Category product)
    {
      var result = await _service.CreateAsync(product);
      return await GetByIdAsync(result.Id);
    }

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
}
