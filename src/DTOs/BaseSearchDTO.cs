using System.Text.Json;

namespace ProductApi.Dtos;

public class BaseSearchDTO
{
  public string Query { get; set; } = "";
  public int Page { get; set; } = 1;
  public int Limit { get; set; } = 20;
  public bool IgnorePage { get; set; } = false;
  public string OrderBy { get; set; } = "";
  public string OrderType { get; set; } = "asc";

  public int Offset => (Page - 1) * Limit;

  public override string ToString()
  {
    return JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
  }
}