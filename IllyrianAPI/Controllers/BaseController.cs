using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using IllyrianAPI.Data;
using IllyrianAPI.Data.Core;
using IllyrianAPI.Data.General;
using System.Globalization;

namespace IllyrianAPI.Controllers
{
    [Authorize]
    public class BaseController : ControllerBase
    {
        protected readonly UserManager<ApplicationUser> _userManager;
        protected readonly IllyrianContext _db;
        protected ApplicationUser CurrentUser => _userManager.GetUserAsync(User).Result;

        public BaseController(IllyrianContext db,
                              UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        // You can add common methods here that all controllers might need
        protected async Task<ApplicationUser> GetCurrentUserAsync()
        {
            return await _userManager.GetUserAsync(User);
        }

        [NonAction]
        public static string FormatDuration(int days)
        {
            if (days % 365 == 0)
            {
                int years = days / 365;
                return $"{years} {(years == 1 ? "year" : "years")}";
            }
            else if (days % 30 == 0)
            {
                int months = days / 30;
                return $"{months} {(months == 1 ? "month" : "months")}";
            }
            else
            {
                return $"{days} days";
            }
        }

        [NonAction]
        public static string FormatPrice(decimal price)
        {
            return price.ToString("C", CultureInfo.CreateSpecificCulture("en-US"));
        }

        [NonAction]
        public static string FormatDate(DateTime date)
        {
            return date.ToString("yyyy-MM-dd");
        }
    }
}