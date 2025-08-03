using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Hosting;
using System;
using System.Diagnostics.Metrics;
using System.Net;
using System.Text.Json;
using WebApiEcomm.API.Helper;

namespace WebApiEcomm.API.Middleware
{
    public class ExceptionsMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHostEnvironment _hostEnvironment;
        private readonly IMemoryCache memoryCache;
        private readonly TimeSpan _timeSpan = TimeSpan.FromSeconds(30);
        public ExceptionsMiddleware(RequestDelegate next, IHostEnvironment hostEnvironment, IMemoryCache memoryCache)
        {
            _next = next;
            _hostEnvironment = hostEnvironment;
            this.memoryCache = memoryCache;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                ApplySecurityHeaders(context);

                if (!IsRequestAllowed(context))
                {
                    context.Response.StatusCode = (int)HttpStatusCode.TooManyRequests;
                    context.Response.ContentType = "application/json";

                    var response = new ApiExceptions(
                        (int)HttpStatusCode.TooManyRequests,
                        "Too many requests. Please try again later."
                    );

                    await context.Response.WriteAsJsonAsync(response);
                    return; 
                }

                await _next(context); 
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";

                var response = _hostEnvironment.IsDevelopment()
                    ? new ApiExceptions((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace)
                    : new ApiExceptions((int)HttpStatusCode.InternalServerError, ex.Message);

                var json = JsonSerializer.Serialize(response);
                await context.Response.WriteAsync(json);
            }
        }
        //Rate Limit
        private bool IsRequestAllowed(HttpContext context)
        {
            var ip = context.Connection.RemoteIpAddress.ToString();
            var CachKey = $"Rate: {ip}";
            var dateNow = DateTime.UtcNow;

            var (timesStamp, count) = memoryCache.GetOrCreate(CachKey, entry =>
            { 
             entry.AbsoluteExpirationRelativeToNow = _timeSpan;
                return (timesStamp: dateNow, count: 0);
            });
            // Check if the request is allowed based on the timestamp and count
            if (dateNow - timesStamp > _timeSpan)
            {
                memoryCache.Set(CachKey, (dateNow, 1));
                return true;
            }
            else if (count < 10)
            {
                memoryCache.Set(CachKey, (timesStamp, count + 1));
                return true;
            }
            else
            {
                return false;
            }
        }

        private void ApplySecurityHeaders(HttpContext context)
        {
            context.Response.Headers.Add("X-Content-Type-Options","nosniff");
            context.Response.Headers.Add("X-Frame-Options", "DENY");
            context.Response.Headers.Add("X-XSS-Protection", "1; mode=block");
            context.Response.Headers.Add("Referrer-Policy", "no-referrer");
            context.Response.Headers.Add("Content-Security-Policy", "default-src 'self'"); 
        }
    }
}
