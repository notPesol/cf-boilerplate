namespace ProductApi.Models;

public class Product : BaseEntity
{
  public string Name { get; set; }
  public decimal Price { get; set; }
  public long CategoryId { get; set; }
  public Category Category { get; set; }
}
