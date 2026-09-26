using System;
using Microsoft.EntityFrameworkCore;
using Stationery.Models;
using Stationery.Repositories;
using Stationery.ViewModels.Carts;

namespace Stationery.Services;

public class CartService : ICartService
{
    private readonly IUnitOfWork _unitOfWork;

    public CartService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<(bool Success, string ErrorMessage)> AddItemAsync(string userId, int productId, int quantity)
    {
        if (string.IsNullOrWhiteSpace(userId))
        {
            return (false, "Kullanıcı bilgisi bulunamadı.");
        }

        if (productId <= 0 || quantity <= 0)
        {
            return (false, "Geçersiz ürün veya miktar.");
        }

        var product = await _unitOfWork.Products.GetByIdAsync(productId);
        if (product is null)
        {
            return (false, "Ürün bulunamadı.");
        }

        if (product.StockQuantity <= 0)
        {
            return (false, "Ürün stokta bulunmuyor.");
        }

        var cart = await _unitOfWork.Carts.Query()
            .Include(c => c.CartItems)
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if (cart is null)
        {
            cart = new Cart
            {
                UserId = userId,
                CartItems = []
            };

            await _unitOfWork.Carts.AddAsync(cart);
            await _unitOfWork.SaveChangesAsync();
        }

        var existingItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);
        var newQuantity = (existingItem?.Quantity ?? 0) + quantity;

        if (newQuantity > product.StockQuantity)
        {
            return (false, $"İstenen miktar stok limitini aşıyor. Maksimum: {product.StockQuantity}");
        }

        if (existingItem is null)
        {
            var cartItem = new CartItem
            {
                CartId = cart.Id,
                ProductId = productId,
                Quantity = quantity
            };

            await _unitOfWork.CartItems.AddAsync(cartItem);
        }
        else
        {
            existingItem.Quantity = newQuantity;
            _unitOfWork.CartItems.Update(existingItem);
        }

        await _unitOfWork.SaveChangesAsync();
        return (true, string.Empty);
    }

    public async Task<CartIndexViewModel> GetCartAsync(string userId)
    {
        if (string.IsNullOrWhiteSpace(userId))
        {
            return new CartIndexViewModel();
        }

        var cart = await _unitOfWork.Carts.Query()
            .Include(c => c.CartItems)
            .ThenInclude(ci => ci.Product)
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if (cart is null)
        {
            return new CartIndexViewModel();
        }

        var items = cart.CartItems
            .Select(ci => new CartItemViewModel
            {
                Id = ci.Id,
                ProductId = ci.ProductId,
                ProductName = ci.Product?.Name ?? "Ürün",
                ImageUrl = ci.Product?.ImageUrl ?? string.Empty,
                UnitPrice = ci.Product?.Price ?? 0m,
                Quantity = ci.Quantity
            })
            .ToList();

        return new CartIndexViewModel
        {
            Items = items
        };
    }

    public async Task<(bool Success, string ErrorMessage)> RemoveItemAsync(string userId, int cartItemId)
    {
        if (string.IsNullOrWhiteSpace(userId))
        {
            return (false, "Kullanıcı bilgisi bulunamadı.");
        }

        if (cartItemId <= 0)
        {
            return (false, "Geçersiz sepet öğesi.");
        }

        var cart = await _unitOfWork.Carts.Query()
            .Include(c => c.CartItems)
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if (cart is null)
        {
            return (false, "Sepet bulunamadı.");
        }

        var cartItem = cart.CartItems.FirstOrDefault(ci => ci.Id == cartItemId);
        if (cartItem is null)
        {
            return (false, "Sepette bu ürün bulunamadı.");
        }

        _unitOfWork.CartItems.Remove(cartItem);
        await _unitOfWork.SaveChangesAsync();
        return (true, string.Empty);
    }

    public async Task<(bool Success, string ErrorMessage)> UpdateItemQuantityAsync(string userId, int cartItemId, int quantity)
    {
        if (string.IsNullOrWhiteSpace(userId))
        {
            return (false, "Kullanıcı bilgisi bulunamadı.");
        }

        if (cartItemId <= 0 || quantity <= 0)
        {
            return (false, "Geçersiz miktar.");
        }

        var cart = await _unitOfWork.Carts.Query()
            .Include(c => c.CartItems)
            .ThenInclude(ci => ci.Product)
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if (cart is null)
        {
            return (false, "Sepet bulunamadı.");
        }

        var cartItem = cart.CartItems.FirstOrDefault(ci => ci.Id == cartItemId);
        if (cartItem is null)
        {
            return (false, "Sepette bu ürün bulunamadı.");
        }

        if (cartItem.Product is null)
        {
            cartItem.Product = await _unitOfWork.Products.GetByIdAsync(cartItem.ProductId);
        }

        if (cartItem.Product is null)
        {
            return (false, "Ürün bulunamadı.");
        }

        if (quantity > cartItem.Product.StockQuantity)
        {
            return (false, $"İstenen miktar stok limitini aşıyor. Maksimum: {cartItem.Product.StockQuantity}");
        }

        cartItem.Quantity = quantity;
        _unitOfWork.CartItems.Update(cartItem);
        await _unitOfWork.SaveChangesAsync();
        return (true, string.Empty);
    }
}
