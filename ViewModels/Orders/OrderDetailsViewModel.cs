using System;
using Stationery.Models.Enums;

namespace Stationery.ViewModels.Orders;

public class OrderDetailsViewModel
{
    public int Id { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal TotalAmount { get; set; }
    public OrderStatus OrderStatus { get; set; }
    public string UserId { get; set; } = null!;
    public string UserName { get; set; } = string.Empty;
    public List<OrderItemViewModel> Items { get; set; } = [];
}
