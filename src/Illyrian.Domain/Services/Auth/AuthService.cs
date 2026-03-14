using Illyrian.Domain.Entities;
using Illyrian.Persistence.General;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Illyrian.Domain.Services.Auth;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly ITokenService _tokenService;

    public AuthService(
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager,
        ITokenService tokenService)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _tokenService = tokenService;
    }

    public async Task<AuthResult> LoginAsync(string email, string password)
    {
        var appUser = await _userManager.FindByEmailAsync(email);
        if (appUser == null || !await _userManager.CheckPasswordAsync(appUser, password))
        {
            return new AuthResult { Succeeded = false, Message = "Invalid email or password" };
        }

        if (appUser.Active == false)
        {
            return new AuthResult { Succeeded = false, Message = "The account is inactive" };
        }

        if (appUser.PasswordExpires < DateTime.Now)
        {
            return new AuthResult { Succeeded = false, Message = "Password has expired. Please reset your password." };
        }

        var userRoles = await _userManager.GetRolesAsync(appUser);
        var domainUser = MapToUser(appUser);
        var (token, expiration) = await _tokenService.GenerateTokenAsync(domainUser, userRoles);

        return new AuthResult
        {
            Succeeded = true,
            Token = token,
            Expiration = expiration,
            UserId = appUser.Id,
            UserRole = userRoles.FirstOrDefault()
        };
    }

    public async Task<AuthResult> RegisterAsync(RegisterModel model)
    {
        var userExists = await _userManager.FindByEmailAsync(model.Email);
        if (userExists != null)
        {
            return new AuthResult { Succeeded = false, Message = "User already exists!" };
        }

        var user = new ApplicationUser
        {
            Email = model.Email,
            SecurityStamp = Guid.NewGuid().ToString(),
            UserName = model.Email,
            PersonalNumber = model.PersonalNumber,
            Firstname = model.Firstname,
            Lastname = model.Lastname,
            Birthdate = model.Birthdate,
            PasswordExpires = DateTime.Now.AddMonths(3),
            CityID = model.CityId,
            SettlementID = model.SettlementId,
            Address = model.Address,
            PhoneNumber = model.PhoneNumber,
            Active = true,
            AllowNotifications = true,
            AllowAdminNotification = true,
            EmailConfirmed = true,
            PhoneNumberConfirmed = true,
            TwoFactorEnabled = false,
            LockoutEnabled = false,
            AccessFailedCount = 0,
            InsertedDate = DateTime.Now
        };

        var result = await _userManager.CreateAsync(user, model.Password);
        if (!result.Succeeded)
        {
            return new AuthResult
            {
                Succeeded = false,
                Message = "User creation failed! Please check user details and try again.",
                Errors = result.Errors.Select(e => e.Description)
            };
        }

        if (!string.IsNullOrEmpty(model.Role))
        {
            var role = await _roleManager.FindByIdAsync(model.Role);
            if (role != null)
            {
                await _userManager.AddToRoleAsync(user, role.Name!);
            }
        }

        return new AuthResult { Succeeded = true, Message = "User created successfully!", UserId = user.Id };
    }

    public async Task<AuthResult> RegisterRoleAsync(string nameSq, string nameEn, string? description)
    {
        var roleExists = await _roleManager.RoleExistsAsync(nameEn);
        if (roleExists)
        {
            return new AuthResult { Succeeded = false, Message = "Role already exists!" };
        }

        var newRole = new ApplicationRole
        {
            Name = nameEn,
            Name_SQ = nameSq,
            Name_EN = nameEn,
            Description = description,
            NormalizedName = nameEn.ToUpper()
        };

        var result = await _roleManager.CreateAsync(newRole);
        if (!result.Succeeded)
        {
            return new AuthResult { Succeeded = false, Message = "Role creation failed!" };
        }

        return new AuthResult { Succeeded = true, Message = "Role created successfully!" };
    }

    public Task LogoutAsync()
    {
        return Task.CompletedTask;
    }

    public async Task<AuthResult> ResetPasswordAsync(string userId, string newPassword)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return new AuthResult { Succeeded = false, Message = "User not found" };
        }

        await _userManager.RemovePasswordAsync(user);
        var result = await _userManager.AddPasswordAsync(user, newPassword);

        if (!result.Succeeded)
        {
            return new AuthResult
            {
                Succeeded = false,
                Message = "Password reset failed",
                Errors = result.Errors.Select(e => e.Description)
            };
        }

        user.PasswordExpires = DateTime.Now.AddMonths(3);
        await _userManager.UpdateAsync(user);

        return new AuthResult { Succeeded = true, Message = "Password reset successful" };
    }

    public async Task<AuthResult> CreateUserAsync(CreateUserModel model)
    {
        var userExists = await _userManager.FindByEmailAsync(model.Email);
        if (userExists != null)
        {
            return new AuthResult { Succeeded = false, Message = "User with this email already exists" };
        }

        var user = new ApplicationUser
        {
            Email = model.Email,
            UserName = string.IsNullOrEmpty(model.UserName) ? model.Email : model.UserName,
            SecurityStamp = Guid.NewGuid().ToString(),
            PersonalNumber = model.PersonalNumber,
            Firstname = model.Firstname,
            Lastname = model.Lastname,
            Birthdate = model.Birthdate,
            PasswordExpires = DateTime.Now.AddMonths(3),
            CityID = model.CityId,
            SettlementID = model.SettlementId,
            Address = model.Address,
            PhoneNumber = model.PhoneNumber,
            Active = model.Active ?? true,
            AllowNotifications = true,
            AllowAdminNotification = true,
            EmailConfirmed = true,
            PhoneNumberConfirmed = true,
            TwoFactorEnabled = false,
            LockoutEnabled = false,
            AccessFailedCount = 0,
            InsertedDate = DateTime.Now
        };

        var result = await _userManager.CreateAsync(user, model.Password);
        if (!result.Succeeded)
        {
            return new AuthResult
            {
                Succeeded = false,
                Message = "User creation failed",
                Errors = result.Errors.Select(e => e.Description)
            };
        }

        if (model.Roles != null && model.Roles.Any())
        {
            foreach (var roleName in model.Roles)
            {
                var role = await _roleManager.FindByNameAsync(roleName);
                if (role != null)
                {
                    await _userManager.AddToRoleAsync(user, role.Name!);
                }
            }
        }
        else
        {
            var userRole = await _roleManager.FindByNameAsync("User");
            if (userRole != null)
            {
                await _userManager.AddToRoleAsync(user, userRole.Name!);
            }
        }

        return new AuthResult { Succeeded = true, Message = "User created successfully", UserId = user.Id };
    }

    public async Task<AuthResult> UpdateUserAsync(string userId, UpdateUserModel model)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return new AuthResult { Succeeded = false, Message = "User not found" };
        }

        if (!string.IsNullOrEmpty(model.Email) && user.Email != model.Email)
        {
            user.Email = model.Email;
            user.NormalizedEmail = model.Email.ToUpper();
        }

        if (!string.IsNullOrEmpty(model.UserName) && user.UserName != model.UserName)
        {
            user.UserName = model.UserName;
            user.NormalizedUserName = model.UserName.ToUpper();
        }

        user.PersonalNumber = model.PersonalNumber ?? user.PersonalNumber;
        user.Firstname = model.Firstname ?? user.Firstname;
        user.Lastname = model.Lastname ?? user.Lastname;
        user.Birthdate = model.Birthdate ?? user.Birthdate;
        user.Address = model.Address ?? user.Address;
        user.PhoneNumber = model.PhoneNumber ?? user.PhoneNumber;
        user.CityID = model.CityId ?? user.CityID;
        user.SettlementID = model.SettlementId ?? user.SettlementID;
        user.Active = model.Active ?? user.Active;

        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            return new AuthResult
            {
                Succeeded = false,
                Message = "User update failed",
                Errors = result.Errors.Select(e => e.Description)
            };
        }

        if (model.Roles != null && model.Roles.Any())
        {
            var currentRoles = await _userManager.GetRolesAsync(user);
            if (currentRoles.Any())
            {
                await _userManager.RemoveFromRolesAsync(user, currentRoles);
            }

            foreach (var roleName in model.Roles)
            {
                var role = await _roleManager.FindByNameAsync(roleName);
                if (role != null)
                {
                    await _userManager.AddToRoleAsync(user, role.Name!);
                }
            }
        }

        return new AuthResult { Succeeded = true, Message = "User updated successfully" };
    }

    public async Task<AuthResult> DeleteUserAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return new AuthResult { Succeeded = false, Message = "User not found" };
        }

        if (await _userManager.IsInRoleAsync(user, "Administrator") || await _userManager.IsInRoleAsync(user, "Admin"))
        {
            var adminUsers = (await _userManager.GetUsersInRoleAsync("Administrator"))
                .Concat(await _userManager.GetUsersInRoleAsync("Admin"))
                .Distinct()
                .ToList();

            if (adminUsers.Count <= 1)
            {
                return new AuthResult { Succeeded = false, Message = "Cannot delete the last administrator account" };
            }
        }

        var result = await _userManager.DeleteAsync(user);
        if (!result.Succeeded)
        {
            return new AuthResult
            {
                Succeeded = false,
                Message = "User deletion failed",
                Errors = result.Errors.Select(e => e.Description)
            };
        }

        return new AuthResult { Succeeded = true, Message = "User deleted successfully" };
    }

    public async Task<Entities.User?> FindUserByIdAsync(string id)
    {
        var appUser = await _userManager.FindByIdAsync(id);
        if (appUser == null) return null;

        var user = MapToUser(appUser);
        var roles = await _userManager.GetRolesAsync(appUser);
        user.Roles = roles.Select(r => new Entities.Role { Name = r }).ToList();
        return user;
    }

    public async Task<Entities.User?> FindUserByEmailAsync(string email)
    {
        var appUser = await _userManager.FindByEmailAsync(email);
        return appUser == null ? null : MapToUser(appUser);
    }

    public async Task<IList<string>> GetUserRolesAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return new List<string>();
        return await _userManager.GetRolesAsync(user);
    }

    public async Task<IEnumerable<Entities.Role>> GetRolesAsync()
    {
        var roles = await _roleManager.Roles.ToListAsync();
        return roles.Select(r => new Entities.Role
        {
            Id = r.Id,
            Name = r.Name,
            NameSq = r.Name_SQ,
            NameEn = r.Name_EN,
            Description = r.Description
        });
    }

    public async Task<IEnumerable<Entities.User>> GetAllUsersWithRolesAsync()
    {
        var appUsers = await _userManager.Users.ToListAsync();
        var users = new List<Entities.User>();

        foreach (var appUser in appUsers)
        {
            var user = MapToUser(appUser);
            var roles = await _userManager.GetRolesAsync(appUser);
            user.Roles = roles.Select(r => new Entities.Role { Name = r }).ToList();
            users.Add(user);
        }

        return users;
    }

    private static Entities.User MapToUser(ApplicationUser appUser)
    {
        return new Entities.User
        {
            Id = appUser.Id,
            PersonalNumber = appUser.PersonalNumber,
            Firstname = appUser.Firstname,
            Lastname = appUser.Lastname,
            Email = appUser.Email,
            UserName = appUser.UserName,
            PhoneNumber = appUser.PhoneNumber,
            Birthdate = appUser.Birthdate,
            PasswordExpires = appUser.PasswordExpires,
            AllowNotifications = appUser.AllowNotifications,
            AllowAdminNotification = appUser.AllowAdminNotification,
            CityId = appUser.CityID,
            SettlementId = appUser.SettlementID,
            Address = appUser.Address,
            GoogleToken = appUser.GoogleToken,
            Active = appUser.Active,
            InsertedFrom = appUser.InsertedFrom,
            InsertedDate = appUser.InsertedDate,
            ImageProfile = appUser.ImageProfile,
            LanguageId = appUser.LanguageID
        };
    }
}
