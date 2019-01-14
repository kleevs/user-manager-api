using Microsoft.AspNetCore.Http;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Core.Authentication
{
    public class UnauthorizeExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public UnauthorizeExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            } catch (UnauthorizedAccessException)
            {
                context.Response.StatusCode = 401;
                var buffer = Encoding.Default.GetBytes("Access denied.");
                context.Response.Body.Write(buffer, 0, buffer.Length);
            }
        }
    }
}
