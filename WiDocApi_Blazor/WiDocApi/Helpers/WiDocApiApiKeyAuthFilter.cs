using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using WiDocApi_Blazor.WiDocApi.Models;

namespace WiDocApi_Blazor.WiDocApi.Helpers
{

    public class WiDocApiApiKeyAuthFilter : IEndpointFilter
    {
        private readonly List<string> _validApiKeys;

        private WiDocApiApikeySettings? _apikeySettings;

        public WiDocApiApiKeyAuthFilter(WiDocApiApikeySettings apikeySettings)
        {
            _apikeySettings = apikeySettings;
            _validApiKeys = _apikeySettings.ApiKeys!;
        }

        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            HttpContext httpContext = context.HttpContext;
            if (!httpContext.Request.Headers.TryGetValue(_apikeySettings!.ApiKeyHeaderName!, out var extractedApiKey))
            {
                httpContext.Response.StatusCode = 401;
                return Results.Unauthorized();
            }

            if (!_validApiKeys.Contains(extractedApiKey!))
            {
                httpContext.Response.StatusCode = 403;
                return Results.Forbid();
            }

            return (await next(context)) ?? Results.Problem("An unexpected error occurred.");
        }
    }



}

