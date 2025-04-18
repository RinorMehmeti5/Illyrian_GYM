using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IllyrianAPI.Data.General;
using IllyrianAPI.Data.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using IllyrianAPI.Models.User;
using System.Globalization;
using Microsoft.AspNetCore.Identity.Data;

namespace IllyrianAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,Administrator")]
    public class UserController : BaseController
    {
        private readonly ILogger<UserController> _logger;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public UserController(
            IllyrianContext db,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            ILogger<UserController> logger
        ) : base(db, userManager)
        {
            _logger = logger;
            _roleManager = roleManager;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers()
        {
            try
            {
                _logger.LogInformation("Fetching all users");

                var users = await _db.AspNetUsers
                    .Include(u => u.Role)
                    .ToListAsync();

                var userDTOs = new List<UserDTO>();
                foreach (var user in users)
                {
                    var roles = user.Role.Select(r => r.Name).ToList();

                    userDTOs.Add(new UserDTO
                    {
                        Id = user.Id,
                        PersonalNumber = user.PersonalNumber,
                        Firstname = user.Firstname,
                        Lastname = user.Lastname,
                        Email = user.Email,
                        UserName = user.UserName,
                        PhoneNumber = user.PhoneNumber,
                        Address = user.Address,
                        Birthdate = user.Birthdate,
                        Active = user.Active,
                        InsertedDate = user.InsertedDate,
                        FormattedBirthdate = user.Birthdate.HasValue ? FormatDate(user.Birthdate.Value) : null,
                        FormattedInsertedDate = FormatDate(user.InsertedDate),
                        FullName = $"{user.Firstname ?? ""} {user.Lastname ?? ""}",
                        Roles = roles
                    });
                }

                return userDTOs;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting users");
                return StatusCode(500, new { message = "An error occurred while fetching users", error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> GetUser(string id)
        {
            try
            {
                _logger.LogInformation($"Fetching user with ID: {id}");

                var user = await _db.AspNetUsers
                    .Include(u => u.Role)
                    .FirstOrDefaultAsync(u => u.Id == id);

                if (user == null)
                {
                    return NotFound();
                }

                var roles = user.Role.Select(r => r.Name).ToList();

                var userDTO = new UserDTO
                {
                    Id = user.Id,
                    PersonalNumber = user.PersonalNumber,
                    Firstname = user.Firstname,
                    Lastname = user.Lastname,
                    Email = user.Email,
                    UserName = user.UserName,
                    PhoneNumber = user.PhoneNumber,
                    Address = user.Address,
                    Birthdate = user.Birthdate,
                    Active = user.Active,
                    InsertedDate = user.InsertedDate,
                    FormattedBirthdate = user.Birthdate.HasValue ? FormatDate(user.Birthdate.Value) : null,
                    FormattedInsertedDate = FormatDate(user.InsertedDate),
                    FullName = $"{user.Firstname ?? ""} {user.Lastname ?? ""}",
                    Roles = roles
                };

                return userDTO;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting user with ID: {id}");
                return StatusCode(500, new { message = $"An error occurred while fetching user with ID: {id}", error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult<UserDTO>> CreateUser(CreateUserRequest request)
        {
            try
            {
                _logger.LogInformation("Creating new user");

                // Check if user already exists
                var userExists = await _userManager.FindByEmailAsync(request.Email);
                if (userExists != null)
                {
                    return BadRequest(new { message = "User with this email already exists" });
                }

                var user = new ApplicationUser
                {
                    Email = request.Email,
                    UserName = string.IsNullOrEmpty(request.UserName) ? request.Email : request.UserName,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    PersonalNumber = request.PersonalNumber,
                    Firstname = request.Firstname,
                    Lastname = request.Lastname,
                    Birthdate = request.Birthdate,
                    PasswordExpires = DateTime.Now.AddMonths(3),
                    CityID = request.CityId,
                    SettlementID = request.SettlementId,
                    Address = request.Address,
                    PhoneNumber = request.PhoneNumber,
                    Active = request.Active ?? true,
                    AllowNotifications = true,
                    AllowAdminNotification = true,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    TwoFactorEnabled = false,
                    LockoutEnabled = false,
                    AccessFailedCount = 0,
                    InsertedDate = DateTime.Now
                };

                var result = await _userManager.CreateAsync(user, request.Password);
                if (!result.Succeeded)
                {
                    return BadRequest(new { message = "User creation failed", errors = result.Errors });
                }

                // Add roles
                if (request.Roles != null && request.Roles.Any())
                {
                    foreach (var roleName in request.Roles)
                    {
                        var role = await _roleManager.FindByNameAsync(roleName);
                        if (role != null)
                        {
                            await _userManager.AddToRoleAsync(user, role.Name);
                        }
                    }
                }
                else
                {
                    // Add default "User" role if no roles provided
                    var userRole = await _roleManager.FindByNameAsync("User");
                    if (userRole != null)
                    {
                        await _userManager.AddToRoleAsync(user, userRole.Name);
                    }
                }

                // Get user with roles for response
                var createdUser = await _userManager.FindByIdAsync(user.Id);
                var userRoles = await _userManager.GetRolesAsync(createdUser);

                var userDTO = new UserDTO
                {
                    Id = user.Id,
                    PersonalNumber = user.PersonalNumber,
                    Firstname = user.Firstname,
                    Lastname = user.Lastname,
                    Email = user.Email,
                    UserName = user.UserName,
                    PhoneNumber = user.PhoneNumber,
                    Address = user.Address,
                    Birthdate = user.Birthdate,
                    Active = user.Active,
                    InsertedDate = user.InsertedDate,
                    FormattedBirthdate = user.Birthdate.HasValue ? FormatDate(user.Birthdate.Value) : null,
                    FormattedInsertedDate = FormatDate(user.InsertedDate),
                    FullName = $"{user.Firstname ?? ""} {user.Lastname ?? ""}",
                    Roles = userRoles.ToList()
                };

                return CreatedAtAction(nameof(GetUser), new { id = user.Id }, userDTO);
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
                _logger.LogInformation($"Updating user with ID: {id}");

                var user = await _userManager.FindByIdAsync(id);
                if (user == null)
                {
                    return NotFound();
                }

                // Update user properties
                if (!string.IsNullOrEmpty(request.Email) && user.Email != request.Email)
                {
                    user.Email = request.Email;
                    user.NormalizedEmail = request.Email.ToUpper();
                }

                if (!string.IsNullOrEmpty(request.UserName) && user.UserName != request.UserName)
                {
                    user.UserName = request.UserName;
                    user.NormalizedUserName = request.UserName.ToUpper();
                }

                user.PersonalNumber = request.PersonalNumber ?? user.PersonalNumber;
                user.Firstname = request.Firstname ?? user.Firstname;
                user.Lastname = request.Lastname ?? user.Lastname;
                user.Birthdate = request.Birthdate ?? user.Birthdate;
                user.Address = request.Address ?? user.Address;
                user.PhoneNumber = request.PhoneNumber ?? user.PhoneNumber;
                user.CityID = request.CityId ?? user.CityID;
                user.SettlementID = request.SettlementId ?? user.SettlementID;
                user.Active = request.Active ?? user.Active;

                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                {
                    return BadRequest(new { message = "User update failed", errors = result.Errors });
                }

                // Update roles if provided
                if (request.Roles != null && request.Roles.Any())
                {
                    var currentRoles = await _userManager.GetRolesAsync(user);

                    // Remove existing roles
                    if (currentRoles.Any())
                    {
                        await _userManager.RemoveFromRolesAsync(user, currentRoles);
                    }

                    // Add new roles
                    foreach (var roleName in request.Roles)
                    {
                        var role = await _roleManager.FindByNameAsync(roleName);
                        if (role != null)
                        {
                            await _userManager.AddToRoleAsync(user, role.Name);
                        }
                    }
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating user with ID: {id}");
                return StatusCode(500, new { message = $"An error occurred while updating user with ID: {id}", error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            try
            {
                _logger.LogInformation($"Deleting user with ID: {id}");

                var user = await _userManager.FindByIdAsync(id);
                if (user == null)
                {
                    return NotFound();
                }

                // Check if this is the last administrator
                if (await _userManager.IsInRoleAsync(user, "Administrator") || await _userManager.IsInRoleAsync(user, "Admin"))
                {
                    var adminUsers = await _userManager.GetUsersInRoleAsync("Administrator");
                    adminUsers = adminUsers.Concat(await _userManager.GetUsersInRoleAsync("Admin")).ToList();

                    if (adminUsers.Count <= 1)
                    {
                        return BadRequest(new { message = "Cannot delete the last administrator account" });
                    }
                }

                var result = await _userManager.DeleteAsync(user);
                if (!result.Succeeded)
                {
                    return BadRequest(new { message = "User deletion failed", errors = result.Errors });
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting user with ID: {id}");
                return StatusCode(500, new { message = $"An error occurred while deleting user with ID: {id}", error = ex.Message });
            }
        }

        [HttpGet("roles")]
        public async Task<ActionResult<IEnumerable<RoleDTO>>> GetRoles()
        {
            try
            {
                _logger.LogInformation("Fetching all roles");

                var roles = await _db.AspNetRoles
                    .Select(r => new RoleDTO
                    {
                        Id = r.Id,
                        Name = r.Name,
                        NameSq = r.NameSq,
                        NameEn = r.NameEn,
                        Description = r.Description
                    })
                    .ToListAsync();

                return roles;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting roles");
                return StatusCode(500, new { message = "An error occurred while fetching roles", error = ex.Message });
            }
        }

        [HttpPost("{id}/reset-password")]
        public async Task<IActionResult> ResetPassword(string id, Models.User.ResetPasswordRequest request)
        {
            try
            {
                _logger.LogInformation($"Resetting password for user with ID: {id}");

                var user = await _userManager.FindByIdAsync(id);
                if (user == null)
                {
                    return NotFound();
                }

                // Remove existing password
                await _userManager.RemovePasswordAsync(user);

                // Add new password
                var result = await _userManager.AddPasswordAsync(user, request.NewPassword);

                if (!result.Succeeded)
                {
                    return BadRequest(new { message = "Password reset failed", errors = result.Errors });
                }

                // Update password expiration
                user.PasswordExpires = DateTime.Now.AddMonths(3);
                await _userManager.UpdateAsync(user);

                return Ok(new { message = "Password reset successful" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error resetting password for user with ID: {id}");
                return StatusCode(500, new { message = $"An error occurred while resetting password for user with ID: {id}", error = ex.Message });
            }
        }
    }
}