using System;
using Stationery.Models.Enums;
using Stationery.ViewModels.Orders;
using Stationery.ViewModels.Admin;

namespace Stationery.Services;

public interface IOrderService
{
    Task<OrderDetailsViewModel?> GetOrderDetailsAsync(int orderId);
    Task<IList<OrderDetailsViewModel>> GetOrdersForUserAsync(string userId);
    Task<IList<OrderDetailsViewModel>> GetOrdersForAdminAsync();
    Task<IList<OrderSummaryItemViewModel>> GetMyOrdersAsync(string userId);
    Task<IList<AdminOrderSummaryViewModel>> GetAllOrdersForAdminAsync();
    Task<(bool Success, int? OrderId, string? Error)> CreateOrderAsync(string userId, string shippingAddress, string paymentMethod);
    Task<(bool Success, string? Error)> UpdateOrderStatusAsync(int orderId, OrderStatus status);
    Task<(bool Success, string? Error)> CancelOrderAsync(int orderId);
}
    