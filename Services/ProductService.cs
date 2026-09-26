using System;
using Microsoft.EntityFrameworkCore;
using Stationery.Models;
using Stationery.Repositories;
using Stationery.ViewModels.Admin;
using Stationery.ViewModels.Products;

namespace Stationery.Services;

public class ProductService : IProductService
{
    private readonly IUnitOfWork _unitOfWork;

    public ProductService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<(bool Success, int? ProductId, string? Error)> CreateProductAsync(AdminProductFormViewModel model)
    {
        var category = await _unitOfWork.Categories.GetByIdAsync(model.CategoryId);
        if (category is null)
        {
            return (false, null, "Kategori bulunamadı.");
        }
        var product = new Product
        {
            Name = model.Name.Trim(),
            Description = model.Description!.Trim(),
            Price = model.Price,
            CategoryId = model.CategoryId,
            BrandId = model.BrandId,
            StockQuantity = model.StockQuantity
        };
        await _unitOfWork.Products.AddAsync(product);
        await _unitOfWork.SaveChangesAsync();
        return (true, product.Id, null);
    }

    public async Task<(bool Success, string? Error)> DeleteProductAsync(int id)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(id);
        if (product is null)
        {
            return (false, "Ürün bulunamadı.");
        }
        _unitOfWork.Products.Remove(product);
        await _unitOfWork.SaveChangesAsync();
        return (true, null);
    }

    public async Task<IList<ProductListViewModel>> GetAllProductsAsync(int? categoryId = null, int? brandId = null)
    {
        var query = _unitOfWork.Products.Query()
            .Include(p => p.Category)
            .Include(p => p.Brand)
            .AsQueryable();

        // Kategori Filtresi
        if (categoryId is > 0)
        {
            query = query.Where(p => p.CategoryId == categoryId);
        }

        var query2= _unitOfWork.Brands.Query()
            .Include(b => b.Products)
            .AsQueryable();
        // Marka Filtresi 
        if (brandId is > 0)
        {
            query = query.Where(p => p.BrandId == brandId);
        }
        var products = await query.ToListAsync();

        return await query.OrderBy(p => p.Name)
            .Select(p => new ProductListViewModel
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                StockQuantity = p.StockQuantity,
                ImageUrl = p.ImageUrl ?? string.Empty,
                BrandName = p.Brand!.Name ?? string.Empty,
                CategoryName = p.Category!.Name ?? string.Empty
            })
            .ToListAsync();
    }

    public async Task<ProductDetailsViewModel?> GetProductDetailsAsync(int id)
    {
        if (id <= 0) return null!;
        var product = await _unitOfWork.Products.Query()
            .Include(p => p.Category)
            .Include(p => p.Brand)
            .FirstOrDefaultAsync(p => p.Id == id);
        if (product is null) return null!;
        return new ProductDetailsViewModel
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            StockQuantity = product.StockQuantity,
            ImageUrl = product.ImageUrl ?? string.Empty,
            BrandName = product.Brand!.Name ?? string.Empty,
            CategoryName = product.Category!.Name ?? string.Empty
        };
    }

    public Task<IList<ProductListViewModel>> GetProductsForAdminAsync()
    {
        return GetAllProductsAsync();
    }

    public async Task<(bool Success, string? Error)> UpdateProductAsync(AdminProductFormViewModel model)
    {
        var product = await _unitOfWork.Products.GetByIdAsync((int)model.Id!);
        if (product is null)
        {
            return (false, "Ürün bulunamadı.");
        }
        var category = await _unitOfWork.Categories.GetByIdAsync(model.CategoryId);
        if (category is null)
        {
            return (false, "Kategori bulunamadı.");
        }
        product.Name = model.Name.Trim();
        product.Description = model.Description?.Trim()!;
        product.Price = model.Price;
        product.StockQuantity = model.StockQuantity;
        product.ImageUrl = model.ImageUrl?.Trim() ?? string.Empty;
        product.CategoryId = model.CategoryId;
        product.BrandId = model.BrandId;
        _unitOfWork.Products.Update(product);
        await _unitOfWork.SaveChangesAsync();
        return (true, null);
    }

    public async Task<AdminProductFormViewModel?> GetProductForEditAsync(int id)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(id);
        if (product is null)
        {
            return null;
        }
        return new AdminProductFormViewModel
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            StockQuantity = product.StockQuantity,
            ImageUrl = product.ImageUrl ?? string.Empty,
            CategoryId = product.CategoryId,
            BrandId = product.BrandId
        };
    }

    public async Task<(bool Success, string? ImageUrl, string? Error)> UploadProductImageAsync(int id, IFormFile file, IWebHostEnvironment env)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(id);
        if (product is null)
        {
            return (false, null, "Ürün bulunamadı.");
        }
        if (file.Length == 0)
        {
            return (false, null, "Geçersiz dosya.");
        }
        string[] allowed = [".jpg", ".jpeg", ".png", ".webp", ".gif"];
        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!allowed.Contains(extension))
        {
            return (false, null, "Yalnızca resim dosyaları yüklenebilir.");
        }
        var directory = Path.Combine(env.WebRootPath, "images", "products");
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
        var fileName = $"{id}_{Guid.NewGuid():N}{extension}";

        var filePath = Path.Combine(directory, fileName);
        await using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        product.ImageUrl = $"/images/products/{fileName}";
        _unitOfWork.Products.Update(product);
        await _unitOfWork.SaveChangesAsync();

        return (true, product.ImageUrl, null);
    }
}
