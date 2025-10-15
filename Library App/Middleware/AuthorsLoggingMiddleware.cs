using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Library_App.Middleware
{
    public class AuthorsLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<AuthorsLoggingMiddleware> _logger;
        public AuthorsLoggingMiddleware(RequestDelegate next, ILogger<AuthorsLoggingMiddleware> logger)
        { _next = next; _logger = logger; }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path.StartsWithSegments("/authors", out var remaining))
            {
                _logger.LogInformation("[AuthorsMiddleware] {Method} {Path}{Remaining}", context.Request.Method, "/authors", remaining);
                Console.WriteLine($"[AuthorsMiddleware] {context.Request.Method} {context.Request.Path}");
                Console.WriteLine("Mark");
            }
            await _next(context);
        }
    }
}
