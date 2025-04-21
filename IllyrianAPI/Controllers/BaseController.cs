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
            Logs log = null;
            try
            {
                // Initialize log entry with default values
                log = new Logs
                {
                    Action = context.ActionDescriptor.RouteValues["action"]?.ToString(),
                    Controller = context.ActionDescriptor.RouteValues["controller"]?.ToString(),
                    HttpMethod = context.HttpContext.Request.Method,
                    Ip = context.HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                    Url = context.HttpContext.Request.GetDisplayUrl(),
                    InsertedDate = DateTime.Now,
                    Error = false
                };

                // Determine if user is authenticated and set UserId accordingly
                if (context.HttpContext.User.Identity?.IsAuthenticated == true)
                {
                    log.UserId = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
                                 context.HttpContext.User.FindFirst("sub")?.Value;
                }
                else
                {
                    // For unauthenticated requests, set UserId to null - this is now allowed in the DB
                    log.UserId = null;
                }

                // Check if this is a sensitive endpoint like login
                bool isSensitiveEndpoint =
                    context.ActionDescriptor.RouteValues["action"]?.ToLower() == "login" ||
                    context.ActionDescriptor.RouteValues["controller"]?.ToLower() == "auth" &&
                    context.ActionDescriptor.RouteValues["action"]?.ToLower() == "register";

                // Log request content except for sensitive actions (to avoid logging passwords)
                if (context.ActionArguments.Any() && !isSensitiveEndpoint)
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
                else if (isSensitiveEndpoint)
                {
                    // For sensitive endpoints, log that we intentionally skipped content
                    log.FormContent = "Content not logged for security reasons (sensitive endpoint)";
                }

                // Save the initial log entry to capture the request
                await _db.Logs.AddAsync(log);
                await _db.SaveChangesAsync();

                // Execute the action
                var result = await next();

                // Update the log with response information
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
                if (result.Exception != null && !result.ExceptionHandled)
                {
                    log.Error = true;
                    log.Exception = JsonSerializer.Serialize(new
                    {
                        Message = result.Exception.Message,
                        StackTrace = result.Exception.StackTrace,
                        InnerException = result.Exception.InnerException?.Message
                    });
                }

                // Update the log entry
                _db.Logs.Update(log);
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // If the filter itself fails, log to debug at minimum
                System.Diagnostics.Debug.WriteLine($"Error in OnActionExecutionAsync: {ex.Message}");

                // Try to record the error in the log
                if (log != null)
                {
                    try
                    {
                        log.Error = true;
                        log.Exception = JsonSerializer.Serialize(new
                        {
                            Message = $"Logging filter failed: {ex.Message}",
                            StackTrace = ex.StackTrace,
                            InnerException = ex.InnerException?.Message
                        });

                        _db.Logs.Update(log);
                        await _db.SaveChangesAsync();
                    }
                    catch
                    {
                        // Last resort - can't do much else if even error logging fails
                        System.Diagnostics.Debug.WriteLine($"Failed to log the error: {ex.Message}");
                    }
                }

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
                // Create a new log entry for errors that might occur outside the filter
                var log = new Logs
                {
                    Action = HttpContext?.Request?.RouteValues["action"]?.ToString(),
                    Controller = HttpContext?.Request?.RouteValues["controller"]?.ToString(),
                    HttpMethod = HttpContext?.Request?.Method ?? "UNKNOWN",
                    Ip = HttpContext?.Connection?.RemoteIpAddress?.ToString() ?? "unknown",
                    Url = HttpContext?.Request != null ? HttpContext.Request.GetDisplayUrl() : "unknown",
                    InsertedDate = DateTime.Now,
                    Error = true,
                    Exception = JsonSerializer.Serialize(new
                    {
                        Message = ex.Message,
                        StackTrace = ex.StackTrace,
                        InnerException = ex.InnerException?.Message
                    })
                };

                // Set the user ID if available, otherwise leave it as null
                if (User?.Identity?.IsAuthenticated == true)
                {
                    log.UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
                                User.FindFirst("sub")?.Value;
                }
                else
                {
                    log.UserId = null; // Will be null for unauthenticated requests
                }

                await _db.Logs.AddAsync(log);
                await _db.SaveChangesAsync();
            }
            catch (Exception logEx)
            {
                // If logging fails, at least write to console/debug
                System.Diagnostics.Debug.WriteLine($"Failed to log error: {ex.Message}. Logging error: {logEx.Message}");
            }
        }
    }
}