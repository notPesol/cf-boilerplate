using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProductApi.Dtos;
using ProductApi.Models;

namespace ProductApi.Services;

public class ProductAssociationService : BaseService<Product, DBContext, ProductSearchDTO>
{

  public ProductAssociationService(DBContext context) : base(context, context.Set<Product>())
  {
  }

  public override async Task<Product?> GetByIdAsync(long id)
  {
    return await _entity.Include(p => p.Category).Where(p => p.Id == id).FirstAsync();
  }

  public override Task<QueryResult<Product>> GetAllAsync(ProductSearchDTO searchDTO)
  {
    _query = _query.Include(p => p.Category);

    if (!searchDTO.Query.Trim().IsNullOrEmpty())
    {
      _query = _query.Where(p => EF.Functions.Like(p.Name, $"%{searchDTO.Query}%"));
    }

    return base.GetAllAsync(searchDTO);
  }

  public override Dictionary<string, Expression<Func<Product, object>>> GetOrderMappings()
  {
    var mappings = new Dictionary<string, Expression<Func<Product, object>>>
      {
        { "name", p => p.Name.ToLower() },
        { "price", p => p.Price }
      };

    return mappings;
  }
}