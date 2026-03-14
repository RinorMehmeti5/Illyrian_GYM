using System.Text.Json;
using Illyrian.Domain.Exceptions;

namespace Illyrian.RestApi.Utils.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (NotFoundException ex)
        {
            _logger.LogWarning(ex, "Resource not found");
            await WriteResponse(context, StatusCodes.Status404NotFound, ex.Message);
        }
        catch (ValidationException ex)
        {
            _logger.LogWarning(ex, "Validation error");
            await WriteResponse(context, StatusCodes.Status400BadRequest, ex.Message, ex.Errors);
        }
        catch (DomainException ex)
        {
            _logger.LogWarning(ex, "Domain error");
            await WriteResponse(context, StatusCodes.Status400BadRequest, ex.Message);
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning(ex, "Unauthorized access");
            await WriteResponse(context, StatusCodes.Status401Unauthorized, ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred");
            await WriteResponse(context, StatusCodes.Status500InternalServerError,
                "An error occurred while processing your request", error: ex.Message);
        }
    }

    private static async Task WriteResponse(HttpContext context, int statusCode, string message,
        IEnumerable<string>? errors = null, string? error = null)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        var response = new { message, errors, error };
        await context.Response.WriteAsync(JsonSerializer.Serialize(response, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
        }));
    }
}
