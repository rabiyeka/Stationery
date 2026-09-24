using System;
using Stationery.ViewModels.Admin;
using Stationery.ViewModels.Products;

namespace Stationery.Services;

public interface IProductService
{
    Task<IList<ProductListViewModel>> GetAllProductsAsync(int? categoryId= null);
    Task<ProductDetailsViewModel?> GetProductDetailsAsync(int id);
    Task<IList<ProductListViewModel>> GetProductsForAdminAsync();
    Task<(bool Success, int? ProductId, string? Error)> CreateProductAsync(AdminProductFormViewModel model);
    Task<(bool Success, string? Error)> UpdateProductAsync(AdminProductFormViewModel model);
    Task<(bool Success, string? Error)> DeleteProductAsync(int id);
    Task<(bool Success, string? ImageUrl, string? Error)> UploadProductImageAsync(int id, IFormFile file, IWebHostEnvironment env);
}
