namespace CatalogServices.Models
{
    public class Join
    {
    public int ProductID { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public int CategoryID { get; set; }
    public string? CategoryName { get; set; }
    }
}


