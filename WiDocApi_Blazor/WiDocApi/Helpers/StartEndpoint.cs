using Microsoft.Extensions.Configuration;
using System.Collections.Concurrent;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Text;
using WiDocApi_Blazor.WiDocApi.Models;
using WiDocApi_Blazor.WiDocApi.Services;
using Microsoft.AspNetCore.DataProtection.KeyManagement;

public class StartEndpoint
{
    private readonly IConfiguration _configuration;
    private readonly HttpClient _http;
    private readonly SessionStorageService _sessionService;
    private readonly WiDocApiApikeySettings _apikeySettings;
    private readonly ConcurrentDictionary<string, CachedApiResponse> _apiResponseCache = new ConcurrentDictionary<string, CachedApiResponse>();

    public StartEndpoint(IConfiguration configuration, HttpClient http, SessionStorageService sessionService, WiDocApiApikeySettings apikeySettings)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        _http = http ?? throw new ArgumentNullException(nameof(http));
        _sessionService = sessionService ?? throw new ArgumentNullException(nameof(sessionService));
        _apikeySettings = apikeySettings ?? throw new ArgumentNullException(nameof(apikeySettings));
    }

    public async Task TryEndpoint(ApiEndpoint endpoint, string baseUrl)
    {
        var apiRequest = BuildApiRequest(endpoint, baseUrl);

        try
        {
            endpoint.IsLoading = true;
            endpoint.HasError = false;
            endpoint.ApiResponse = null;
            endpoint.ApiRequest = apiRequest;

            using var requestMessage = new HttpRequestMessage(HttpMethod.Parse(endpoint.HttpMethod.ToString()!), new Uri(apiRequest));

           var apiKeys = _apikeySettings.ApiKeys;
            var apikeyTest = string.Empty;

            if (await _sessionService.ApiKeyExists())
            {
                apikeyTest = (await _sessionService.GetFromSessionStorage("apiKey"));
            }

            if (!string.IsNullOrEmpty(apikeyTest))
            {
                requestMessage.Headers.Add(_apikeySettings.ApiKeyHeaderName!, apikeyTest);
            }


            if (IsContentRequired(endpoint.HttpMethod.ToString()!))
            {
                var jsonPayload = endpoint.Payload ?? string.Empty;
                requestMessage.Content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
            }

            using var response = await _http.SendAsync(requestMessage).ConfigureAwait(false);

            response.EnsureSuccessStatusCode();
            var rawJson = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            endpoint.ApiResponse = FormatJson(rawJson);

            _apiResponseCache.TryRemove(apiRequest, out _);
        }
        catch (Exception ex)
        {
            endpoint.HasError = true;
            endpoint.ApiResponse = $"Exception: {ex.Message}";
        }
        finally
        {
            endpoint.IsLoading = false;
        }
    }

    private string BuildApiRequest(ApiEndpoint endpoint, string baseUrl)
    {
        string formattedPath = endpoint.Path!.StartsWith("/") ? endpoint.Path : $"/{endpoint.Path}";

        string fullPath = Regex.Replace(formattedPath, @"\{(.*?)\}", match =>
        {
            var paramName = match.Groups[1].Value.Split(':')[0];

            if (endpoint.DynamicInputValues.TryGetValue(paramName, out var paramValue))
            {
                return paramValue?.ToString() ?? throw new KeyNotFoundException($"The given key '{paramName}' was not present in the dictionary.");
            }
            else
            {
                throw new KeyNotFoundException($"The given key '{paramName}' was not present in the dictionary.");
            }
        });

        var queryParameters = new List<string>();

        foreach (var kvp in endpoint.Parameters)
        {
            if (endpoint.DynamicInputValues.TryGetValue(kvp.Key, out var paramValue))
            {
                queryParameters.Add($"{kvp.Key}={Uri.EscapeDataString(paramValue.ToString()!)}");
            }
            else
            {
                throw new KeyNotFoundException($"The given key '{kvp.Key}' was not present in the dictionary.");
            }
        }

        var fullUrl = queryParameters.Count > 0 ? $"{baseUrl}{fullPath}?{string.Join("&", queryParameters)}" : $"{baseUrl}{fullPath}";

        return fullUrl;
    }

    private bool IsContentRequired(string method)
    {
        return method == "POST" || method == "PUT" || method == "PATCH";
    }

    private string FormatJson(string json)
    {
        return JsonSerializer.Serialize(JsonDocument.Parse(json).RootElement, new JsonSerializerOptions { WriteIndented = true });
    }
}
