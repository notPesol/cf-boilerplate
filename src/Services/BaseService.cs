using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProductApi.Dtos;
using ProductApi.Models;

namespace ProductApi.Services;

public abstract class BaseService<TEntity, TContext, TSearchDTO> : ICrudService<TEntity, TSearchDTO>
  where TEntity : BaseEntity
  where TContext : DbContext
  where TSearchDTO : BaseSearchDTO
{
  protected readonly TContext _context;
  protected readonly DbSet<TEntity> _entity;
  protected IQueryable<TEntity> _query;

  public BaseService(TContext context, DbSet<TEntity> entity)
  {
    _context = context;
    _entity = entity;
    _query = _entity.AsQueryable<TEntity>();
  }

  public virtual async Task<TEntity> CreateAsync(TEntity entity)
  {
    _entity.Add(entity);
    await _context.SaveChangesAsync();
    return entity;
  }

  public virtual async Task<int> DeleteAsync(long id)
  {
    var row = await _entity.FindAsync(id);
    if (row != null)
    {
      _entity.Remove(row);
      return await _context.SaveChangesAsync();
    }

    return 0;
  }

  public virtual async Task<QueryResult<TEntity>> GetAllAsync(TSearchDTO searchDTO)
  {
    var count = await _query.CountAsync();

    _query = ApplyOrder(searchDTO);
    _query = ApplyPagination(searchDTO);

    var result = await _query.ToListAsync();

    return new QueryResult<TEntity> { Data = result, Count = count };
  }

  public virtual async Task<TEntity?> GetByIdAsync(long id)
  {
    return await _entity.FindAsync(id);
  }

  public virtual async Task<int> UpdateAsync(long id, TEntity entity)
  {
    if (id != entity.Id)
    {
      return 0;
    }

    _context.Entry(entity).State = EntityState.Modified;

    try
    {
      return await _context.SaveChangesAsync();
    }
    catch (DbUpdateConcurrencyException)
    {
      if (!EntityExists(id))
      {
        return 0;
      }
      else
      {
        throw;
      }
    }
  }

  public virtual IQueryable<TEntity> ApplyPagination(TSearchDTO searchDTO)
  {
    if (!searchDTO.IgnorePage)
    {
      _query = _query.Skip(searchDTO.Offset);
      _query = _query.Take(searchDTO.Limit);
    }
    return _query;
  }

  public virtual IQueryable<TEntity> ApplyOrder(TSearchDTO searchDTO)
  {
    bool isDesc = searchDTO.OrderType == "desc";
    var orderMappings = GetOrderMappings();

    if (!searchDTO.OrderBy.IsNullOrEmpty() && orderMappings.ContainsKey(searchDTO.OrderBy))
    {
      var orderByExpression = orderMappings[searchDTO.OrderBy];
      _query = isDesc ? _query.OrderByDescending(orderByExpression) : _query.OrderBy(orderByExpression);
    }
    else
    {
      _query = isDesc ? _query.OrderByDescending(e => e.Id) : _query.OrderBy(e => e.Id);
    }

    return _query;
  }

  public virtual Dictionary<string, Expression<Func<TEntity, object>>> GetOrderMappings()
  {
    var mappings = new Dictionary<string, Expression<Func<TEntity, object>>>();
    // Override it
    // Add mappings for each sortable property
    // mappings.Add("property1", x => x.Property1);
    // mappings.Add("property2", x => x.Property2);
    return mappings;
  }

  private bool EntityExists(long id)
  {
    return _entity.Any(e => e.Id == id);
  }
}

interface ICrudService<TEntity, TSearchDTO>
{
  Task<TEntity?> GetByIdAsync(long id);
  Task<QueryResult<TEntity>> GetAllAsync(TSearchDTO searchDTO);
  Task<TEntity> CreateAsync(TEntity entity);
  Task<int> UpdateAsync(long id, TEntity entity);
  Task<int> DeleteAsync(long id);
}

public class QueryResult<TEntity>
{
  public IEnumerable<TEntity> Data { get; set; } = null;
  public int? Count { get; set; }
}