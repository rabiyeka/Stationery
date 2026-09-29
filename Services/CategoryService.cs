using System;
using Microsoft.EntityFrameworkCore;
using Stationery.Models;
using Stationery.Repositories;
using Stationery.ViewModels.Admin;
using Stationery.ViewModels.Products;

namespace Stationery.Services;

public class CategoryService : ICategoryService
{
    private readonly IUnitOfWork _unitOfWork;

    public CategoryService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<(bool Success, string? Error)> CreateCategoryAsync(string name, string? description)
    {
        var normalizedName = name.Trim();
        var categoryExists = await _unitOfWork.Categories.Query()
            .AnyAsync(c => c.Name.ToLower() == normalizedName.ToLower());
        if (categoryExists)
        {
            return (false, "Bu isimde bir kategori zaten mevcut.");
        }
        await _unitOfWork.Categories.AddAsync(new Category {
            Name= normalizedName,
            Description= description?.Trim() ?? string.Empty
        });
        await _unitOfWork.SaveChangesAsync();
        return (true, null);

    }

    public async Task<(bool Success, string? Error)> DeleteCategoryAsync(int id)
    {
        var category = await _unitOfWork.Categories.Query()
            .Include(c => c.Products)
            .FirstOrDefaultAsync(c => c.Id == id);
        if (category is null) return (false, "Kategori bulunamadı.");
        if (category.Products.Any())
        {
            return (false, "Bu kategoriye ait ürünler bulunduğu için silinemez.");
        }
        _unitOfWork.Categories.Remove(category);
        await _unitOfWork.SaveChangesAsync();
        return (true, null);
    }

    public async Task<IList<CategoryFilterViewModel>> GetCategoriesForFilterAsync()
    {
        return await _unitOfWork.Categories.Query()
            .Select(c=> new CategoryFilterViewModel
            {
                Id = c.Id,
                Name = c.Name,
                ProductCount = c.Products.Count
            })
            .OrderBy(c=> c.Name)
            .ToListAsync();
    }

    public async Task<IList<AdminCategoryListViewModel>> GetCategoriesForAdminAsync()
    {
        return await _unitOfWork.Categories.Query()
            .Select(c=> new AdminCategoryListViewModel
            {
                Id = c.Id,
                Name = c.Name,
                ProductCount = c.Products.Count
            })
            .OrderBy(c=> c.Name)
            .ToListAsync();
    }

    public async Task<AdminCategoryFormViewModel?> GetCategoryForEditAsync(int id)
    {
        var category= await _unitOfWork.Categories.GetByIdAsync(id);
        if(category is null) return null;
        return new AdminCategoryFormViewModel
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description
        };
    }

    public async Task<(bool Success, string? Error)> UpdateCategoryAsync(int id, string name, string? description)
    {
        var category= await _unitOfWork.Categories.GetByIdAsync(id);
        if(category is null) return (false, "Kategori bulunamadı.");
        category.Name= name;
        category.Description= description?.Trim() ?? string.Empty;
        _unitOfWork.Categories.Update(category);
        await _unitOfWork.SaveChangesAsync();
        return (true, null);
    }
}
