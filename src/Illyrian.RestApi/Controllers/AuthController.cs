using AutoMapper;
using Illyrian.Domain.Services.Auth;
using Illyrian.Domain.Services.User;
using Illyrian.Persistence.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Illyrian.RestApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;

    public AuthController(IAuthService authService, ICurrentUserService currentUserService, IMapper mapper)
    {
        _authService = authService;
        _currentUserService = currentUserService;
        _mapper = mapper;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequest model)
    {
        var result = await _authService.LoginAsync(model.Email, model.Password);

        if (!result.Succeeded)
        {
            return Unauthorized(new { message = result.Message });
        }

        return Ok(new
        {
            token = result.Token,
            userid = result.UserId,
            userrole = result.UserRole,
            userlanguage = result.UserLanguage,
            expiration = result.Expiration
        });
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterRequest model)
    {
        var registerModel = _mapper.Map<RegisterModel>(model);
        var result = await _authService.RegisterAsync(registerModel);

        if (!result.Succeeded)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { Status = "Error", Message = result.Message });
        }

        return Ok(new { Status = "Success", Message = result.Message });
    }

    [HttpPost("register-role")]
    [AllowAnonymous]
    public async Task<IActionResult> RegisterRole([FromBody] RegisterRoleRequest model)
    {
        var result = await _authService.RegisterRoleAsync(model.Name_SQ, model.Name_EN, model.Description);

        if (!result.Succeeded)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { Status = "Error", Message = result.Message });
        }

        return Ok(new { Status = "Success", Message = result.Message });
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await _authService.LogoutAsync();
        return Ok(new { message = "Logged out successfully" });
    }

    [HttpGet("status")]
    public IActionResult GetAuthStatus()
    {
        return Ok(new { isAuthenticated = _currentUserService.IsAuthenticated });
    }

    [Authorize]
    [HttpGet("username")]
    public IActionResult GetUsername()
    {
        return Ok(new { username = _currentUserService.UserName });
    }
}
