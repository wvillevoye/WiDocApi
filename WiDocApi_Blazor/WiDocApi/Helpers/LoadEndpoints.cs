using WiDocApi_Blazor.WiDocApi.Models;
using static System.Net.WebRequestMethods;
using System.Text.Json;

namespace WiDocApi_Blazor.WiDocApi.Helpers
{
    public class LoadEndpoints
    {
        private readonly HttpClient _http;
    
        public List<ApiEndpoint> Endpoints { get; private set; } = [];

        public LoadEndpoints(HttpClient http)
        {
            _http = http;
        }
        public async Task<(string messages , List<ApiEndpoint> endpoints)> LoadEndpointsFromJsonFile(string baseUrl, string jsonFilePath )
        {
            try
            {

                var urlGetApiCalls = $"{baseUrl}/{jsonFilePath}";
                var response = await _http.GetAsync(urlGetApiCalls);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    Endpoints = JsonSerializer.Deserialize<List<ApiEndpoint>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
                    return ("", Endpoints.FindAll(d=>d.Active).ToList());
                }
                else
                {
                    // Handle the case where the JSON file is not found or not accessible
                    
                    return ("Error loading API endpoints from JSON file.", Endpoints);
                    
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur while loading the file
               
               return ($"Exception: {ex.Message}", Endpoints);
                 
            }
        
        }


    }
}
