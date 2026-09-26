using System;

namespace Stationery.ViewModels.Products;

public class ProductsIndexViewModel
{
    public IList<ProductListViewModel> Products { get; set; } = [];
    public IList<CategoryFilterViewModel> Categories { get; set; } = [];
    public IList<BrandFilterViewModel> Brands { get; set; } = [];
    public int? SelectedCategoryId { get; set; }

}
