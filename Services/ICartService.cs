using System;
using Stationery.ViewModels.Carts;

namespace Stationery.Services;

public interface ICartService
{
    Task<CartIndexViewModel> GetCartAsync(string userId);
    Task<(bool Success, string ErrorMessage)> AddItemAsync(string userId, int productId, int quantity);
    Task<(bool Success, string ErrorMessage)> UpdateItemQuantityAsync(string userId, int cartItemId, int quantity);
    Task<(bool Success, string ErrorMessage)> RemoveItemAsync(string userId, int cartItemId);
}
