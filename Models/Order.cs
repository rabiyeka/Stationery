using System;
using Stationery.Models.Enums;

namespace Stationery.Models;

public class Order
{
    public int Id { get; set; }
    public string UserId { get; set; } = null!;
    public StationeryUser? User { get; set; } = null!;
    public DateTime OrderDate { get; set; }
    public decimal TotalAmount { get; set; }
    public string ShippingAddress { get; set; } = string.Empty;
    public string PaymentMethod { get; set; } = string.Empty;
    public ICollection<OrderItem> Items { get; set; } = [];
    public OrderStatus OrderStatus { get; set; } = OrderStatus.Pending;
}
