using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace LooperCorp.AppServer;

internal sealed class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception ex, CancellationToken cancellationToken)
    {
        _logger.LogError("Error: {Message}", ex.Message);

        var problemDetail = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "",
            Detail = ex.Message // todo: do not return exception detail in Production
        };
        httpContext.Response.StatusCode = problemDetail.Status.Value;
        httpContext.Response.WriteAsJsonAsync(problemDetail);

        // true to indicate the error is properly handle,
        // false to fallback to old behavior
        return ValueTask.FromResult(true);
    }
}
