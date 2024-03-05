using System.Text.Json;

namespace ProductApi.Models;

public abstract class BaseEntity
{
  public long Id { get; set; }

  // public override string ToString()
  // {
  //   return JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
  // }
}