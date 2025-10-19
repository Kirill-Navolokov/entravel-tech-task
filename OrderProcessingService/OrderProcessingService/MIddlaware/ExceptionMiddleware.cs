using System.Net;
using OrderProcessingService.Dtos;

namespace OrderProcessingService.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
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
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            var error = new ErrorDto("Something went wrong", HttpStatusCode.InternalServerError);

            context.Response.Headers.Clear();
            context.Response.StatusCode = (int)error.StatusCode;

            await context.Response.WriteAsJsonAsync(error);
        }
    }
}