namespace ProductApi.Models;

public class Category : BaseEntity
{
  public string Name { get; set; }
  public string Desctiption { get; set; }
  public IList<Product> Products { get; set; }

  public Category()
  {
    Products = [];
  }
}