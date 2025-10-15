using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Library_App.Middleware
{
    public class BooksLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<BooksLoggingMiddleware> _logger;
        public BooksLoggingMiddleware(RequestDelegate next, ILogger<BooksLoggingMiddleware> logger)
        { _next = next; _logger = logger; }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path.StartsWithSegments("/books", out var remaining))
            {
                _logger.LogInformation("[BooksMiddleware] {Method} {Path}{Remaining}", context.Request.Method, "/books", remaining);
                // Write to console (may not appear in VS Output depending on profile)
                Console.WriteLine($"[BooksMiddleware] {context.Request.Method} {context.Request.Path}");
                Console.WriteLine("Mark");
                // Always visible via logger
                _logger.LogInformation("Mark");
            }
            await _next(context);
        }
    }
}
