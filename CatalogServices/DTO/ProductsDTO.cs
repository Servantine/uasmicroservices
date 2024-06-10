﻿namespace CatalogServices;

public class ProductsDTO
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public int CategoryID { get; set; }
}
