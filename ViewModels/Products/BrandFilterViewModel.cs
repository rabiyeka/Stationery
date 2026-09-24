using System;

namespace Stationery.ViewModels.Products;

public class BrandFilterViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } =null!;
    public int ProductCount { get; set; }
}
