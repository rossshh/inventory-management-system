using ims.Data;
using ims.Models;
using ims.Repository;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ims.Tests;

public class RepositoryTests
{
    private DbContextOptions<AppDbContext> _options;

    public RepositoryTests()
    {
        _options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }

    [Fact]
    public async Task UserRepository_GetAllAsync_ReturnsAllUsers()
    {
        // Arrange
        using var context = new AppDbContext(_options);
        var repository = new UserRepository(context);
        var user = new User { UserName = "adminuser", PasswordHash = "hash", Role = UserRole.Admin };
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();

        // Act
        var users = await repository.GetAllAsync();

        // Assert
        Assert.Single(users);
        Assert.Equal("adminuser", users.First().UserName);
    }

    [Fact]
    public async Task ProductRepository_AddAsync_AddsProduct()
    {
        // Arrange
        using var context = new AppDbContext(_options);
        var repository = new ProductRepository(context);
        var product = new Product { Name = "Test Product", Description = "Description", Price = 10.0m, Quantity = 5, SupplierId = 1 };

        // Act
        await repository.AddAsync(product);

        // Assert
        var addedProduct = await context.Products.FindAsync(product.Id);
        Assert.NotNull(addedProduct);
        Assert.Equal("Test Product", addedProduct.Name);
        Assert.Equal(1, addedProduct.SupplierId);
    }

    [Fact]
    public async Task OrderRepository_GetByIdAsync_ReturnsOrderWithItems()
    {
        // Arrange
        using var context = new AppDbContext(_options);
        var repository = new OrderRepository(context);
        var order = new Order 
        { 
            UserId = 1, 
            OrderDate = DateTime.UtcNow,
            OrderItems = new List<OrderItem> 
            { 
                new OrderItem { ProductId = 1, Quantity = 2, UnitPrice = 50.0m } 
            } 
        };
        await context.Orders.AddAsync(order);
        await context.SaveChangesAsync();

        // Act
        var retrievedOrder = await repository.GetByIdAsync(order.Id);

        // Assert
        Assert.NotNull(retrievedOrder);
        Assert.Equal(1, retrievedOrder.UserId);
        Assert.Single(retrievedOrder.OrderItems);
    }

    [Fact]
    public async Task SupplierRepository_AddAsync_AddsSupplier()
    {
        // Arrange
        using var context = new AppDbContext(_options);
        var repository = new SupplierRepository(context);
        var supplier = new Supplier { Name = "Test Supplier", ContactPerson = "Person", Email = "test@test.com" };

        // Act
        await repository.AddAsync(supplier);

        // Assert
        var addedSupplier = await context.Suppliers.FindAsync(supplier.Id);
        Assert.NotNull(addedSupplier);
        Assert.Equal("Test Supplier", addedSupplier.Name);
    }
}