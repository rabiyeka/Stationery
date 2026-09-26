using System;
using Microsoft.EntityFrameworkCore;
using Stationery.Models;
using Stationery.Repositories;
using Stationery.ViewModels.Admin;
using Stationery.ViewModels.Products;

namespace Stationery.Services;

public class BrandService : IBrandService
{
    private readonly IUnitOfWork _unitOfWork;

    public BrandService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IList<BrandFilterViewModel>> GetAllBrandsForFilterAsync()
    {
        return await _unitOfWork.Brands.Query()
            .Select(b => new BrandFilterViewModel
            {
                Id = b.Id,
                Name = b.Name,
                ProductCount = b.Products.Count
            })
            .OrderBy(b => b.Name)
            .ToListAsync();
    }

    public async Task<IList<AdminBrandListViewModel>> GetBrandsForAdminAsync()
    {
        return await _unitOfWork.Brands.Query()
            .Select(b => new AdminBrandListViewModel
            {
                Id = b.Id,
                Name = b.Name,
                ProductCount = b.Products.Count
            })
            .OrderBy(b => b.Name)
            .ToListAsync();
    }

    public async Task<AdminBrandFormViewModel?> GetBrandForEditAsync(int? id)
    {
        if (id is null || id <= 0)
        {
            return null;
        }

        var brand = await _unitOfWork.Brands.GetByIdAsync(id.Value);
        if (brand is null)
        {
            return null;
        }

        return new AdminBrandFormViewModel
        {
            Id = brand.Id,
            Name = brand.Name
        };
    }

    public async Task<(bool Success, string? Error)> CreateBrandAsync(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return (false, "Marka adı boş olamaz.");
        }

        var exists = await _unitOfWork.Brands.Query()
            .AnyAsync(b => b.Name.ToLower() == name.Trim().ToLower());

        if (exists)
        {
            return (false, "Bu isimde bir marka zaten mevcut.");
        }

        await _unitOfWork.Brands.AddAsync(new Brand
        {
            Name = name.Trim()
        });

        await _unitOfWork.SaveChangesAsync();
        return (true, null);
    }

    public async Task<(bool Success, string? Error)> UpdateBrandAsync(int id, string name)
    {
        var brand = await _unitOfWork.Brands.GetByIdAsync(id);
        if (brand is null)
        {
            return (false, "Marka bulunamadı.");
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            return (false, "Marka adı boş olamaz.");
        }

        var exists = await _unitOfWork.Brands.Query()
            .AnyAsync(b => b.Id != id && b.Name.ToLower() == name.Trim().ToLower());

        if (exists)
        {
            return (false, "Bu isimde başka bir marka zaten mevcut.");
        }

        brand.Name = name.Trim();
        _unitOfWork.Brands.Update(brand);
        await _unitOfWork.SaveChangesAsync();
        return (true, null);
    }

    public async Task<(bool Success, string? Error)> DeleteBrandAsync(int id)
    {
        var brand = await _unitOfWork.Brands.Query()
            .Include(b => b.Products)
            .FirstOrDefaultAsync(b => b.Id == id);

        if (brand is null)
        {
            return (false, "Marka bulunamadı.");
        }

        if (brand.Products.Any())
        {
            return (false, "Bu markaya ait ürünler bulunduğu için silinemez.");
        }

        _unitOfWork.Brands.Remove(brand);
        await _unitOfWork.SaveChangesAsync();
        return (true, null);
    }
}
