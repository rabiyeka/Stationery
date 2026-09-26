using System;

namespace Stationery.ViewModels.Orders;

public class OrderSummaryItemViewModel
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = null!;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal LineTotal => Quantity * UnitPrice;
}
