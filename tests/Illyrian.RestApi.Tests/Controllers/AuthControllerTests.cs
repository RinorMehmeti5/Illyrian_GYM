using Illyrian.Domain.Services.Auth;
using Illyrian.Domain.Services.User;
using Illyrian.Persistence.Auth;
using Illyrian.RestApi.Controllers;
using Illyrian.RestApi.Tests.Configuration;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Illyrian.RestApi.Tests.Controllers;

public class AuthControllerTests
{
    private readonly Mock<IAuthService> _authServiceMock;
    private readonly Mock<ICurrentUserService> _currentUserServiceMock;
    private readonly AuthController _controller;

    public AuthControllerTests()
    {
        _authServiceMock = new Mock<IAuthService>();
        _currentUserServiceMock = new Mock<ICurrentUserService>();
        _controller = new AuthController(_authServiceMock.Object, _currentUserServiceMock.Object, AutoMapperFixture.Mapper);
    }

    [Fact]
    public async Task Login_ValidCredentials_ReturnsOkWithToken()
    {
        var request = new LoginRequest { Email = "test@test.com", Password = "Password123!" };
        var authResult = new AuthResult
        {
            Succeeded = true,
            Token = "jwt-token-here",
            Expiration = DateTime.UtcNow.AddHours(3),
            UserId = "user-1",
            UserRole = "User",
            UserLanguage = "1"
        };

        _authServiceMock.Setup(s => s.LoginAsync(request.Email, request.Password))
            .ReturnsAsync(authResult);

        var result = await _controller.Login(request);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(okResult.Value);
    }

    [Fact]
    public async Task Login_InvalidCredentials_ReturnsUnauthorized()
    {
        var request = new LoginRequest { Email = "test@test.com", Password = "wrong" };
        var authResult = new AuthResult
        {
            Succeeded = false,
            Message = "Invalid credentials"
        };

        _authServiceMock.Setup(s => s.LoginAsync(request.Email, request.Password))
            .ReturnsAsync(authResult);

        var result = await _controller.Login(request);

        Assert.IsType<UnauthorizedObjectResult>(result);
    }

    [Fact]
    public async Task Register_ValidData_ReturnsOk()
    {
        var request = new RegisterRequest
        {
            Email = "new@test.com",
            Password = "Password123!",
            Role = "User",
            Firstname = "John",
            Lastname = "Doe"
        };
        var authResult = new AuthResult { Succeeded = true };

        _authServiceMock.Setup(s => s.RegisterAsync(It.IsAny<RegisterModel>()))
            .ReturnsAsync(authResult);

        var result = await _controller.Register(request);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(okResult.Value);
    }

    [Fact]
    public async Task Register_DuplicateEmail_ReturnsServerError()
    {
        var request = new RegisterRequest
        {
            Email = "existing@test.com",
            Password = "Password123!",
            Role = "User"
        };
        var authResult = new AuthResult { Succeeded = false, Message = "User already exists" };

        _authServiceMock.Setup(s => s.RegisterAsync(It.IsAny<RegisterModel>()))
            .ReturnsAsync(authResult);

        var result = await _controller.Register(request);

        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, objectResult.StatusCode);
    }
}
