using AutoMapper;
using Illyrian.Domain.Services.Auth;
using Illyrian.Persistence.Administration.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Illyrian.RestApi.Areas.Administration.Controllers;

[Area("Administration")]
[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Admin,Administrator")]
public class UserController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IMapper _mapper;
    private readonly ILogger<UserController> _logger;

    public UserController(IAuthService authService, IMapper mapper, ILogger<UserController> logger)
    {
        _authService = authService;
        _mapper = mapper;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
    {
        try
        {
            var users = await _authService.GetAllUsersWithRolesAsync();
            return Ok(users.Select(u => _mapper.Map<UserDto>(u)));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting users");
            return StatusCode(500, new { message = "An error occurred while fetching users", error = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto>> GetUser(string id)
    {
        try
        {
            var user = await _authService.FindUserByIdAsync(id);
            if (user == null) return NotFound();
            return Ok(_mapper.Map<UserDto>(user));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting user with ID: {Id}", id);
            return StatusCode(500, new { message = $"An error occurred while fetching user with ID: {id}", error = ex.Message });
        }
    }

    [HttpPost]
    public async Task<ActionResult<UserDto>> CreateUser(CreateUserRequest request)
    {
        try
        {
            var model = _mapper.Map<CreateUserModel>(request);
            var result = await _authService.CreateUserAsync(model);
            if (!result.Succeeded)
            {
                return BadRequest(new { message = result.Message, errors = result.Errors });
            }

            var user = await _authService.FindUserByIdAsync(result.UserId!);
            return CreatedAtAction(nameof(GetUser), new { id = result.UserId }, _mapper.Map<UserDto>(user));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating user");
            return StatusCode(500, new { message = "An error occurred while creating user", error = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(string id, UpdateUserRequest request)
    {
        try
        {
            var model = _mapper.Map<UpdateUserModel>(request);
            var result = await _authService.UpdateUserAsync(id, model);
            if (!result.Succeeded)
            {
                return BadRequest(new { message = result.Message, errors = result.Errors });
            }
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating user with ID: {Id}", id);
            return StatusCode(500, new { message = $"An error occurred while updating user with ID: {id}", error = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(string id)
    {
        try
        {
            var result = await _authService.DeleteUserAsync(id);
            if (!result.Succeeded)
            {
                return BadRequest(new { message = result.Message });
            }
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting user with ID: {Id}", id);
            return StatusCode(500, new { message = $"An error occurred while deleting user with ID: {id}", error = ex.Message });
        }
    }

    [HttpGet("roles")]
    public async Task<ActionResult<IEnumerable<RoleDto>>> GetRoles()
    {
        try
        {
            var roles = await _authService.GetRolesAsync();
            return Ok(roles.Select(r => _mapper.Map<RoleDto>(r)));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting roles");
            return StatusCode(500, new { message = "An error occurred while fetching roles", error = ex.Message });
        }
    }

    [HttpPost("{id}/reset-password")]
    public async Task<IActionResult> ResetPassword(string id, ResetPasswordRequest request)
    {
        try
        {
            var result = await _authService.ResetPasswordAsync(id, request.NewPassword);
            if (!result.Succeeded)
            {
                return BadRequest(new { message = result.Message, errors = result.Errors });
            }
            return Ok(new { message = result.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error resetting password for user with ID: {Id}", id);
            return StatusCode(500, new { message = $"An error occurred while resetting password for user with ID: {id}", error = ex.Message });
        }
    }
}
