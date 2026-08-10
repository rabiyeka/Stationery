using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Stationery.Models;

namespace Stationery.Data;

public class StationeryDbContext : IdentityDbContext<StationeryUser>
{
    public StationeryDbContext(DbContextOptions<StationeryDbContext> options) : base(options)
    {
    }
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Brand> Brands => Set<Brand>();
    public DbSet<Cart> Carts => Set<Cart>();
    public DbSet<CartItem> CartItems => Set<CartItem>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Product>()
            .HasOne(p => p.Brand)
            .WithMany(b => b.Products)
            .HasForeignKey(p => p.BrandId)
            .OnDelete(DeleteBehavior.Restrict); 

        builder.Entity<Brand>()
            .Property(b => b.Name)
            .IsRequired()
            .HasMaxLength(50);

        builder.Entity<Brand>()
            .HasIndex(b => b.Name)
            .IsUnique();

        builder.Entity<Product>()
            .HasOne(p => p.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Product>()
            .Property(p=> p.Price)
            .HasPrecision(10, 2);

        builder.Entity<Cart>()
            .HasOne(c => c.User)
            .WithMany()
            .HasForeignKey(c=> c.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Cart>()
            .HasIndex(c=>c.UserId)
            .IsUnique();

        builder.Entity<CartItem>()
            .HasOne(i=>i.Cart)
            .WithMany(c=>c.CartItems)
            .HasForeignKey(i=>i.CartId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.Entity<CartItem>()
            .HasOne(i=>i.Product)
            .WithMany()
            .HasForeignKey(i=>i.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<CartItem>()
            .HasIndex(i=>new {i.CartId,i.ProductId})
            .IsUnique();
        
        builder.Entity<Order>()
            .HasOne(o=>o.User)
            .WithMany()
            .HasForeignKey(o=>o.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Order>()
            .Property(o=>o.TotalAmount)
            .HasPrecision(10,2);

        builder.Entity<Order>()
            .Property(o=>o.OrderStatus)
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Entity<OrderItem>()
            .HasOne(i=>i.Order)
            .WithMany(o=>o.Items)
            .HasForeignKey(i=>i.OrderId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.Entity<OrderItem>()
            .HasOne(i=>i.Product)
            .WithMany()
            .HasForeignKey(i=>i.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<OrderItem>()
            .Property(i=>i.UnitPrice)
            .HasPrecision(10,2);
        
    
    }
}
