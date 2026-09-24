using System;
using Stationery.ViewModels.Admin;
using Stationery.ViewModels.Products;

namespace Stationery.Services;

public interface IBrandService
{
    Task<IList<BrandFilterViewModel>> GetAllBrandsForFilterAsync();

    Task<IList<AdminBrandListViewModel>> GetBrandsForAdminAsync();

    Task<AdminBrandFormViewModel?> GetBrandForEditAsync(int? id);

    Task<(bool Success, string? Error)> CreateBrandAsync(string name);

    Task<(bool Success, string? Error)> UpdateBrandAsync(int id, string name);

    Task<(bool Success, string? Error)> DeleteBrandAsync(int id);

}
