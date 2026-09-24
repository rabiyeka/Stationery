using System;
using Stationery.ViewModels.Admin;
using Stationery.ViewModels.Products;

namespace Stationery.Services;

public interface ICategoryService
{
    Task<IList<CategoryFilterViewModel>> GetCategoriesForFilterAsync();
    Task<IList<AdminCategoryListViewModel>> GetCategoriesForAdminAsync();
    Task<AdminCategoryFormViewModel?> GetCategoryForEditAsync(int id);
    Task<(bool Success, string? Error)>CreateCategoryAsync(string name, string? description);
    Task<(bool Success, string? Error)> UpdateCategoryAsync(int id, string name, string? description);
    Task<(bool Success, string? Error)> DeleteCategoryAsync(int id);
}
