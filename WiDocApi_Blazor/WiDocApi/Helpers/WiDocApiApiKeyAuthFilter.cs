using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using WiDocApi_Blazor.WiDocApi.Models;

namespace WiDocApi_Blazor.WiDocApi.Helpers
{
    public class WiDocApiApiKeyAuthFilter : IEndpointFilter
    {
        private readonly string _validApiKey;
        private WiDocApiApikeySettings? _apikeySettings;

        public WiDocApiApiKeyAuthFilter(WiDocApiApikeySettings apikeySettings)
        {
            _apikeySettings = apikeySettings;

            _validApiKey = _apikeySettings!.ApiKey!;
        }

        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            var httpContext = context.HttpContext;

            if (!httpContext.Request.Headers.TryGetValue(_apikeySettings!.ApiKeyHeaderName!, out var extractedApiKey))
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

