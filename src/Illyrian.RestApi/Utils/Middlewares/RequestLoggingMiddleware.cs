using System.Security.Claims;
using System.Text.Json;
using Illyrian.Domain.Entities;
using Illyrian.Domain.Repositories;
using Microsoft.AspNetCore.Http.Extensions;

namespace Illyrian.RestApi.Utils.Middlewares;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;

    public RequestLoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, ILogRepository logRepository)
    {
        var log = new LogEntry
        {
            Controller = context.Request.RouteValues["controller"]?.ToString() ?? string.Empty,
            Action = context.Request.RouteValues["action"]?.ToString() ?? string.Empty,
            HttpMethod = context.Request.Method,
            Ip = context.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            Url = context.Request.GetDisplayUrl(),
            InsertedDate = DateTime.Now,
            Error = false
        };

        if (context.User.Identity?.IsAuthenticated == true)
        {
            log.UserId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                         ?? context.User.FindFirst("sub")?.Value;
        }

        bool isSensitiveEndpoint =
            string.Equals(log.Action, "login", StringComparison.OrdinalIgnoreCase) ||
            (string.Equals(log.Controller, "auth", StringComparison.OrdinalIgnoreCase) &&
             string.Equals(log.Action, "register", StringComparison.OrdinalIgnoreCase));

        if (isSensitiveEndpoint)
        {
            log.FormContent = "Content not logged for security reasons (sensitive endpoint)";
        }
        else if (context.Request.ContentLength > 0 && context.Request.Body.CanRead)
        {
            context.Request.EnableBuffering();
            using var reader = new StreamReader(context.Request.Body, leaveOpen: true);
            var body = await reader.ReadToEndAsync();
            context.Request.Body.Position = 0;
            if (!string.IsNullOrWhiteSpace(body))
            {
                log.FormContent = body.Length > 4000 ? body[..4000] : body;
            }
        }

        bool logInserted = false;
        try
        {
            await logRepository.AddAsync(log);
            await logRepository.SaveChangesAsync();
            logInserted = true;
        }
        catch { /* don't block request if logging fails */ }

        try
        {
            await _next(context);

            if (logInserted)
            {
                log.Response = $"StatusCode: {context.Response.StatusCode}";
                logRepository.Update(log);
                await logRepository.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            log.Error = true;
            log.Exception = JsonSerializer.Serialize(new
            {
                Message = ex.Message,
                StackTrace = ex.StackTrace,
                InnerException = ex.InnerException?.Message
            });

            if (logInserted)
            {
                try
                {
                    logRepository.Update(log);
                    await logRepository.SaveChangesAsync();
                }
                catch { }
            }

            throw;
        }
    }
}
