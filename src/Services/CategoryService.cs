using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProductApi.Dtos;
using ProductApi.Models;

namespace ProductApi.Services;

public class CategoryService : BaseService<Category, DBContext, BaseSearchDTO>
{

  public CategoryService(DBContext context) : base(context, context.Categories)
  {

  }

  public override Task<QueryResult<Category>> GetAllAsync(BaseSearchDTO searchDTO)
  {

    if (!searchDTO.Query.Trim().IsNullOrEmpty())
    {
      _query = _query.Where(c => EF.Functions.Like(c.Name, $"%{searchDTO.Query}%"));
    }

    return base.GetAllAsync(searchDTO);
  }

  public override Dictionary<string, Expression<Func<Category, object>>> GetOrderMappings()
  {
    var mappings = new Dictionary<string, Expression<Func<Category, object>>>
      {
        { "name", p => p.Name.ToLower() },
      };

    return mappings;
  }
}