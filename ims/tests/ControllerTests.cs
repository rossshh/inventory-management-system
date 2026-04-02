using ims.Controllers;
using ims.DTO;
using ims.Helpers;
using ims.Models;
using ims.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace ims.Tests;
public class ControllerTests
{
    private readonly Mock<IProductService> _productServiceMock;
    private readonly Mock<IAuthService> _authServiceMock;
    private readonly Mock<IUserService> _userServiceMock;

    public ControllerTests()
    {
        _productServiceMock = new Mock<IProductService>();
        _authServiceMock = new Mock<IAuthService>();
        _userServiceMock = new Mock<IUserService>();
    }

    [Fact]
    public async Task ProductController_GetById_ReturnsOk_WhenProductExists()
    {
        // Arrange
        var product = new Product { Id = 1, Name = "Laptop" };
        _productServiceMock.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(product);
        var controller = new ProductController(_productServiceMock.Object);

        // Act
        var result = await controller.GetById(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedProduct = Assert.IsType<Product>(okResult.Value);
        Assert.Equal("Laptop", returnedProduct.Name);
    }

    [Fact]
    public async Task ProductController_GetById_ReturnsNotFound_WhenProductMissing()
    {
        // Arrange
        _productServiceMock.Setup(s => s.GetByIdAsync(99)).ReturnsAsync((Product)null!);
        var controller = new ProductController(_productServiceMock.Object);

        // Act
        var result = await controller.GetById(99);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        var error = Assert.IsType<ErrorResponse>(notFoundResult.Value);
        Assert.Equal(404, error.StatusCode);
    }

    [Fact]
    public async Task AuthController_Login_ReturnsOk_WhenCredentialsValid()
    {
        // Arrange
        var authResponse = new AuthResponseDto { Token = "dummy-token", UserName = "admin" };
        _authServiceMock.Setup(s => s.AuthenticateAsync(It.IsAny<LoginDto>())).ReturnsAsync(authResponse);
        var controller = new AuthController(_authServiceMock.Object);

        // Act
        var result = await controller.Login(new LoginDto { UserName = "admin", Password = "password" });

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<AuthResponseDto>(okResult.Value);
        Assert.Equal("dummy-token", response.Token);
    }

    [Fact]
    public async Task AuthController_Login_ReturnsUnauthorized_WhenCredentialsInvalid()
    {
        // Arrange
        _authServiceMock.Setup(s => s.AuthenticateAsync(It.IsAny<LoginDto>())).ReturnsAsync((AuthResponseDto)null!);
        var controller = new AuthController(_authServiceMock.Object);

        // Act
        var result = await controller.Login(new LoginDto { UserName = "bad", Password = "bad" });

        // Assert
        var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
        var error = Assert.IsType<ErrorResponse>(unauthorizedResult.Value);
        Assert.Equal(401, error.StatusCode);
    }

    [Fact]
    public async Task UsersController_GetAll_ReturnsOk_WithListOfUsers()
    {
        // Arrange
        var users = new List<UserDto> { new UserDto { UserName = "user1" } };
        _userServiceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(users);
        var controller = new UsersController(_userServiceMock.Object);

        // Act
        var result = await controller.GetAll();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedUsers = Assert.IsType<List<UserDto>>(okResult.Value);
        Assert.Single(returnedUsers);
    }
}
