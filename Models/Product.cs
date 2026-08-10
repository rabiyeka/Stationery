using System;

namespace Stationery.Models;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public string ImageUrl { get; set; } = null!;
    public int CategoryId { get; set; }
    public Category? Category { get; set; } = null!;
    public int BrandId { get; set; }
    public Brand? Brand { get; set; } = null!;
}
