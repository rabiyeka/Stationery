using System;

namespace Stationery.ViewModels.Admin;

public class AdminOrderSummaryViewModel
{
    public int Id { get; set; }
    public DateTime OrderDate { get; set; }
    public string Status { get; set; } = null!;
    public decimal TotalAmount { get; set; }
    public int ItemCount { get; set; }
    public string CustomerEmail { get; set; } = null!;
    public string ShippingAddress { get; set; } = string.Empty;

}
