using Microsoft.Extensions.Hosting;
using System;
using System.Net;
using System.Text.Json;
using WebApiEcomm.API.Helper;

namespace WebApiEcomm.API.Middleware
{
    public class ExceptionsMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHostEnvironment _hostEnvironment;
        public ExceptionsMiddleware(RequestDelegate next, IHostEnvironment hostEnvironment)
        {
            _next = next;
            _hostEnvironment = hostEnvironment
                ;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
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
    }
}
