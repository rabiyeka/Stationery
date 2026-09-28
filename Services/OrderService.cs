using System;
using Microsoft.EntityFrameworkCore;
using Stationery.Models;
using Stationery.Models.Enums;
using Stationery.Repositories;
using Stationery.ViewModels.Orders;
using Stationery.ViewModels.Admin;

namespace Stationery.Services;

public class OrderService : IOrderService
{
    private readonly IUnitOfWork _unitOfWork;

    public OrderService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<OrderDetailsViewModel?> GetOrderDetailsAsync(int orderId)
    {
        if (orderId <= 0)
        {
            return null;
        }

        var order = await _unitOfWork.Orders.Query()
            .Include(o => o.Items)
            .ThenInclude(i => i.Product)
            .Include(o => o.User)
            .FirstOrDefaultAsync(o => o.Id == orderId);

        if (order is null)
        {
            return null;
        }

        return new OrderDetailsViewModel
        {
            Id = order.Id,
            OrderDate = order.OrderDate,
            TotalAmount = order.TotalAmount,
            ShippingAddress = order.ShippingAddress,
            PaymentMethod = order.PaymentMethod,
            OrderStatus = order.OrderStatus,
            UserId = order.UserId,
            UserName = order.User?.UserName ?? string.Empty,
            Items = order.Items
                .Select(i => new OrderItemViewModel
                {
                    Id = i.Id,
                    ProductId = i.ProductId,
                    ProductName = i.Product?.Name ?? "Ürün",
                    ImageUrl = i.Product?.ImageUrl ?? string.Empty,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice
                })
                .ToList()
        };
    }

    public async Task<IList<AdminOrderSummaryViewModel>> GetAllOrdersForAdminAsync()
    {
        return await _unitOfWork
            .Orders
            .Query()
            .Include(o=>o.User)
            .OrderByDescending(o=>o.OrderDate)
            .Select(o=>new AdminOrderSummaryViewModel
            {
                Id=o.Id,
                OrderDate=o.OrderDate,
                Status=o.OrderStatus.ToString(),
                TotalAmount=o.TotalAmount,
                ItemCount=o.Items.Sum(i=>i.Quantity),
                CustomerEmail = o.User!.Email ?? string.Empty
            }).ToListAsync();
    }
    public async Task<IList<OrderDetailsViewModel>> GetOrdersForUserAsync(string userId)
    {
        if (string.IsNullOrWhiteSpace(userId))
        {
            return [];
        }

        return await _unitOfWork.Orders.Query()
            .Where(o => o.UserId == userId)
            .Include(o => o.Items)
            .ThenInclude(i => i.Product)
            .Include(o => o.User)
            .OrderByDescending(o => o.OrderDate)
            .Select(o => new OrderDetailsViewModel
            {
                Id = o.Id,
                OrderDate = o.OrderDate,
                TotalAmount = o.TotalAmount,
                ShippingAddress = o.ShippingAddress,
                PaymentMethod = o.PaymentMethod,
                OrderStatus = o.OrderStatus,
                UserId = o.UserId,
                UserName = o.User != null ? o.User.UserName ?? string.Empty : string.Empty,
                Items = o.Items.Select(i => new OrderItemViewModel
                {
                    Id = i.Id,
                    ProductId = i.ProductId,
                    ProductName = i.Product != null ? i.Product.Name : "Ürün",
                    ImageUrl = i.Product != null ? i.Product.ImageUrl : string.Empty,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice
                }).ToList()
            })
            .ToListAsync();
    }

    public async Task<IList<OrderSummaryItemViewModel>> GetMyOrdersAsync(string userId)
    {
        return await _unitOfWork
                .Orders
                .Query()
                .Where(o=>o.UserId==userId)
                .OrderByDescending(o => o.OrderDate)
                .Select(o => new OrderSummaryItemViewModel
                {
                    Id = o.Id,
                    OrderDate = o.OrderDate,
                    Status = o.OrderStatus.ToString(),
                    TotalAmount = o.TotalAmount,
                    ItemCount = o.Items.Sum(i => i.Quantity)
                }).ToListAsync();
    }
    public async Task<IList<OrderDetailsViewModel>> GetOrdersForAdminAsync()
    {
        return await _unitOfWork.Orders.Query()
            .Include(o => o.Items)
            .ThenInclude(i => i.Product)
            .Include(o => o.User)
            .OrderByDescending(o => o.OrderDate)
            .Select(o => new OrderDetailsViewModel
            {
                Id = o.Id,
                OrderDate = o.OrderDate,
                TotalAmount = o.TotalAmount,
                OrderStatus = o.OrderStatus,
                UserId = o.UserId,
                UserName = o.User != null ? o.User.UserName ?? string.Empty : string.Empty,
                Items = o.Items.Select(i => new OrderItemViewModel
                {
                    Id = i.Id,
                    ProductId = i.ProductId,
                    ProductName = i.Product != null ? i.Product.Name : "Ürün",
                    ImageUrl = i.Product != null ? i.Product.ImageUrl : string.Empty,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice
                }).ToList()
            })
            .ToListAsync();
    }

