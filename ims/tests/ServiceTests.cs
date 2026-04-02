using ims.DTO;
using ims.Helpers;
using ims.Models;
using ims.Repository.Interfaces;
using ims.Services;
using Microsoft.Extensions.Options;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace ims.Tests;

public class ServiceTests
{
    private readonly Mock<IUserRepository> _userRepoMock;
    private readonly Mock<IOrderRepository> _orderRepoMock;
    private readonly Mock<IProductRepository> _productRepoMock;
    private readonly IOptions<JwtSettings> _jwtOptions;

    public ServiceTests()
    {
        _userRepoMock = new Mock<IUserRepository>();
        _orderRepoMock = new Mock<IOrderRepository>();
        _productRepoMock = new Mock<IProductRepository>();
        var settings = new JwtSettings
        {
            SecretKey = "SuperSecretKeyForTestingPurposesOnly!",
            Issuer = "TestIssuer",
            Audience = "TestAudience",
            ExpiryMinutes = 60
        };
        _jwtOptions = Options.Create(settings);
    }

    [Fact]
    public async Task AuthService_AuthenticateAsync_ReturnsToken_WhenCredentialsValid()
    {
        // Arrange
        var password = "TestPassword123!";
        var user = new User { Id = 1, UserName = "testuser", Role = UserRole.Staff };
        var hasher = new Microsoft.AspNetCore.Identity.PasswordHasher<User>();
        user.PasswordHash = hasher.HashPassword(user, password);

        _userRepoMock.Setup(r => r.GetByUserNameAsync("testuser")).ReturnsAsync(user);
        
        var authService = new AuthService(_userRepoMock.Object, _jwtOptions);
        var loginDto = new LoginDto { UserName = "testuser", Password = password };

        // Act
        var result = await authService.AuthenticateAsync(loginDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("testuser", result.UserName);
        Assert.NotEmpty(result.Token);
    }

    [Fact]
    public async Task AuthService_RegisterAsync_ReturnsUserDto_WhenUserDoesNotExist()
    {
        // Arrange
        _userRepoMock.Setup(r => r.ExistsByUserNameAsync("newuser")).ReturnsAsync(false);
        var authService = new AuthService(_userRepoMock.Object, _jwtOptions);
        var registerDto = new RegisterDto { UserName = "newuser", Password = "Password123!", Role = UserRole.Staff };

        // Act
        var result = await authService.RegisterAsync(registerDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("newuser", result.UserName);
        _userRepoMock.Verify(r => r.AddAsync(It.IsAny<User>()), Times.Once);
    }

    [Fact]
    public async Task AuthService_RegisterAsync_ReturnsNull_WhenUserAlreadyExists()
    {
        // Arrange
        _userRepoMock.Setup(r => r.ExistsByUserNameAsync("existinguser")).ReturnsAsync(true);
        var authService = new AuthService(_userRepoMock.Object, _jwtOptions);
        var registerDto = new RegisterDto { UserName = "existinguser", Password = "Password123!" };

        // Act
        var result = await authService.RegisterAsync(registerDto);

        // Assert
        Assert.Null(result);
        _userRepoMock.Verify(r => r.AddAsync(It.IsAny<User>()), Times.Never);
    }

    [Fact]
    public async Task AuthService_AuthenticateAsync_ReturnsNull_WhenPasswordInvalid()
    {
        // Arrange
        var user = new User { UserName = "testuser", PasswordHash = "somehash" };
        _userRepoMock.Setup(r => r.GetByUserNameAsync("testuser")).ReturnsAsync(user);
        
        var authService = new AuthService(_userRepoMock.Object, _jwtOptions);
        var loginDto = new LoginDto { UserName = "testuser", Password = "WrongPassword" };

        // Act
        var result = await authService.AuthenticateAsync(loginDto);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task UserService_GetByIdAsync_ReturnsUserDto_WithoutPasswordHash()
    {
        // Arrange
        var user = new User { Id = 1, UserName = "testuser", PasswordHash = "secret-hash", Role = UserRole.Admin };
        _userRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(user);
        
        var userService = new UserService(_userRepoMock.Object);

        // Act
        var result = await userService.GetByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("testuser", result.UserName);
        // The DTO doesn't even have PasswordHash, so this is verified by the return type
    }

    [Fact]
    public async Task OrderService_GetSupplierOrderReportAsync_ReturnsReportFromRepository()
    {
        // Arrange
        var mockReport = new List<SupplierOrderReportDto>
        {
            new SupplierOrderReportDto { Supplier = "Supplier A", TotalQuantity = 10, TotalValue = 500.0m }
        };
        _orderRepoMock.Setup(r => r.GetSupplierOrderReportAsync()).ReturnsAsync(mockReport);
        
        var orderService = new OrderService(_orderRepoMock.Object, _productRepoMock.Object);

        // Act
        var result = await orderService.GetSupplierOrderReportAsync();

        // Assert
        Assert.Single(result);
        Assert.Equal("Supplier A", result.First().Supplier);
        Assert.Equal(500.0m, result.First().TotalValue);
    }
}
