using System.Text.Json;

namespace ProductApi.Models;

public abstract class BaseEntity
{
  public long Id { get; set; }
}