using System;

namespace Stationery.ViewModels.Carts;

public class CartIndexViewModel
{
    public IList<CartItemViewModel> Items { get; set; } = [];
    public decimal TotalAmount => Items.Sum(i => i.LineTotal);
    public int TotalItemCount => Items.Sum(i => i.Quantity);
}
