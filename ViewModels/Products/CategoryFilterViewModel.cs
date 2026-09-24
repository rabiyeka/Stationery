using System;

namespace Stationery.ViewModels.Products;

public class CategoryFilterViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } =null!;
    public int ProductCount { get; set; }
}
