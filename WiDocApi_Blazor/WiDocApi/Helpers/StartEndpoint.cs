using WiDocApi_Blazor.WiDocApi.Models;
using System.Text.Json;
using System.Net.Http.Headers;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace WiDocApi_Blazor.WiDocApi.Helpers
{
    public class StartEndpoint
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _http;
        private readonly ConcurrentDictionary<string, CachedApiResponse> _apiResponseCache = new ConcurrentDictionary<string, CachedApiResponse>();

        public StartEndpoint(IConfiguration configuration, HttpClient http)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _http = http ?? throw new ArgumentNullException(nameof(http));
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

                using var requestMessage = new HttpRequestMessage(HttpMethod.Parse(endpoint.Method!), new Uri(apiRequest));

                var apiKey = _configuration["ApiSettings:ValidApiKey"];
                if (!string.IsNullOrEmpty(apiKey))
                {
                    requestMessage.Headers.Add("X-Api-Key", apiKey);
                }

                if (IsContentRequired(endpoint.Method!))
                {
                    var jsonPayload = endpoint.Payload ?? string.Empty;
                    requestMessage.Content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
                }

                var stopwatch = Stopwatch.StartNew(); // Measure API time

                using var response = await _http.SendAsync(requestMessage).ConfigureAwait(false);
                stopwatch.Stop();

                response.EnsureSuccessStatusCode();
                var rawJson = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                endpoint.ApiResponse = FormatJson(rawJson);

                // Invalidate the cache for the updated record
                _apiResponseCache.TryRemove(apiRequest, out _);

                // Optional: Log API response time for diagnostics
                Console.WriteLine($"API Request {apiRequest} took {stopwatch.ElapsedMilliseconds} ms");
            }
            catch (Exception ex)
            {
                endpoint.HasError = true;
                endpoint.ApiResponse = $"Exception: {ex.Message}"; // Log detailed error message
            }
            finally
            {
                endpoint.IsLoading = false;
            }
        }

        private string BuildApiRequest(ApiEndpoint endpoint, string baseUrl)
        {
            // Ensure the path starts with a '/'
            string formattedPath = endpoint.Path!.StartsWith("/") ? endpoint.Path : $"/{endpoint.Path}";

            // Use Regex to replace each parameter in the path with the corresponding value from DynamicInputValues
            string fullPath = Regex.Replace(formattedPath, @"\{(.*?)\}", match =>
            {
                var paramName = match.Groups[1].Value;
                return endpoint.DynamicInputValues.TryGetValue(paramName, out var paramValue)
                    ? paramValue
                    : throw new KeyNotFoundException($"The given key '{paramName}' was not present in the dictionary.");
            });

            // Concatenate base URL with the path
            return $"{baseUrl}{fullPath}";
        }

        private bool IsContentRequired(string method)
        {
            return method == "POST" || method == "PUT" || method == "PATCH";
        }

        private string FormatJson(string json)
        {
            // Avoid unnecessary intermediate JSON deserialization for faster formatting
            return JsonSerializer.Serialize(JsonDocument.Parse(json).RootElement, new JsonSerializerOptions { WriteIndented = true });
        }
    }
}
