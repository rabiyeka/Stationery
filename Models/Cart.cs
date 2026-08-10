using System;

namespace Stationery.Models;

public class Cart
{
    public int Id { get; set; }
    public string UserId { get; set; } = null!;
    public StationeryUser? User { get; set; } = null!;
    public ICollection<CartItem> CartItems { get; set; } = [];
}