    public async Task<(bool Success, int? OrderId, string? Error)> CreateOrderAsync(string userId, string shippingAddress, string paymentMethod)
    {
        if (string.IsNullOrWhiteSpace(userId))
        {
            return (false, null, "Kullanıcı bilgisi bulunamadı.");
        }

        var cart = await _unitOfWork.Carts.Query()
            .Include(c => c.CartItems)
            .ThenInclude(ci => ci.Product)
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if (cart is null || cart.CartItems.Count == 0)
        {
            return (false, null, "Sepet boş.");
        }

        foreach (var cartItem in cart.CartItems)
        {
            if (cartItem.Product is null)
            {
                return (false, null, $"Id:{cartItem.ProductId} id'li ürün bulunamadı.");
            }

            if (cartItem.Quantity <= 0)
            {
                return (false, null, "Geçersiz ürün miktarı.");
            }

            if (cartItem.Quantity > cartItem.Product.StockQuantity)
            {
                return (false, null, $"{cartItem.Product.Name} için stok yetersiz.");
            }
        }

        var totalAmount = cart.CartItems.Sum(ci => ci.Product!.Price * ci.Quantity);

        var order = new Order
        {
            UserId = userId,
            OrderDate = DateTime.UtcNow,
            TotalAmount = totalAmount,
            ShippingAddress = shippingAddress,
            PaymentMethod = paymentMethod,
            OrderStatus = OrderStatus.Pending,
            Items = []
        };

        await _unitOfWork.Orders.AddAsync(order);
        await _unitOfWork.SaveChangesAsync();

        foreach (var cartItem in cart.CartItems)
        {
            var product = cartItem.Product!;
            product.StockQuantity -= cartItem.Quantity;

            var orderItem = new OrderItem
            {
                OrderId = order.Id,
                ProductId = product.Id,
                Quantity = cartItem.Quantity,
                UnitPrice = product.Price
            };

            await _unitOfWork.OrderItems.AddAsync(orderItem);
            _unitOfWork.CartItems.Remove(cartItem);
        }

        await _unitOfWork.SaveChangesAsync();

        return (true, order.Id, null);
    }

    public async Task<(bool Success, string? Error)> UpdateOrderStatusAsync(int orderId, OrderStatus status)
    {
        if (orderId <= 0)
        {
            return (false, "Geçersiz sipariş numarası.");
        }

        var order = await _unitOfWork.Orders.GetByIdAsync(orderId);
        if (order is null)
        {
            return (false, "Sipariş bulunamadı.");
        }

        order.OrderStatus = status;
        _unitOfWork.Orders.Update(order);
        await _unitOfWork.SaveChangesAsync();

        return (true, null);
    }

    public async Task<(bool Success, string? Error)> CancelOrderAsync(int orderId)
    {
        if (orderId <= 0)
        {
            return (false, "Geçersiz sipariş numarası.");
        }

        var order = await _unitOfWork.Orders.Query()
            .Include(o => o.Items)
            .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(o => o.Id == orderId);

        if (order is null)
        {
            return (false, "Sipariş bulunamadı.");
        }

        if (order.OrderStatus == OrderStatus.Cancelled)
        {
            return (false, "Sipariş zaten iptal edilmiş.");
        }

        if (order.OrderStatus == OrderStatus.Delivered)
        {
            return (false, "Teslim edilen sipariş iptal edilemez.");
        }

        foreach (var item in order.Items)
        {
            if (item.Product is not null)
            {
                item.Product.StockQuantity += item.Quantity;
            }
        }

        order.OrderStatus = OrderStatus.Cancelled;
        _unitOfWork.Orders.Update(order);
        await _unitOfWork.SaveChangesAsync();

        return (true, null);
    }
}
