using System;
using Stationery.Models;

namespace Stationery.Repositories;

public interface IUnitOfWork : IAsyncDisposable
{
    IRepository<Category> Categories { get; }
    IRepository<Product> Products { get; }
    IRepository<Cart> Carts { get; }
    IRepository<CartItem> CartItems { get; }
    IRepository<Order> Orders { get; }
    IRepository<OrderItem> OrderItems { get; }
    IRepository<Brand> Brands { get; }
    Task<int> SaveChangesAsync();
    Task ExecuteTransactionAsync(Func<Task> action);

}
