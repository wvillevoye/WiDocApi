using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace WiDocApi_Blazor.WiDocApi.Helpers
{
    public class ApiKeyAuthFilter : IEndpointFilter
    {
        private readonly string _validApiKey;
        private const string ApiKeyHeaderName = "X-Api-Key";

        public ApiKeyAuthFilter(IConfiguration configuration)
        {
            _validApiKey = configuration["ApiSettings:ValidApiKey"]!;
        }

        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            var httpContext = context.HttpContext;

            if (!httpContext.Request.Headers.TryGetValue(ApiKeyHeaderName, out var extractedApiKey))
            {
                httpContext.Response.StatusCode = 401;
                return Results.Unauthorized();
            }

            if (_validApiKey != extractedApiKey)
            {
                httpContext.Response.StatusCode = 403;
                return Results.Forbid();
            }

            var result = await next(context);
            return result ?? Results.Problem("An unexpected error occurred.");
        }
    }
}

