using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace ProductService.Middleware
{
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;
        private const string ApiKeyHeader = "X-API-KEY";
        private const string ExpectedApiKey = "super-secreta-productos"; // Cambia esto según tu clave real

        public ApiKeyMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Request.Headers.TryGetValue(ApiKeyHeader, out var extractedApiKey))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("API Key no encontrada");
                return;
            }

            if (extractedApiKey != ExpectedApiKey)
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("API Key inválida");
                return;
            }

            await _next(context);
        }
    }
}
