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
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;
    }

    [Fact]
    public async Task UserRepository_GetAllAsync_ReturnsAllUsers()
    {
        // Arrange
        using var context = new AppDbContext(_options);
        var repository = new UserRepository(context);
        var user = new User { UserName = "testuser", PasswordHash = "hash", Role = UserRole.User };
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();

        // Act
        var users = await repository.GetAllAsync();

        // Assert
        Assert.Single(users);
        Assert.Equal("testuser", users.First().UserName);
    }

    [Fact]
    public async Task ProductRepository_AddAsync_AddsProduct()
    {
        // Arrange
        using var context = new AppDbContext(_options);
        var repository = new ProductRepository(context);
        var product = new Product { Name = "Test Product", Description = "Description", Price = 10.0m, Quantity = 5 };

        // Act
        await repository.AddAsync(product);

        // Assert
        var addedProduct = await context.Products.FindAsync(product.Id);
        Assert.NotNull(addedProduct);
        Assert.Equal("Test Product", addedProduct.Name);
    }

    [Fact]
    public async Task OrderRepository_GetByIdAsync_ReturnsOrder()
    {
        // Arrange
        using var context = new AppDbContext(_options);
        var repository = new OrderRepository(context);
        var order = new Order { ProductId = 1, Quantity = 2, CreatedBy = "user" };
        await context.Orders.AddAsync(order);
        await context.SaveChangesAsync();

        // Act
        var retrievedOrder = await repository.GetByIdAsync(order.Id);

        // Assert
        Assert.NotNull(retrievedOrder);
        Assert.Equal(2, retrievedOrder.Quantity);
    }
}