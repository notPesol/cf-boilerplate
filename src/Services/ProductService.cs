using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProductApi.Dtos;
using ProductApi.Models;

namespace ProductApi.Services;

public class ProductService : BaseService<Product, DBContext, ProductSearchDTO>
{

  public ProductService(DBContext context) : base(context, context.Set<Product>())
  {
  }

  public override Task<QueryResult<Product>> GetAllAsync(ProductSearchDTO searchDTO)
  {
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