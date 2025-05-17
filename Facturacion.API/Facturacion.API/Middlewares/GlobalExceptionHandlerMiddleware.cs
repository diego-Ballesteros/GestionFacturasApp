using System.Net;
using System.Text.Json;

namespace Facturacion.API.Middlewares;

public class GlobalExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

    public GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex) 
        {         
            _logger.LogError(ex, "Unhandled exception occurred. Path: {Path}, Method: {Method}, Error: {ErrorMessage}",
                httpContext.Request.Path, httpContext.Request.Method, ex.Message);

            
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError; 
            
            var errorResponse = new
            {
                StatusCode = httpContext.Response.StatusCode,
                Message = "An unexpected internal server error has occurred. Please try again later or contact support.",
                TraceId = httpContext.TraceIdentifier 
            };
            
            var jsonResponse = JsonSerializer.Serialize(errorResponse, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase 
            });

            await httpContext.Response.WriteAsync(jsonResponse);
        }
    }
}
