using System;
using Microsoft.EntityFrameworkCore.Storage;
using Stationery.Data;
using Stationery.Models;

namespace Stationery.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly StationeryDbContext _context;
    private IRepository<Category>? _categories;
    private IRepository<Product>? _products;
    private IRepository<Cart>? _carts;
    private IRepository<CartItem>? _cartItems;
    private IRepository<Order>? _orders;
    private IRepository<OrderItem>? _orderItems;
    private IRepository<Brand>? _brands;

    public UnitOfWork(StationeryDbContext context)
    {
        _context = context;
    }

    public IRepository<Category> Categories => _categories ??= new Repository<Category>(_context);

    public IRepository<Product> Products => _products ??= new Repository<Product>(_context);

    public IRepository<Cart> Carts => _carts ??= new Repository<Cart>(_context);

    public IRepository<CartItem> CartItems => _cartItems ??= new Repository<CartItem>(_context);

    public IRepository<Order> Orders => _orders ??= new Repository<Order>(_context);

    public IRepository<OrderItem> OrderItems => _orderItems ??= new Repository<OrderItem>(_context);

    public IRepository<Brand> Brands => _brands ??= new Repository<Brand>(_context);

    public ValueTask DisposeAsync()
    {
        return _context.DisposeAsync();
    }

    public async Task ExecuteTransactionAsync(Func<Task> action)
    {
        await using IDbContextTransaction transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            await action();
            await transaction.CommitAsync();
        }
        catch 
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}
