using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using IllyrianAPI.Data.Core;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using IllyrianAPI.Data.General;
using IllyrianAPI.Models.Auth;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;

namespace IllyrianAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : BaseController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IllyrianContext _db;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IConfiguration _configuration;

        public AuthController(
            IllyrianContext db,
            RoleManager<ApplicationRole> roleManager,
            UserManager<ApplicationUser> userManager,
            IConfiguration configuration
        ) : base(db, userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _db = db;
            _configuration = configuration;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] Login model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            var userLanguage = await _db.Language.Where(l => l.LanguageId == user.LanguageID).Select(l => l.NameEn).FirstOrDefaultAsync();

            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
            {
                return Unauthorized(new { message = "Invalid email or password" });
            }

            if (user.Active == false)
            {
                return Unauthorized(new { message = "The account is inactive" });
            }

            if (user.PasswordExpires < DateTime.Now)
            {
                return Unauthorized(new { message = "Password has expired. Please reset your password." });
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                userid = user.Id.ToString(),
                userrole = userRoles.FirstOrDefault(),
                userlanguage = userLanguage,
                expiration = token.ValidTo
            });
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] Register model)
        {
            var userExists = await _userManager.FindByEmailAsync(model.Email);
            if (userExists != null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", Message = "User already exists!" });
            }

            ApplicationUser user = new ApplicationUser()
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
                return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", Message = "User creation failed! Please check user details and try again." });

            if (!string.IsNullOrEmpty(model.Role))
            {
                var role = await _roleManager.FindByIdAsync(model.Role);
                if (role != null)
                {
                    await _userManager.AddToRoleAsync(user, role.Name);
                }
            }

            return Ok(new { Status = "Success", Message = "User created successfully!" });
        }

        [HttpPost("register-role")]
        //[Authorize(Roles = "Admin")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterRole([FromBody] RegisterRole model)
        {
            var roleExists = await _roleManager.RoleExistsAsync(model.Name_EN);
            if (roleExists)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", Message = "Role already exists!" });
            }

            ApplicationRole newRole = new ApplicationRole
            {
                Name = model.Name_EN,
                Name_SQ = model.Name_SQ,
                Name_EN = model.Name_EN,
                Description = model.Description,
                NormalizedName = model.Name_EN.ToUpper()
            };

            var result = await _roleManager.CreateAsync(newRole);
            if (!result.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", Message = "Role creation failed! Please check role details and try again." });
            }

            return Ok(new { Status = "Success", Message = "Role created successfully!" });
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);


            return Ok(new { message = "Logged out successfully" });
        }

        [HttpGet("status")]
        public IActionResult GetAuthStatus()
        {
            return Ok(new { isAuthenticated = User.Identity.IsAuthenticated });
        }

        [Authorize]
        [HttpGet("username")]
        public async Task<IActionResult> GetUsername()
        {
            var user = await _userManager.GetUserAsync(User);
            return Ok(new { username = user.UserName });
        }
    }
}