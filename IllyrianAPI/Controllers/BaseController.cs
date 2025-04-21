using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http.Extensions;
using IllyrianAPI.Data;
using IllyrianAPI.Data.Core;
using IllyrianAPI.Data.General;
using System.Globalization;
using System.Text.Json;
using System.Security.Claims;

namespace IllyrianAPI.Controllers
{
    [Authorize]
    public class BaseController : ControllerBase, IAsyncActionFilter
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

        [NonAction]
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            try
            {
                // Safely get user ID - this works even for anonymous/login requests
                string userId = "anonymous";

                // Try to get from claims (for authenticated requests)
                if (context.HttpContext.User.Identity?.IsAuthenticated == true)
                {
                    userId = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
                             context.HttpContext.User.FindFirst("sub")?.Value ??
                             userId;
                }

                // For anonymous endpoints like login/register, use a default value
                bool isAnonymousEndpoint =
                    context.ActionDescriptor.EndpointMetadata.Any(m => m is AllowAnonymousAttribute) ||
                    context.ActionDescriptor.RouteValues["action"]?.ToLower() == "login" ||
                    context.ActionDescriptor.RouteValues["action"]?.ToLower() == "register";

                // Create log entry
                var log = new Logs
                {
                    Action = context.ActionDescriptor.RouteValues["action"]?.ToString(),
                    Controller = context.ActionDescriptor.RouteValues["controller"]?.ToString(),
                    HttpMethod = context.HttpContext.Request.Method,
                    Ip = context.HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                    Url = context.HttpContext.Request.GetDisplayUrl(),
                    UserId = userId,
                    InsertedDate = DateTime.Now,
                    Error = false
                };

                // Log request content for non-login actions (to avoid logging passwords)
                if (context.ActionArguments.Any() &&
                    !context.ActionDescriptor.RouteValues["action"]?.ToString()?.Equals("login", StringComparison.OrdinalIgnoreCase) == true)
                {
                    try
                    {
                        log.FormContent = JsonSerializer.Serialize(context.ActionArguments, new JsonSerializerOptions
                        {
                            WriteIndented = true,
                            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                        });
                    }
                    catch (Exception ex)
                    {
                        // If serialization fails, log that fact
                        log.FormContent = $"Failed to serialize request: {ex.Message}";
                    }
                }

                // Execute the action
                var result = await next();

                // Log response
                if (result.Result != null)
                {
                    try
                    {
                        // Handle different result types
                        if (result.Result is ObjectResult objResult)
                        {
                            // Don't log tokens
                            if (objResult.Value != null &&
                                objResult.Value.GetType().GetProperty("token") != null)
                            {
                                log.Response = "Response contains authentication token - not logged for security";
                            }
                            else
                            {
                                log.Response = JsonSerializer.Serialize(objResult.Value);
                            }
                        }
                        else if (result.Result is JsonResult jsonResult)
                        {
                            log.Response = JsonSerializer.Serialize(jsonResult.Value);
                        }
                        else if (result.Result is ContentResult contentResult)
                        {
                            log.Response = contentResult.Content;
                        }
                        // Add more specific handling as needed
                    }
                    catch (Exception ex)
                    {
                        // If serialization fails, log that fact
                        log.Response = $"Failed to serialize response: {ex.Message}";
                    }
                }

                // Log exception if present
                if (result.Exception != null)
                {
                    log.Error = true;
                    log.Exception = JsonSerializer.Serialize(new
                    {
                        Message = result.Exception.Message,
                        StackTrace = result.Exception.StackTrace,
                        InnerException = result.Exception.InnerException?.Message
                    });
                }

                // Save log entry - make sure to use a fresh DbContext if needed
                try
                {
                    await _db.Logs.AddAsync(log);
                    await _db.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    // If logging fails, at least write to console/debug
                    System.Diagnostics.Debug.WriteLine($"Failed to save log: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                // If the filter itself fails, log to debug at minimum
                System.Diagnostics.Debug.WriteLine($"Error in OnActionExecutionAsync: {ex.Message}");
                // Continue executing the action even if logging fails
                if (next != null)
                {
                    await next();
                }
            }
        }

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

        [NonAction]
        public async Task LogError(Exception ex)
        {
            try
            {
                // Skip logging if the user is not authenticated
                if (User.Identity?.IsAuthenticated != true)
                {
                    System.Diagnostics.Debug.WriteLine($"Error occurred but user is not authenticated: {ex.Message}");
                    return;
                }

                // Get the user ID - only for authenticated users
                string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
                                User.FindFirst("sub")?.Value;

                // If we can't get a user ID, skip logging to avoid FK constraint violation
                if (string.IsNullOrEmpty(userId))
                {
                    System.Diagnostics.Debug.WriteLine($"Error occurred but couldn't identify user: {ex.Message}");
                    return;
                }

                var log = new Logs
                {
                    Action = HttpContext?.Request?.RouteValues["action"]?.ToString(),
                    Controller = HttpContext?.Request?.RouteValues["controller"]?.ToString(),
                    HttpMethod = HttpContext?.Request?.Method ?? "UNKNOWN",
                    Ip = HttpContext?.Connection?.RemoteIpAddress?.ToString() ?? "unknown",
                    Url = HttpContext?.Request != null ? HttpContext.Request.GetDisplayUrl() : "unknown",
                    UserId = userId,
                    InsertedDate = DateTime.Now,
                    Error = true,
                    Exception = JsonSerializer.Serialize(new
                    {
                        Message = ex.Message,
                        StackTrace = ex.StackTrace,
                        InnerException = ex.InnerException?.Message
                    })
                };

                await _db.Logs.AddAsync(log);
                await _db.SaveChangesAsync();
            }
            catch
            {
                // If logging fails, at least write to console/debug
                System.Diagnostics.Debug.WriteLine($"Failed to log error: {ex.Message}");
            }
        }
    }
}